using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Responses;

namespace Aquantica.BLL.Interfaces;

public interface IRuleSetService
{
    Task<List<RuleSetResponse>> GetAllRuleSetsAsync();
    
    Task<RuleSetResponse> GetRuleSetByIdAsync(int id);
    
    Task<bool> CreateRuleSetAsync(CreateRuleSetRequest request);
    
    Task<bool> UpdateRuleSetAsync(UpdateRuleSetRequest request);
    
    Task<bool> DeleteRuleSetAsync(int id);
    
    Task<RuleSetResponse> GetRuleSetsBySectionIdAsync(int sectionId);
    
    Task<bool> AssignRuleSetToSectionAsync(int ruleSetId, int sectionId);
}