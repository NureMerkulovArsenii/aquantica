using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Core.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataAdministrationController : ControllerBase
{
    private readonly IDataAdministrationService _dataAdministrationService;

    public DataAdministrationController(IDataAdministrationService dataAdministrationService)
    {
        _dataAdministrationService = dataAdministrationService;
    }

    [HttpPost("backup")]
    public async Task<IActionResult> CreateDataBaseBackupAsync()
    {
        try
        {
            await _dataAdministrationService.CreateDataBaseBackupAsync();

            return Ok(Resources.Get("BACKUP_CREATED").ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_CREATE_BACKUP").ToApiErrorResponse());
        }
    }
}
