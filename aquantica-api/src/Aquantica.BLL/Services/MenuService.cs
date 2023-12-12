using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests.Menu;
using Aquantica.Contracts.Responses;
using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;
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
    
    public async Task<ServiceResult<MenuResponse>> GetMenuById(int id)
    {
        var menuItem = await _unitOfWork.MenuItemRepository
            .GetAllByCondition(x => x.Id == id)
            .AsNoTracking()
            .Select(menuItem => new MenuResponse
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Icon = menuItem.Icon,
                Url = menuItem.Url,
                ParentId = menuItem.ParentId,
                AccessActionId = menuItem.AccessActionId,
            })
            .FirstOrDefaultAsync();

        return new ServiceResult<MenuResponse>(menuItem);
    }
    
    public async Task<ServiceResult<MenuResponse>> CreateMenu(CreateMenuRequest request)
    {
        var menuItem = new MenuItem
        {
            Name = request.Name,
            Icon = request.Icon,
            Url = request.Url,
            Order = request.Order,
            ParentId = request.ParentId,
            AccessActionId = request.AccessActionId,
        };

        await _unitOfWork.MenuItemRepository.AddAsync(menuItem);
        await _unitOfWork.SaveAsync();

        return new ServiceResult<MenuResponse>(new MenuResponse
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Icon = menuItem.Icon,
            Url = menuItem.Url,
            ParentId = menuItem.ParentId,
            AccessActionId = menuItem.AccessActionId,
        });
    }
    
    public async Task<ServiceResult<MenuResponse>> UpdateMenu(UpdateMenuRequest request)
    {
        var menuItem = await _unitOfWork.MenuItemRepository
            .GetAllByCondition(x => x.Id == request.Id)
            .FirstOrDefaultAsync();

        if (menuItem == null)
            throw new Exception();

        menuItem.Name = request.Name;
        menuItem.Icon = request.Icon;
        menuItem.Url = request.Url;
        menuItem.Order = request.Order;
        menuItem.ParentId = request.ParentId;
        menuItem.AccessActionId = request.AccessActionId;

        _unitOfWork.MenuItemRepository.Update(menuItem);
        await _unitOfWork.SaveAsync();

        return new ServiceResult<MenuResponse>(new MenuResponse
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Icon = menuItem.Icon,
            Url = menuItem.Url,
            ParentId = menuItem.ParentId,
            AccessActionId = menuItem.AccessActionId,
        });
    }
    
    public async Task<bool> DeleteMenu(int id)
    {
        var menuItem = await _unitOfWork.MenuItemRepository
            .GetAllByCondition(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (menuItem == null)
            throw new Exception();

        _unitOfWork.MenuItemRepository.Delete(menuItem);
        await _unitOfWork.SaveAsync();

        return true;
    }
}