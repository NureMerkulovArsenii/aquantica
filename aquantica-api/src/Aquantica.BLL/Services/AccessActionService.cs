using Aquantica.Contracts.Requests.AccessActions;
using Aquantica.Contracts.Responses.AccessActions;
using Aquantica.Core.DTOs.Role;
using Aquantica.Core.Entities;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.BLL.Services;

public class AccessActionService
{
    private readonly IUnitOfWork _unitOfWork;

    public AccessActionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<AccessActionResponse>> GetAccessActionsAsync()
    {
        var accessActions = await _unitOfWork.AccessActionRepository
            .GetAll()
            .Select(x => new AccessActionResponse
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                RoleIds = x.Roles.Select(x => x.Id).ToList()
            })
            .AsNoTracking()
            .ToListAsync();

        return accessActions;
    }

    public async Task<AccessActionDetailedResponse> GetAccessActionByIdAsync(int id)
    {
        var accessAction = await _unitOfWork.AccessActionRepository
            .GetAllByCondition(x => x.Id == id)
            .Select(x => new AccessActionDetailedResponse
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                Roles = x.Roles.Select(x => new RoleDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsEnabled = x.IsEnabled,
                    IsBlocked = x.IsBlocked,
                    IsDefault = x.IsDefault,
                }).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return accessAction;
    }

    public async Task<bool> CreateAccessActionAsync(CreateAccessActionRequest request)
    {
        var accessAction = new AccessAction
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
        };

        await _unitOfWork.AccessActionRepository.AddAsync(accessAction);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<bool> UpdateAccessActionAsync(UpdateAccessActionRequest request)
    {
        var accessAction = await _unitOfWork.AccessActionRepository
            .GetAllByCondition(x => x.Id == request.Id)
            .FirstOrDefaultAsync();

        if (accessAction == null)
            throw new Exception("AccessAction not found");

        accessAction.Code = request.Code;
        accessAction.Name = request.Name;
        accessAction.Description = request.Description;
        accessAction.IsEnabled = request.IsEnabled;

        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<bool> DeleteAccessActionAsync(int id)
    {
        var accessAction = await _unitOfWork.AccessActionRepository
            .GetAllByCondition(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (accessAction == null)
            throw new Exception("AccessAction not found");

        _unitOfWork.AccessActionRepository.Delete(accessAction);

        await _unitOfWork.SaveAsync();

        return true;
    }
}