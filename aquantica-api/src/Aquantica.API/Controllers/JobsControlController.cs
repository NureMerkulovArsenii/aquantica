using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.JobControl;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsControlController : ControllerBase
{
    private readonly IJobControlService _jobControlService;

    public JobsControlController(IJobControlService jobControlService)
    {
        _jobControlService = jobControlService;
    }

    [HttpGet("get-all-jobs")]
    public async Task<IActionResult> GetAllJobs()
    {
        try
        {
            var result = await _jobControlService.GetAllJobsAsync();

            return Ok(result.Data.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("fire-job-as-method")]
    public async Task<IActionResult> FireJobAsMethod(int jobId)
    {
        try
        {
            var result = await _jobControlService.FireJobAsMethodAsync(jobId);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("trigger-job")]
    public async Task<IActionResult> TriggerJob(int jobId)
    {
        try
        {
            var result = await _jobControlService.TriggerJobAsync(jobId);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("start-job")]
    public async Task<IActionResult> StartJob(int jobId)
    {
        try
        {
            var result = await _jobControlService.StartJobAsync(jobId);

            if (result)
                return Ok(result.ToApiResponse());

            return BadRequest("Job not found");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("stop-job")]
    public async Task<IActionResult> StopJob(int jobId)
    {
        try
        {
            var result = await _jobControlService.StopJobAsync(jobId);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("create-job")]
    public async Task<IActionResult> CreateJob(CreateJobRequest request)
    {
        try
        {
            var result = await _jobControlService.CreateJobAsync(request);

            if (result.IsSuccess)
                return Ok(result.Data.ToApiResponse());

            return BadRequest(result.ErrorMessage.ToApiErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpPost("update-job")]
    public async Task<IActionResult> UpdateJob(UpdateJobRequest request)
    {
        try
        {
            var result = await _jobControlService.UpdateJobAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpGet("delete-job")]
    public async Task<IActionResult> DeleteJob(int jobId)
    {
        try
        {
            var result = await _jobControlService.DeleteJobAsync(jobId);

            if (result.IsSuccess)
                return Ok(result.Data.ToApiResponse());

            return BadRequest(result.ErrorMessage.ToApiErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpGet("stop-all-jobs")]
    public async Task<IActionResult> StopAllJobs()
    {
        try
        {
            var result = await _jobControlService.StopAllJobsAsync();
            if (result.IsSuccess)
                return Ok(result.Data.ToApiResponse());

            return BadRequest(result.ErrorMessage.ToApiErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }
}