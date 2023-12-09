using Aquantica.Contracts.Responses;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface IMenuService
{
    Task<ServiceResult<List<MenuResponse>>> GetMenu();

    // Task<ServiceResult<bool>> CreateMenu();
    //
    // Task<ServiceResult<bool>> UpdateMenu();
    //
    // Task<ServiceResult<bool>> DeleteMenu(int id);
}