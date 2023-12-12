using Aquantica.API.Filters;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.JobControl;
using Aquantica.Core.Exceptions;
using Aquantica.Core.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobControlsController : ControllerBase
{
    private readonly IJobControlService _jobControlService;

    public JobControlsController(IJobControlService jobControlService)
    {
        _jobControlService = jobControlService;
    }

    [HttpGet("get-all-jobs")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllJobs()
    {
        try
        {
            var result = await _jobControlService.GetAllJobsAsync();

            return Ok(result.Data.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_ALL_JOBS").ToApiErrorResponse());
        }
    }

    [HttpGet("fire-job-as-method")]
    [CustomJwtAuthorize]
    public async Task<IActionResult> FireJobAsMethod(int jobId)
    {
        try
        {
            var result = await _jobControlService.FireJobAsMethodAsync(jobId);

            return Ok(result.ToApiResponse());
        }
        catch (JobException e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_FIRE_JOB_AS_METHOD").ToApiErrorResponse());
        }
    }

    [HttpGet("trigger-job")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> TriggerJob(int jobId)
    {
        try
        {
            var result = await _jobControlService.TriggerJobAsync(jobId);

            return Ok(result.ToApiResponse());
        }
        catch (JobException e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_TRIGGER_ONE_TIME_JOB").ToApiErrorResponse());
        }
    }

    [HttpGet("start-all-jobs")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> StartAllJobs()
    {
        try
        {
            var result = await _jobControlService.StartAllJobsAsync();

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_START_ALL_JOBS").ToApiErrorResponse());
        }
    }

    [HttpGet("start-job")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> StartJob(int jobId)
    {
        try
        {
            var result = await _jobControlService.StartJobAsync(jobId);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_START_JOB").ToApiErrorResponse());
        }
    }

    [HttpGet("stop-job")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> StopJob(int jobId)
    {
        try
        {
            var result = await _jobControlService.StopJobAsync(jobId);

            return Ok(result.ToApiResponse());
        }
        catch (JobException e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_STOP_JOB").ToApiErrorResponse());
        }
    }

    [HttpPost("create-job")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> CreateJob(CreateJobRequest request)
    {
        try
        {
            var result = await _jobControlService.CreateJobAsync(request);

            return Ok(result.Data.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_CREATE_JOB").ToApiErrorResponse());
        }
    }

    [HttpPut("update-job")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateJob(UpdateJobRequest request)
    {
        try
        {
            var result = await _jobControlService.UpdateJobAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_UPDATE_JOB").ToApiErrorResponse());
        }
    }

    [HttpDelete("delete-job")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteJob(int jobId)
    {
        try
        {
            var result = await _jobControlService.DeleteJobAsync(jobId);

            return Ok(result.Data.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_DELETE_JOB").ToApiErrorResponse());
        }
    }

    [HttpGet("stop-all-jobs")]
    [CustomJwtAuthorize(Roles = "Admin")]
    public async Task<IActionResult> StopAllJobs()
    {
        try
        {
            var result = await _jobControlService.StopAllJobsAsync();

            return Ok(result.Data.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_STOP_ALL_JOBS").ToApiErrorResponse());
        }
    }
}