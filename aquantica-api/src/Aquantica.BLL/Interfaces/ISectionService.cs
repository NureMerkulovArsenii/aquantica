using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Responses;

namespace Aquantica.BLL.Interfaces;

public interface ISectionService
{
    Task<List<SectionResponse>> GetAllSectionsAsync();
    
    Task<SectionResponse> GetSectionByIdAsync(int id);
    
    Task<bool> CreateSectionAsync(CreateSectionRequest request);
    
    Task<bool> UpdateSectionAsync(UpdateSectionRequest request);
    
    Task<bool> DeleteSectionAsync(int id);
    
    Task<List<IrrigationHistoryResponse>> GetIrrigationHistoryAsync(GetIrrigationHistoryRequest request);
}