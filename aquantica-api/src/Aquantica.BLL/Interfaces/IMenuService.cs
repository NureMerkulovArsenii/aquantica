using Aquantica.Contracts.Requests.Menu;
using Aquantica.Contracts.Responses;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface IMenuService
{
    Task<ServiceResult<List<MenuResponse>>> GetMenu();
    
    Task<ServiceResult<MenuResponse>> GetMenuById(int id);
    
    Task<ServiceResult<MenuResponse>> CreateMenu(CreateMenuRequest request);
    
    Task<ServiceResult<MenuResponse>> UpdateMenu(UpdateMenuRequest request);
    
    Task<bool> DeleteMenu(int id);
    
}
