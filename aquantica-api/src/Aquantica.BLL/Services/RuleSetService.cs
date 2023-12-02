using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Responses;
using Aquantica.Core.Entities;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.BLL.Services;

public class RuleSetService : IRuleSetService
{
    private readonly IUnitOfWork _unitOfWork;

    public RuleSetService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<RuleSetResponse>> GetAllRuleSetsAsync()
    {
        var ruleSets = await _unitOfWork.RulesetRepository
            .GetAll()
            .Select(x => new RuleSetResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                WaterConsumptionThreshold = x.WaterConsumptionThreshold,
                IsIrrigationDurationEnabled = x.IsIrrigationDurationEnabled,
                IrrigationDuration = x.IrrigationDuration,
                RainAvoidanceEnabled = x.RainAvoidanceEnabled,
                RainProbabilityThreshold = x.RainProbabilityThreshold,
                RainAvoidanceDurationThreshold = x.RainAvoidanceDurationThreshold,
                TemperatureThreshold = x.TemperatureThreshold,
                MinSoilHumidityThreshold = x.MinSoilHumidityThreshold,
                MaxSoilHumidityThreshold = x.MaxSoilHumidityThreshold,
                MaxWindSpeed = x.MaxWindSpeed
            })
            .AsNoTracking()
            .ToListAsync();

        return ruleSets;
    }

    public async Task<RuleSetResponse> GetRuleSetByIdAsync(int id)
    {
        var ruleSet = await _unitOfWork.RulesetRepository
            .GetAllByCondition(x => x.Id == id)
            .Select(x => new RuleSetResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                WaterConsumptionThreshold = x.WaterConsumptionThreshold,
                IsIrrigationDurationEnabled = x.IsIrrigationDurationEnabled,
                IrrigationDuration = x.IrrigationDuration,
                RainAvoidanceEnabled = x.RainAvoidanceEnabled,
                RainProbabilityThreshold = x.RainProbabilityThreshold,
                RainAvoidanceDurationThreshold = x.RainAvoidanceDurationThreshold,
                TemperatureThreshold = x.TemperatureThreshold,
                MinSoilHumidityThreshold = x.MinSoilHumidityThreshold,
                MaxSoilHumidityThreshold = x.MaxSoilHumidityThreshold,
                MaxWindSpeed = x.MaxWindSpeed
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return ruleSet;
    }

    public async Task<bool> CreateRuleSetAsync(CreateRuleSetRequest request)
    {
        var ruleSet = new IrrigationRuleset
        {
            Name = request.Name,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
            WaterConsumptionThreshold = request.WaterConsumptionThreshold,
            IsIrrigationDurationEnabled = request.IsIrrigationDurationEnabled,
            IrrigationDuration = ConvertToTimeSpan(request.IrrigationDuration),
            RainAvoidanceEnabled = request.RainAvoidanceEnabled,
            RainProbabilityThreshold = request.RainProbabilityThreshold,
            RainAvoidanceDurationThreshold = ConvertToTimeSpan(request.RainAvoidanceDurationThreshold),
            TemperatureThreshold = request.TemperatureThreshold,
            MinSoilHumidityThreshold = request.MinSoilHumidityThreshold,
            MaxSoilHumidityThreshold = request.MaxSoilHumidityThreshold,
            MaxWindSpeed = request.MaxWindSpeed
        };

        await _unitOfWork.RulesetRepository.AddAsync(ruleSet);

        await _unitOfWork.SaveAsync();
        
        return true;
    }

    public async Task<bool> UpdateRuleSetAsync(UpdateRuleSetRequest request)
    {
        var ruleSet = await _unitOfWork.RulesetRepository
            .GetAllByCondition(x => x.Id == request.Id)
            .FirstOrDefaultAsync();
        
        if (ruleSet == null)
            throw new Exception("RuleSet not found");
        
        ruleSet.Name = request.Name;
        ruleSet.Description = request.Description;
        ruleSet.IsEnabled = request.IsEnabled;
        ruleSet.WaterConsumptionThreshold = request.WaterConsumptionThreshold;
        ruleSet.IsIrrigationDurationEnabled = request.IsIrrigationDurationEnabled;
        ruleSet.IrrigationDuration = ConvertToTimeSpan(request.IrrigationDuration);
        ruleSet.RainAvoidanceEnabled = request.RainAvoidanceEnabled;
        ruleSet.RainProbabilityThreshold = request.RainProbabilityThreshold;
        ruleSet.RainAvoidanceDurationThreshold = ConvertToTimeSpan(request.RainAvoidanceDurationThreshold);
        ruleSet.TemperatureThreshold = request.TemperatureThreshold;
        ruleSet.MinSoilHumidityThreshold = request.MinSoilHumidityThreshold;
        ruleSet.MaxSoilHumidityThreshold = request.MaxSoilHumidityThreshold;
        ruleSet.MaxWindSpeed = request.MaxWindSpeed;
        
        await _unitOfWork.SaveAsync();
        
        return true;
    }

    public async Task<bool> DeleteRuleSetAsync(int id)
    {
        var ruleSet = await _unitOfWork.RulesetRepository
            .GetAllByCondition(x => x.Id == id)
            .FirstOrDefaultAsync();
        
        if (ruleSet == null)
            throw new Exception("RuleSet not found");
        
        _unitOfWork.RulesetRepository.Delete(ruleSet);
        
        await _unitOfWork.SaveAsync();
        
        return true;
    }

    public async Task<RuleSetResponse> GetRuleSetsBySectionIdAsync(int sectionId)
    {
        var ruleSet = await _unitOfWork.RulesetRepository
            .GetAllByCondition(x => x.Id == sectionId)
            .AsNoTracking()
            .Select(x => new RuleSetResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                WaterConsumptionThreshold = x.WaterConsumptionThreshold,
                IsIrrigationDurationEnabled = x.IsIrrigationDurationEnabled,
                IrrigationDuration = x.IrrigationDuration,
                RainAvoidanceEnabled = x.RainAvoidanceEnabled,
                RainProbabilityThreshold = x.RainProbabilityThreshold,
                RainAvoidanceDurationThreshold = x.RainAvoidanceDurationThreshold,
                TemperatureThreshold = x.TemperatureThreshold,
                MinSoilHumidityThreshold = x.MinSoilHumidityThreshold,
                MaxSoilHumidityThreshold = x.MaxSoilHumidityThreshold,
                MaxWindSpeed = x.MaxWindSpeed
            })
            .FirstOrDefaultAsync();

        return ruleSet;
    }

    public async Task<bool> AssignRuleSetToSectionAsync(int ruleSetId, int sectionId)
    {
        var ruleSet = await _unitOfWork.RulesetRepository
            .GetAllByCondition(x => x.Id == ruleSetId)
            .FirstOrDefaultAsync();
        
        if (ruleSet == null)
            throw new Exception("RuleSet not found");
        
        var section = await _unitOfWork.SectionRepository
            .GetAllByCondition(x => x.Id == sectionId)
            .FirstOrDefaultAsync();
        
        if (section == null)
            throw new Exception("Section not found");
        
        section.IrrigationRuleset = ruleSet;
        
        await _unitOfWork.SaveAsync();
        
        return true;
        
    }
    
    private TimeSpan ConvertToTimeSpan(int minutes)
    {
        var hours = minutes / 60;
        var minutesLeft = minutes % 60;
        return new TimeSpan(hours, minutesLeft, 0);
    }
}