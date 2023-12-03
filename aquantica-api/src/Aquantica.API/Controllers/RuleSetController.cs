using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.Rulesets;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RuleSetController : ControllerBase
{
    private readonly IRuleSetService _ruleSetService;

    public RuleSetController(IRuleSetService ruleSetService)
    {
        _ruleSetService = ruleSetService;
    }
    
    [HttpGet("rulesets")]
    public async Task<IActionResult> GetRuleSets(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ruleSetService.GetAllRuleSetsAsync();

            return Ok(result.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpGet("ruleset/{id}")]
    public async Task<IActionResult> GetRuleSetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ruleSetService.GetRuleSetByIdAsync(id);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpPost("ruleset")]
    public async Task<IActionResult> CreateRuleSet(CreateRuleSetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ruleSetService.CreateRuleSetAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpPut("ruleset")]
    public async Task<IActionResult> UpdateRuleSet(UpdateRuleSetRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ruleSetService.UpdateRuleSetAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpDelete("ruleset/{id}")]
    public async Task<IActionResult> DeleteRuleSet(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ruleSetService.DeleteRuleSetAsync(id);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpGet("ruleset/section/{sectionId}")]
    public async Task<IActionResult> GetRuleSetsBySectionId(int sectionId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ruleSetService.GetRuleSetsBySectionIdAsync(sectionId);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpPost("ruleset/assign")]
    public async Task<IActionResult> AssignRuleSetToSection(AssignRuleSetToSectionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _ruleSetService.AssignRuleSetToSectionAsync(request.RuleSetId, request.SectionId);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
}