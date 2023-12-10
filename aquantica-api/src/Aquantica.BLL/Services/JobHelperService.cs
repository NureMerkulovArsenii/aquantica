using Aquantica.BLL.Interfaces;
using Aquantica.Core.Constants;
using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;
using Aquantica.DAL.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Aquantica.BLL.Services;

public class JobHelperService : IJobHelperService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<JobHelperService> _logger;

    public JobHelperService(IUnitOfWork unitOfWork,
        ILogger<JobHelperService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public void AddJobEventRecord(BackgroundJobDTO job, bool isStart, bool isError = false, string message = null)
    {
        try
        {
            string eventMessage;

            if (string.IsNullOrEmpty(message))
            {
                if (isError)
                    eventMessage = JobConstants.JOB_ERROR_MESSAGE;
                else
                    eventMessage = isStart ? JobConstants.JOB_STARTED_MESSAGE : JobConstants.JOB_FINISHED_MESSAGE;
            }
            else
            {
                eventMessage = message;
            }

            var jobEvent = new BackgroundJobEvent
            {
                IsStart = isStart,
                EventMessage = eventMessage,
                BackgroundJobId = job.Id
            };

            _unitOfWork.BackgroundJobEventRepository.Add(jobEvent);

            _unitOfWork.Save();

            _logger.LogInformation($"Job {job.Name} {eventMessage}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}