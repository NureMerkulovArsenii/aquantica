using Aquantica.BLL.Interfaces;
using Aquantica.DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace Aquantica.BLL.Services;

public class DataAdministrationService : IDataAdministrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISettingsService _settingsService;
    private readonly IConfiguration _configuration;

    public DataAdministrationService(
        IUnitOfWork unitOfWork,
        ISettingsService settingsService,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _settingsService = settingsService;
        _configuration = configuration;
    }

    public async Task CreateDataBaseBackupAsync()
    {
        var databaseConnectionString = _configuration.GetConnectionString("DefaultConnection");

        var databaseName = databaseConnectionString.Split(";")[1].Split("=")[1];

        var backupSetting = _settingsService.GetStringSetting("BACKUP_PATH");

        var backupPath = backupSetting.Value;

        var backupFileName = $"{databaseName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.bak";

        var backupFilePath = Path.Combine(backupPath, backupFileName);

        var sql = $"BACKUP DATABASE {databaseName} TO DISK = '{backupFilePath}'";

        await _unitOfWork.ExecuteSqlRawAsync(sql);
    }
}