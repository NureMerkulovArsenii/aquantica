using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Responses;
using Aquantica.Core.DTOs;
using Aquantica.Core.DTOs.Irrigation;
using Aquantica.Core.DTOs.Section;
using Aquantica.Core.Entities;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface ISectionService
{
    Task<List<IrrigationSectionDTO>> GetAllSectionsAsync();

    IrrigationSectionDTO GetSectionById(int sectionId);

    Task<SectionWithLocationDTO> GetSectionByIdAsync(int id);

    ServiceResult<IrrigationSection> GetRootSection();

    Task<bool> CreateSectionAsync(CreateSectionRequest request);

    Task<bool> UpdateSectionAsync(UpdateSectionRequest request);

    Task<bool> DeleteSectionAsync(int id);

    Task<List<IrrigationHistoryResponse>> GetIrrigationHistoryAsync(GetIrrigationHistoryRequest request);

    bool CreateIrrigationEvent(IrrigationEventDTO request);

    bool UpdateIrrigationEvent(IrrigationEventDTO request);

    List<IrrigationEventDTO> GetOngoingIrrigationEventBySectionId(int sectionId);

    IrrigationEventDTO GetIrrigationEventById(int id);
}