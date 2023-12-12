using Aquantica.Contracts.Requests.AccessActions;
using Aquantica.Contracts.Responses.AccessActions;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface IAccessActionService
{
    Task<List<AccessActionResponse>> GetAccessActionsAsync();

    Task<AccessActionDetailedResponse> GetAccessActionByIdAsync(int id);

    Task<bool> CreateAccessActionAsync(CreateAccessActionRequest request);

    Task<bool> UpdateAccessActionAsync(UpdateAccessActionRequest request);

    Task<bool> DeleteAccessActionAsync(int id);
}