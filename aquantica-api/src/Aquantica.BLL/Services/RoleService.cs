using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests.Roles;
using Aquantica.Core.DTOs.Role;
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
                //ToDo: Add AccessActions and Users
                AccessActionsIds = x.AccessActions.Select(action => action.Id).ToList(),
                UserIds = x.Users.Select(user => user.Id).ToList()
            })
            .FirstOrDefaultAsync();

        return role;
    }

    public async Task<RoleDTO> CreateRoleAsync(CreateRoleRequest request)
    {
        var accessActions = await _unitOfWork.AccessActionRepository
            .GetAllByCondition(x => request.AccessActionsIds.Contains(x.Id))
            .ToListAsync();

        var users = await _unitOfWork.UserRepository
            .GetAllByCondition(x => request.UserIds.Contains(x.Id))
            .ToListAsync();

        var role = new Role()
        {
            Name = request.Name,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
            IsBlocked = request.IsBlocked,
            IsDefault = request.IsDefault,
            AccessActions = accessActions,
            Users = users
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


        var users = await _unitOfWork.UserRepository
            .GetAllByCondition(x => request.UserIds.Contains(x.Id))
            .ToListAsync();
        
        var accessActions = await _unitOfWork.AccessActionRepository
            .GetAllByCondition(x => request.AccessActionsIds.Contains(x.Id))
            .ToListAsync();

        role.Name = request.Name;
        role.Description = request.Description;
        role.IsEnabled = request.IsEnabled;
        role.IsBlocked = request.IsBlocked;
        role.IsDefault = request.IsDefault;
        role.AccessActions = accessActions;
        role.Users = users;

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