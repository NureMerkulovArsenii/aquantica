using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Responses.IrrigationSection;
using Aquantica.Core.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class SectionController : ControllerBase
{
    private readonly ISectionService _sectionService;

    public SectionController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _sectionService.GetAllSectionsAsync();

            if (result == null)
                return NotFound(Resources.Get("SECTIONS_NOT_FOUND").ToApiErrorResponse());

            var response = result.Select(x => new IrrigationSectionResponse
            {
                Id = x.Id,
                Number = x.Number,
                Name = x.Name,
                ParentId = x.ParentId,
                IsEnabled = x.IsEnabled,
                DeviceUri = x.DeviceUri,
                SectionRulesetId = x.SectionRulesetId,
                LocationName = x.LocationName,
            });

            return Ok(response.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_SECTIONS").ToApiErrorResponse());
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSectionById(int id)
    {
        try
        {
            var result = await _sectionService.GetSectionByIdAsync(id);

            if (result == null)
                return NotFound(Resources.Get("SECTION_NOT_FOUND").ToApiErrorResponse());

            var response = new IrrigationSectionDetailedResponse()
            {
                Id = result.Id,
                Number = result.Number,
                Name = result.Name,
                ParentId = result.ParentId,
                IsEnabled = result.IsEnabled,
                DeviceUri = result.DeviceUri,
                SectionTypeId = result.SectionTypeId,
                SectionRulesetId = result.SectionRulesetId,
                Location = result.Location,
            };

            return Ok(response.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_SECTION").ToApiErrorResponse());
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSection(CreateSectionRequest request)
    {
        try
        {
            var result = await _sectionService.CreateSectionAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_CREATE_SECTION").ToApiErrorResponse());
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateSection(UpdateSectionRequest request)
    {
        try
        {
            var result = await _sectionService.UpdateSectionAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_UPDATE_SECTION").ToApiErrorResponse());
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteSection(int id)
    {
        try
        {
            var result = await _sectionService.DeleteSectionAsync(id);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_DELETE_SECTION").ToApiErrorResponse());
        }
    }


    [HttpGet("irrigation-history")]
    public async Task<IActionResult> GetHistory(GetIrrigationHistoryRequest request)
    {
        try
        {
            var result = await _sectionService.GetIrrigationHistoryAsync(request);

            return Ok(result.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_IRRIGATION_HISTORY").ToApiErrorResponse());
        }
    }
}