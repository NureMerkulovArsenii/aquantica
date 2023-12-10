using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Responses;
using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;
using Aquantica.Core.ServiceResult;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.BLL.Services;

public class SectionService : ISectionService
{
    private readonly IUnitOfWork _unitOfWork;

    public SectionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SectionResponse>> GetAllSectionsAsync()
    {
        var sections = await _unitOfWork.SectionRepository
            .GetAll()
            .Select(x => new SectionResponse
            {
                Id = x.Id,
                Name = x.Name,
                Number = x.Number,
                ParentId = x.ParentId,
                IsEnabled = x.IsEnabled,
                SectionRulesetId = x.SectionRulesetId,
                ParentNumber = x.ParentSection == null ? null : x.ParentSection.Number
            })
            .ToListAsync();

        return sections;
    }

    public GetIrrigationSectionDTO GetSectionById(int sectionId)
    {
        var sections = _unitOfWork.SectionRepository
            .GetAll()
            .Select(x => new GetIrrigationSectionDTO
            {
                Id = x.Id,
                Name = x.Name,
                Number = x.Number,
                ParentId = x.ParentId,
                IsEnabled = x.IsEnabled,
                DeviceUri = x.DeviceUri,
                SectionRulesetId = x.SectionRulesetId,
                ParentNumber = x.ParentSection == null ? null : x.ParentSection.Number,
                LocationId = x.LocationId,
            })
            .FirstOrDefault(x => x.Id == sectionId);

        return sections;
    }

    public async Task<SectionResponse> GetSectionByIdAsync(int id)
    {
        var section = await _unitOfWork.SectionRepository
            .GetAllByCondition(x => x.Id == id)
            .Select(x => new SectionResponse
            {
                Id = x.Id,
                Name = x.Name,
                Number = x.Number,
                ParentId = x.ParentId,
                IsEnabled = x.IsEnabled,
                DeviceUri = x.DeviceUri,
                SectionRulesetId = x.SectionRulesetId,
                ParentNumber = x.ParentSection == null ? null : x.ParentSection.Number
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return section;
    }

    public ServiceResult<IrrigationSection> GetRootSection()
    {
        try
        {
            var res = new ServiceResult<IrrigationSection>();

            var section = _unitOfWork.SectionRepository
                .FirstOrDefault(x => x.ParentId == null);

            if (section == null)
                res.ErrorMessage = "Root section not found";

            if (!section.IsEnabled)
                res.ErrorMessage = "Root section is disabled";

            res.Data = section;

            return res;
        }
        catch (Exception e)
        {
            return new ServiceResult<IrrigationSection>(e.Message);
        }
    }


    public async Task<bool> CreateSectionAsync(CreateSectionRequest request)
    {
        var sectionCheck = await _unitOfWork.SectionRepository
            .GetAllByCondition(x => x.Number == request.Number)
            .FirstOrDefaultAsync();

        if (sectionCheck != null)
            throw new Exception("Section with this number already exists");

        var sectionType = await _unitOfWork.SectionTypeRepository
            .GetAllByCondition(x => x.Id == request.SectionTypeId)
            .FirstOrDefaultAsync();

        if (sectionType == null)
            throw new Exception("Section type not found");

        IrrigationSection parentSection = null;

        if (request.ParentId != null)
        {
            parentSection = await _unitOfWork.SectionRepository
                .GetAllByCondition(x => x.Id == request.ParentId)
                .FirstOrDefaultAsync();

            if (parentSection == null)
                throw new Exception("Parent section not found");
        }

        IrrigationRuleset ruleSet = null;

        if (request.SectionRulesetId != null)
        {
            ruleSet = await _unitOfWork.RulesetRepository
                .GetAllByCondition(x => x.Id == request.SectionRulesetId)
                .FirstOrDefaultAsync();

            if (ruleSet == null)
                throw new Exception("RuleSet not found");
        }

        var section = new IrrigationSection
        {
            Name = request.Name,
            Number = request.Number,
            ParentSection = parentSection,
            IsEnabled = request.IsEnabled,
            DeviceUri = request.DeviceUri,
            IrrigationRuleset = ruleSet,
            IrrigationSectionType = sectionType,
        };

        await _unitOfWork.SectionRepository.AddAsync(section);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<bool> UpdateSectionAsync(UpdateSectionRequest request)
    {
        var section = await _unitOfWork.SectionRepository
            .GetAllByCondition(x => x.Id == request.Id)
            .FirstOrDefaultAsync();

        if (section == null)
            throw new Exception("Section not found");

        var sectionCheck = await _unitOfWork.SectionRepository
            .GetAllByCondition(x => x.Number == request.Number && x.Id != request.Id)
            .FirstOrDefaultAsync();

        if (sectionCheck != null)
            throw new Exception("Section with this number already exists");

        var sectionType = await _unitOfWork.SectionTypeRepository
            .GetAllByCondition(x => x.Id == request.SectionTypeId)
            .FirstOrDefaultAsync();

        if (sectionType == null)
            throw new Exception("Section type not found");

        IrrigationSection parentSection = null;

        if (request.ParentId != null)
        {
            parentSection = await _unitOfWork.SectionRepository
                .GetAllByCondition(x => x.Id == request.ParentId)
                .FirstOrDefaultAsync();

            if (parentSection == null)
                throw new Exception("Parent section not found");
        }

        IrrigationRuleset ruleSet = null;

        if (request.SectionRulesetId != null)
        {
            ruleSet = await _unitOfWork.RulesetRepository
                .GetAllByCondition(x => x.Id == request.SectionRulesetId)
                .FirstOrDefaultAsync();

            if (ruleSet == null)
                throw new Exception("RuleSet not found");
        }

        section.Name = request.Name;
        section.Number = request.Number;
        section.ParentSection = parentSection;
        section.IsEnabled = request.IsEnabled;
        section.DeviceUri = request.DeviceUri;
        section.IrrigationRuleset = ruleSet;
        section.IrrigationSectionType = sectionType;

        await _unitOfWork.SaveAsync();

        return true;
    }


    public async Task<bool> DeleteSectionAsync(int id)
    {
        var childSections = await _unitOfWork.SectionRepository
            .GetAllByCondition(x => x.ParentId == id)
            .AsNoTracking()
            .ToListAsync();

        if (childSections.Count > 0)
            throw new Exception("Section has child sections");

        await _unitOfWork.SectionRepository.DeleteAsync(x => x.Id == id);

        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<List<IrrigationHistoryResponse>> GetIrrigationHistoryAsync(GetIrrigationHistoryRequest request)
    {
        if (request.StartDate > request.EndDate)
            throw new Exception("Start date cannot be greater than end date");

        var irrigationHistory = _unitOfWork.IrrigationEventRepository
            .GetAllByCondition(x => x.StartTime >= request.StartDate && x.EndTime <= request.EndDate);

        if (request.SectionId != 0)
            irrigationHistory = irrigationHistory.Where(x => x.SectionId == request.SectionId);

        var res = await irrigationHistory
            .OrderBy(x => x.StartTime)
            .Select(x => new IrrigationHistoryResponse
            {
                SectionId = x.SectionId,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Duration = x.EndTime - x.StartTime,
                WaterUsed = x.WaterUsed
            })
            .AsNoTracking()
            .ToListAsync();

        return res;
    }

    public bool CreateIrrigationEvent(IrrigationEventDTO request)
    {
        try
        {
            var section = _unitOfWork.SectionRepository
                .GetAllByCondition(x => x.Id == request.SectionId)
                .FirstOrDefault();

            if (section == null)
                throw new Exception("Section not found");

            var ruleset = _unitOfWork.RulesetRepository
                .GetAllByCondition(x => x.Id == request.IrrigationRulesetId)
                .FirstOrDefault();

            if (ruleset == null)
                throw new Exception("Ruleset not found");

            var irrigationEvent = new IrrigationEvent
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                WaterUsed = request.WaterUsed,
                Section = section,
                IrrigationRuleset = ruleset,
                IsStopped = request.IsStopped
            };

            _unitOfWork.IrrigationEventRepository.Add(irrigationEvent);
            _unitOfWork.Save();

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    //Updated irrigation event
    public bool UpdateIrrigationEvent(IrrigationEventDTO request)
    {
        try
        {
            var irrigationEvent = _unitOfWork.IrrigationEventRepository
                .GetAllByCondition(x => x.Id == request.Id)
                .FirstOrDefault();

            if (irrigationEvent == null)
                throw new Exception("Irrigation event not found");

            var section = _unitOfWork.SectionRepository
                .GetAllByCondition(x => x.Id == request.SectionId)
                .FirstOrDefault();

            if (section == null)
                throw new Exception("Section not found");

            var ruleset = _unitOfWork.RulesetRepository
                .GetAllByCondition(x => x.Id == request.IrrigationRulesetId)
                .FirstOrDefault();

            if (ruleset == null)
                throw new Exception("Ruleset not found");

            irrigationEvent.StartTime = request.StartTime;
            irrigationEvent.EndTime = request.EndTime;
            irrigationEvent.WaterUsed = request.WaterUsed;
            irrigationEvent.Section = section;
            irrigationEvent.IrrigationRuleset = ruleset;
            irrigationEvent.IsStopped = request.IsStopped;

            _unitOfWork.IrrigationEventRepository.Update(irrigationEvent);
            _unitOfWork.Save();

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public List<IrrigationEventDTO> GetOngoingIrrigationEventBySectionId(int sectionId)
    {
        var irrigationEvent = _unitOfWork.IrrigationEventRepository
            .GetAllByCondition(x => x.SectionId == sectionId && !x.IsStopped)
            .OrderByDescending(x => x.StartTime)
            .AsNoTracking()
            .Select(x => new IrrigationEventDTO
            {
                Id = x.Id,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                WaterUsed = x.WaterUsed,
                IsStopped = x.IsStopped,
                SectionId = x.SectionId,
                IrrigationRulesetId = x.IrrigationRulesetId
            })
            .ToList();

        return irrigationEvent;
    }

    public IrrigationEventDTO GetIrrigationEventById(int id)
    {
        var irrigationEvent = _unitOfWork.IrrigationEventRepository
            .GetAllByCondition(x => x.Id == id)
            .AsNoTracking()
            .Select(x => new IrrigationEventDTO
            {
                Id = x.Id,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                IsStopped = x.IsStopped,
                WaterUsed = x.WaterUsed,
                SectionId = x.SectionId,
                IrrigationRulesetId = x.IrrigationRulesetId
            })
            .FirstOrDefault();

        return irrigationEvent;
    }
}