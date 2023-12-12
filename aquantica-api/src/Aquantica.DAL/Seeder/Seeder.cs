using System.Security.Cryptography;
using System.Text;
using Aquantica.Core.Entities;
using Aquantica.Core.Enums;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aquantica.DAL.Seeder;

public class Seeder : ISeeder
{
    private readonly ILogger<Seeder> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public Seeder(ILogger<Seeder> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedIfNeededAsync()
    {
        _logger.LogInformation("Seeder: Checking if seed is needed");
        var accessActionsExist = await _unitOfWork.AccessActionRepository.ExistAsync(null);

        if (!accessActionsExist)
        {
            await GenerateAccessActionsAsync();
            await GenerateRoles();
            await GenerateLocations();
            await GenerateIrrigationSectionTypes();
            await GenerateSections();
            await GenerateSettings();
            await GenerateBackgroundJobs();
            await GenerateRulesets();
            await GenerateUsers();
            await GenerateMenu();
        }
        else
        {
            _logger.LogInformation("Seeder: Seed is not needed");
        }

        _logger.LogInformation("Seeder: Seed completed");
    }

    private async Task GenerateAccessActionsAsync()
    {
        var accessActions = new List<AccessAction>
        {
            new AccessAction
            {
                Name = "Create",
                Code = "create",
                Description = "Create action",
                IsEnabled = true
            },
            new AccessAction
            {
                Name = "Read",
                Code = "read",
                Description = "Read action",
                IsEnabled = true
            },
            new AccessAction
            {
                Name = "Update",
                Code = "update",
                Description = "Update action",
                IsEnabled = true
            },
            new AccessAction
            {
                Name = "Delete",
                Code = "delete",
                Description = "Delete action",
                IsEnabled = true
            },
            new AccessAction
            {
                Name = "Fire job as method",
                Code = "FIRE_JOB_AS_METHOD",
                Description = "Fire job as method",
                IsEnabled = true
            },
        };

        await _unitOfWork.AccessActionRepository.AddRangeAsync(accessActions);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Access actions generated");
    }

    private async Task GenerateRoles()
    {
        var accessActions = await _unitOfWork.AccessActionRepository
            .GetAllByCondition(x => x.Code != "FIRE_JOB_AS_METHOD")
            .ToListAsync();

        var adminRole = new Role
        {
            Name = "Admin",
            Description = "Admin role",
            IsEnabled = true,
            IsDefault = false,
            IsBlocked = false,
            AccessActions = accessActions
        };

        var userRole = new Role
        {
            Name = "User",
            Description = "User role",
            IsEnabled = true,
            IsDefault = true,
            IsBlocked = false,
            AccessActions = accessActions.Where(x => x.Code == "read").ToList()
        };

        await _unitOfWork.RoleRepository.AddRangeAsync(new List<Role> { adminRole, userRole });

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Roles generated");
    }

    private async Task GenerateLocations()
    {
        var locations = new List<Location>
        {
            new()
            {
                Name = "Root",
                Latitude = 49.9935,
                Longitude = 36.2304,
            },
            new()
            {
                Name = "IrrigationSection 1",
                Latitude = 49.9935,
                Longitude = 36.2304,
            }
        };

        await _unitOfWork.LocationRepository.AddRangeAsync(locations);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Locations generated");
    }

    private async Task GenerateIrrigationSectionTypes()
    {
        var sectionTypes = new List<IrrigationSectionType>
        {
            new()
            {
                Name = "Field",
                Description = "Field",
            },
            new()
            {
                Name = "Greenhouse",
                Description = "Greenhouse",
            }
        };

        await _unitOfWork.SectionTypeRepository.AddRangeAsync(sectionTypes);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: IrrigationSection types generated");
    }

    private async Task GenerateSections()
    {
        var locations = await _unitOfWork.LocationRepository
            .GetAll()
            .ToListAsync();

        var sectionTypes = await _unitOfWork.SectionTypeRepository.GetAll().ToListAsync();

        var rootSection = new IrrigationSection
        {
            Name = "Root",
            Number = 0,
            IsEnabled = true,
            DeviceUri = "127.0.0.1:8180",
            Location = locations[0],
            IrrigationSectionType = sectionTypes[0]
        };

        var section1 = new IrrigationSection
        {
            Name = "IrrigationSection 1",
            Number = 1,
            IsEnabled = true,
            DeviceUri = "127.0.0.1:8180",
            Location = locations[1],
            IrrigationSectionType = sectionTypes[1],
            ParentSection = rootSection
        };

        await _unitOfWork.SectionRepository.AddRangeAsync(new List<IrrigationSection> { rootSection, section1 });

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Sections generated");
    }

    private async Task GenerateSettings()
    {
        var settings = new List<Setting>
        {
            new()
            {
                Name = "Is Irrigation Enabled",
                Code = "IS_IRRIGATION_ENABLED",
                Value = "true",
                Description = "Is irrigation enabled",
                ValueType = SettingValueType.Boolean
            },
            new()
            {
                Name = "Is Weather Forecast Enabled",
                Code = "IS_WEATHER_FORECAST_ENABLED",
                Value = "true",
                Description = "Is weather forecast enabled",
                ValueType = SettingValueType.Boolean
            },
            new()
            {
                Name = "Remove Weather Record Days Threshold",
                Code = "REMOVE_WEATHER_RECORD_DAYS_THRESHOLD",
                Value = "7",
                Description = "Remove weather record days threshold",
                ValueType = SettingValueType.Number,
            },
            new()
            {
                Name = "Backup Path",
                Code = "BACKUP_PATH",
                Value = "D:\\aquantica-backups",
                Description = "Backup path",
                ValueType = SettingValueType.String,
            },
        };

        await _unitOfWork.SettingsRepository.AddRangeAsync(settings);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Settings generated");
    }

    private async Task GenerateBackgroundJobs()
    {
        var jobs = new List<BackgroundJob>
        {
            new()
            {
                Name = "StartIrrigation",
                IsEnabled = true,
                JobRepetitionType = JobRepetitionType.Minutes,
                JobRepetitionValue = 1,
                JobMethod = JobMethodEnum.StartIrrigation,
                IrrigationSectionId = 1,
                CronExpression = "*/2 * * * *",
            },
            new()
            {
                Name = "StopIrrigation",
                IsEnabled = true,
                JobRepetitionType = JobRepetitionType.Minutes,
                JobRepetitionValue = 1,
                JobMethod = JobMethodEnum.StopIrrigation,
                IrrigationSectionId = 1,
                CronExpression = "*/2 * * * *",
            },
            new()
            {
                Name = "GetWeatherForecast",
                IsEnabled = true,
                JobRepetitionType = JobRepetitionType.Seconds,
                JobRepetitionValue = 15,
                JobMethod = JobMethodEnum.GetWeatherForecast,
                IrrigationSectionId = 1,
                CronExpression = "*/15 * * * * *",
            },
            new()
            {
                Name = "CollectSensorData",
                IsEnabled = true,
                JobRepetitionType = JobRepetitionType.Minutes,
                JobRepetitionValue = 1,
                JobMethod = JobMethodEnum.CollectSensorData,
                IrrigationSectionId = 1,
                CronExpression = "*/30 * * * * *",
            }
        };

        await _unitOfWork.BackgroundJobRepository.AddRangeAsync(jobs);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Background jobs generated");
    }

    private async Task GenerateRulesets()
    {
        var rulesets = new List<IrrigationRuleset>
        {
            new()
            {
                Name = "Ruleset 1",
                Description = "Ruleset 1",
                IsEnabled = true,
                TemperatureThreshold = 10,
                OptimalSoilHumidity = 55,
                MinSoilHumidityThreshold = 45,
                WaterConsumptionThreshold = 0.5,
                WaterConsumptionPerMinute = 0.5,
                HumidityGrowthPerLiterConsumed = 0.5,
                HumidityGrowthPerRainMm = 0.5,
                RainAmountThreshold = 0.3,
                RainProbabilityThreshold = 50,
                RainAvoidanceDurationThreshold = TimeSpan.FromHours(1),
                IsIrrigationDurationEnabled = true,
                IrrigationDuration = TimeSpan.FromHours(1),
                RainAvoidanceEnabled = true,
                IrrigationSections = _unitOfWork.SectionRepository.GetAll().ToList()
            },
        };

        await _unitOfWork.RulesetRepository.AddRangeAsync(rulesets);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Rulesets generated");
    }

    private async Task GenerateUsers()
    {
        var hmac = new HMACSHA256();

        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123456"));

        var passSalt = hmac.Key;

        var hashString = Convert.ToBase64String(passwordHash);

        var salTString = Convert.ToBase64String(passSalt);

        var user = new User
        {
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@company.com",
            PasswordHash = hashString,
            PasswordSalt = salTString,
            IsEnabled = true,
            IsBlocked = false,
            Role = _unitOfWork.RoleRepository.GetAll().FirstOrDefault(x => x.Name == "Admin"),
        };

        await _unitOfWork.UserRepository.AddAsync(user);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Users generated");
    }

    private async Task GenerateMenu()
    {
        var readAccessAction = await _unitOfWork.AccessActionRepository
            .GetAllByCondition(x => x.Code == "read")
            .FirstOrDefaultAsync();

        var menuItems = new List<MenuItem>
        {
            new()
            {
                Name = "Dashboard",
                Icon = "dashboard",
                Url = "/dashboard",
                Order = 1,
                AccessAction = readAccessAction,
            },
            new()
            {
                Name = "Irrigation",
                Icon = "water",
                Url = "/irrigation",
                Order = 2,
                AccessAction = readAccessAction,
            },
            new()
            {
                Name = "Settings",
                Icon = "settings",
                Url = "/settings",
                Order = 3,
                AccessAction = readAccessAction,
            },
            new()
            {
                Name = "Users",
                Icon = "people",
                Url = "/users",
                Order = 4,
                AccessAction = readAccessAction,
            },
            new()
            {
                Name = "Roles",
                Icon = "people",
                Url = "/roles",
                Order = 5,
                AccessAction = readAccessAction,
            },
            new()
            {
                Name = "Sections",
                Icon = "people",
                Url = "/sections",
                Order = 6,
                AccessAction = readAccessAction,
            }
        };

        await _unitOfWork.MenuItemRepository.AddRangeAsync(menuItems);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Seeder: Menu items generated");
    }
}