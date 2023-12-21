using Aquantica.Contracts.Requests.JobControl;
using Aquantica.Contracts.Responses.JobControl;
using Aquantica.Core.Entities;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface IJobControlService
{
    Task<ServiceResult<List<JobResponse>>> GetAllJobsAsync();
    
    Task<ServiceResult<JobDetailedResponse>> GetJobByIdAsync(int id);

    Task<ServiceResult<bool>> FireJobAsMethodAsync(int jobId);

    Task<ServiceResult<bool>> TriggerJobAsync(int jobId);

    Task<bool> StartJobAsync(int jobId);

    Task<ServiceResult<bool>> StartAllJobsAsync();

    Task<bool> StopJobAsync(int jobId);

    Task<ServiceResult<bool>> StopAllJobsAsync();

    Task<ServiceResult<bool>> CreateJobAsync(CreateJobRequest request);

    Task<ServiceResult<bool>> UpdateJobAsync(UpdateJobRequest request);

    Task<ServiceResult<bool>> DeleteJobAsync(int id);
}