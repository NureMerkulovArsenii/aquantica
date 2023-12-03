using Aquantica.Core.Entities;
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
            }
        };

        await _unitOfWork.AccessActionRepository.AddRangeAsync(accessActions);

        await _unitOfWork.SaveAsync();
        
        _logger.LogInformation("Seeder: Access actions generated");
    }

    private async Task GenerateRoles()
    {
        var accessActions = await _unitOfWork.AccessActionRepository
            .GetAll()
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
       
}