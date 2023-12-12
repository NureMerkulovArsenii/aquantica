using System.Linq.Expressions;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests.JobControl;
using Aquantica.Contracts.Responses.JobControl;
using Aquantica.Core.Constants;
using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;
using Aquantica.Core.Enums;
using Aquantica.Core.Exceptions;
using Aquantica.Core.Resources;
using Aquantica.Core.ServiceResult;
using Aquantica.DAL.UnitOfWork;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BackgroundJob = Aquantica.Core.Entities.BackgroundJob;

namespace Aquantica.BLL.Services;

public class JobControlService : IJobControlService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly IArduinoControllersService _arduinoControllersService;
    private readonly CustomUserManager _userManager;
    private readonly ILogger<JobControlService> _logger;

    public JobControlService(IUnitOfWork unitOfWork,
        IWeatherForecastService weatherForecastService,
        IArduinoControllersService arduinoControllersService,
        CustomUserManager userManager,
        ILogger<JobControlService> logger)
    {
        _unitOfWork = unitOfWork;
        _weatherForecastService = weatherForecastService;
        _arduinoControllersService = arduinoControllersService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ServiceResult<List<JobResponse>>> GetAllJobsAsync()
    {
        var jobs = await _unitOfWork.BackgroundJobRepository
            .GetAll()
            .Select(x => new JobResponse
            {
                Id = x.Id,
                Name = x.Name,
                IsEnabled = x.IsEnabled,
                JobRepetitionType = x.JobRepetitionType,
                JobRepetitionValue = x.JobRepetitionValue,
                JobMethod = x.JobMethod,
            })
            .ToListAsync();

        return new ServiceResult<List<JobResponse>>(jobs);
    }


    public async Task<ServiceResult<bool>> FireJobAsMethodAsync(int jobId)
    {
        var userAccessActions = _userManager.GetCurrentUserAccessAction();

        var accessAction = userAccessActions.FirstOrDefault(x => x.Code == "FIRE_JOB_AS_METHOD");

        if (accessAction == null)
            throw new JobException(Resources.Get("ACCESS_DENIED"));

        var job = await _unitOfWork.BackgroundJobRepository.GetByIdAsync(jobId);

        if (job == null)
            throw new JobException(Resources.Get("JOB_NOT_FOUND"));

        var method = GetJobMethod(job);

        method.Compile().Invoke();

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> TriggerJobAsync(int jobId)
    {
        var job = await _unitOfWork.BackgroundJobRepository.GetByIdAsync(jobId);

        if (job == null)
            throw new JobException(Resources.Get("JOB_NOT_FOUND"));

        RecurringJob.TriggerJob(job.Name);

        return new ServiceResult<bool>(true);
    }


    public async Task<bool> StartJobAsync(int jobId)
    {
        var job = await _unitOfWork.BackgroundJobRepository.GetByIdAsync(jobId);

        if (job == null)
            return false;

        job.IsEnabled = true;

        _unitOfWork.BackgroundJobRepository.Update(job);

        await _unitOfWork.SaveAsync();

        RecurringJob.AddOrUpdate(job.Name, GetJobMethod(job), job.CronExpression);

        return true;
    }


    public async Task<bool> StopJobAsync(int jobId)
    {
        var job = await _unitOfWork.BackgroundJobRepository.GetByIdAsync(jobId);

        if (job == null)
            throw new JobException(Resources.Get("JOB_NOT_FOUND"));

        job.IsEnabled = false;

        _unitOfWork.BackgroundJobRepository.Update(job);

        await _unitOfWork.SaveAsync();

        RecurringJob.RemoveIfExists(job.Name);

        return true;
    }

    public async Task<ServiceResult<bool>> StartAllJobsAsync()
    {
        var jobs = await _unitOfWork.BackgroundJobRepository
            .GetAll()
            .ToListAsync();

        foreach (var job in jobs)
        {
            RecurringJob.AddOrUpdate(job.Name, GetJobMethod(job), job.CronExpression);

            job.IsEnabled = true;

            _unitOfWork.BackgroundJobRepository.Update(job);
        }

        await _unitOfWork.SaveAsync();

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> StopAllJobsAsync()
    {
        using var connection = JobStorage.Current.GetConnection();

        foreach (var recurringJob in connection.GetRecurringJobs())
        {
            RecurringJob.RemoveIfExists(recurringJob.Id);

            var job = await _unitOfWork.BackgroundJobRepository.FirstOrDefaultAsync(x => x.Name == recurringJob.Id);
            if (job != null)
                job.IsEnabled = false;

            _unitOfWork.BackgroundJobRepository.Update(job);
        }

        await _unitOfWork.SaveAsync();

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> CreateJobAsync(CreateJobRequest request)
    {
        var jobCheck = await _unitOfWork.BackgroundJobRepository.ExistAsync(x => x.Name == request.Name);

        if (jobCheck)
            throw new JobException(Resources.Get("JOB_ALREADY_EXISTS"));

        var job = new BackgroundJob
        {
            Name = request.Name,
            IsEnabled = true,
            JobRepetitionType = request.JobRepetitionType,
            JobRepetitionValue = request.JobRepetitionValue,
            JobMethod = request.JobMethod,
            CronExpression = GetCronExpression(request.JobRepetitionType, request.JobRepetitionValue)
        };

        var method = GetJobMethod(job);

        await _unitOfWork.BackgroundJobRepository.AddAsync(job);

        await _unitOfWork.SaveAsync();

        RecurringJob.AddOrUpdate(job.Name, method, job.CronExpression);

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> UpdateJobAsync(UpdateJobRequest request)
    {
        var job = await _unitOfWork.BackgroundJobRepository.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (job == null)
            throw new JobException(Resources.Get("JOB_NOT_FOUND"));

        RecurringJob.RemoveIfExists(job.Name);
        
        job.Name = request.Name;
        job.IsEnabled = request.IsEnabled;
        job.JobRepetitionType = request.JobRepetitionType;
        job.JobRepetitionValue = request.JobRepetitionValue;
        job.JobMethod = request.JobMethod;
        job.CronExpression = GetCronExpression(request.JobRepetitionType, request.JobRepetitionValue);

        var method = GetJobMethod(job);

        _unitOfWork.BackgroundJobRepository.Update(job);

        await _unitOfWork.SaveAsync();

        RecurringJob.AddOrUpdate(job.Name, method, job.CronExpression);

        return new ServiceResult<bool>(true);
    }

    public async Task<ServiceResult<bool>> DeleteJobAsync(int id)
    {
        try
        {
            var job = await _unitOfWork.BackgroundJobRepository.GetByIdAsync(id);

            if (job == null)
                throw new JobException(Resources.Get("JOB_NOT_FOUND"));

            RecurringJob.RemoveIfExists(job.Name);

            _unitOfWork.BackgroundJobRepository.Delete(job);

            await _unitOfWork.SaveAsync();

            return new ServiceResult<bool>(true);
        }
        catch (Exception e)
        {
            return new ServiceResult<bool>(e.Message);
        }
    }


    private Expression<Action> GetJobMethod(BackgroundJob job)
    {
        var jobDto = new BackgroundJobDTO
        {
            Id = job.Id,
            Name = job.Name,
            IsEnabled = job.IsEnabled,
            JobRepetitionType = job.JobRepetitionType,
            JobRepetitionValue = job.JobRepetitionValue,
            JobMethod = job.JobMethod,
            CronExpression = job.CronExpression,
            IrrigationSectionId = job.IrrigationSectionId
        };

        return job.JobMethod switch
        {
            JobMethodEnum.GetWeatherForecast => () => _weatherForecastService.GetWeatherForecastsFromApi(jobDto),
            JobMethodEnum.StartIrrigation => () => _arduinoControllersService.StartIrrigationIfNeeded(jobDto),
            JobMethodEnum.StopIrrigation => () => _arduinoControllersService.StopIrrigation(jobDto),
            JobMethodEnum.CollectSensorData => () => _arduinoControllersService.GetControllerData(jobDto),
            _ => throw new ArgumentOutOfRangeException(nameof(job), job.JobMethod, "Invalid job method")
        };
    }

    private string GetCronExpression(JobRepetitionType repetitionType, int value)
    {
        return repetitionType switch
        {
            JobRepetitionType.Seconds => $"*/{value} * * * * *",
            JobRepetitionType.Minutes => $"* */{value} * * * *",
            JobRepetitionType.Hours => $"* * */{value} * * *",
            JobRepetitionType.Days => $"* * * */{value} * *",
            JobRepetitionType.Weeks => $"* * * * */{value} *",
            JobRepetitionType.Months => $"* * * * * */{value}",
            _ => throw new ArgumentOutOfRangeException(nameof(repetitionType), repetitionType,
                "Invalid repetition type")
        };
    }
}