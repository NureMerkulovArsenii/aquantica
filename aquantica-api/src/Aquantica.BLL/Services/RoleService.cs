using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests.Roles;
using Aquantica.Core.DTOs.Role;
using Aquantica.Core.DTOs.User;
using Aquantica.Core.Entities;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.BLL.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<RoleDTO>> GetAllRolesAsync()
    {
        var roles = await _unitOfWork.RoleRepository
            .GetAll()
            .Select(x => new RoleDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                IsBlocked = x.IsBlocked,
                IsDefault = x.IsDefault
            })
            .ToListAsync();

        return roles;
    }

    public async Task<RoleDetailedDTO> GetRoleByIdAsync(int id)
    {
        var role = await _unitOfWork.RoleRepository
            .GetAllByCondition(x => x.Id == id)
            .Select(x => new RoleDetailedDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                IsBlocked = x.IsBlocked,
                IsDefault = x.IsDefault,
                AccessActions = x.AccessActions.Select(action => new AccessActionDTO
                {
                    Id = action.Id,
                    Code = action.Code,
                    Name = action.Name,
                    Description = action.Description,
                    IsEnabled = action.IsEnabled
                }).ToList(),
                UserIds = x.Users.Select(user => user.Id).ToList()
            })
            .FirstOrDefaultAsync();

        return role;
    }

    public async Task<RoleDTO> CreateRoleAsync(CreateRoleRequest request)
    {
        List<AccessAction> accessActions = null;
        if (request.AccessActionsIds != null)
            accessActions = await _unitOfWork.AccessActionRepository
                .GetAllByCondition(x => request.AccessActionsIds.Contains(x.Id))
                .ToListAsync();

        var role = new Role()
        {
            Name = request.Name,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
            IsBlocked = request.IsBlocked,
            IsDefault = request.IsDefault,
            AccessActions = accessActions,
        };

        await _unitOfWork.RoleRepository.AddAsync(role);

        await _unitOfWork.SaveAsync();

        var response = new RoleDTO()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsEnabled = role.IsEnabled,
            IsBlocked = role.IsBlocked,
            IsDefault = role.IsDefault
        };

        return response;
    }

    public async Task<RoleDTO> UpdateRoleAsync(UpdateRoleRequest request)
    {
        var role = await _unitOfWork.RoleRepository
            .GetAllByCondition(x => x.Id == request.Id)
            .FirstOrDefaultAsync();

        if (role == null)
            throw new Exception();
        
        List<AccessAction> accessActions = null;
        if (request.AccessActionsIds != null)
            accessActions = await _unitOfWork.AccessActionRepository
                .GetAllByCondition(x => request.AccessActionsIds.Contains(x.Id))
                .ToListAsync();

        role.Name = request.Name;
        role.Description = request.Description;
        role.IsEnabled = request.IsEnabled;
        role.IsBlocked = request.IsBlocked;
        role.IsDefault = request.IsDefault;
        role.AccessActions = accessActions;

        _unitOfWork.RoleRepository.Update(role);

        await _unitOfWork.SaveAsync();

        var response = new RoleDTO()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsEnabled = role.IsEnabled,
            IsBlocked = role.IsBlocked,
            IsDefault = role.IsDefault
        };

        return response;
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        var role = await _unitOfWork.RoleRepository
            .ExistAsync(x => x.Id == id);

        if (!role)
            throw new Exception();

        await _unitOfWork.RoleRepository.DeleteAsync(x => x.Id == id);

        await _unitOfWork.SaveAsync();

        return true;
    }
}