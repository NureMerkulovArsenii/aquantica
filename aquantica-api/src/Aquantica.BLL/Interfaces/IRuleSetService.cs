using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Requests.Rulesets;
using Aquantica.Contracts.Responses;
using Aquantica.Core.DTOs.Ruleset;

namespace Aquantica.BLL.Interfaces;

public interface IRuleSetService
{
    Task<List<RuleSetDetailedDTO>> GetAllRuleSetsAsync();
    
    Task<RuleSetDetailedDTO> GetRuleSetByIdAsync(int id);
    
    RuleSetDetailedDTO GetRuleSetBySectionId(int id);
    
    Task<bool> CreateRuleSetAsync(CreateRuleSetRequest request);
    
    Task<bool> UpdateRuleSetAsync(UpdateRuleSetRequest request);
    
    Task<bool> DeleteRuleSetAsync(int id);
    
    Task<RuleSetDetailedDTO> GetRuleSetsBySectionIdAsync(int sectionId);
    
    Task<bool> AssignRuleSetToSectionAsync(int ruleSetId, int sectionId);
}