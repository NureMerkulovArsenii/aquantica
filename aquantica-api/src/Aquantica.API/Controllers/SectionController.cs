using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
    private readonly ISectionService _sectionService;

    public SectionController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [HttpGet("allSections")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sectionService.GetAllSectionsAsync();

            return Ok(result.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpGet("section/{id}")]
    public async Task<IActionResult> GetSectionById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sectionService.GetSectionByIdAsync(id);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSection(CreateSectionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sectionService.CreateSectionAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateSection(UpdateSectionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sectionService.UpdateSectionAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteSection(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sectionService.DeleteSectionAsync(id);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }


    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(GetIrrigationHistoryRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _sectionService.GetIrrigationHistoryAsync(request);

            return Ok(result.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }
}