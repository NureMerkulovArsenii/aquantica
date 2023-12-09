using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Responses;
using Aquantica.Core.DTOs;
using Aquantica.Core.ServiceResult;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.BLL.Services;

public class MenuService : IMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CustomUserManager _userManager;

    public MenuService(IUnitOfWork unitOfWork, CustomUserManager userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<ServiceResult<List<MenuResponse>>> GetMenu()
    {
        try
        {
            var accessActions = _userManager.GetCurrentUserAccessAction();

            var accessActionIds = accessActions.Select(x => x.Id).ToList();

            var menuItems = await _unitOfWork.MenuItemRepository
                .GetAllByCondition(x => accessActionIds.Contains(x.AccessActionId))
                .AsNoTracking()
                .OrderBy(x => x.Order)
                .Select(menuItem => new MenuResponse
                {
                    Id = menuItem.Id,
                    Name = menuItem.Name,
                    Icon = menuItem.Icon,
                    Url = menuItem.Url,
                    ParentId = menuItem.ParentId,
                    AccessActionId = menuItem.AccessActionId,
                })
                .ToListAsync();

            return new ServiceResult<List<MenuResponse>>(menuItems);
        }
        catch (Exception e)
        {
            return new ServiceResult<List<MenuResponse>>(e.Message);
        }
    }
}