namespace Aquantica.Contracts.Requests.Rulesets;

public class UpdateRuleSetRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public double WaterConsumptionThreshold { get; set; }
    
    public bool IsIrrigationDurationEnabled { get; set; }
    
    public int IrrigationDuration { get; set; }
    
    public bool RainAvoidanceEnabled { get; set; }
    
    public double RainProbabilityThreshold { get; set; }
    
    public int RainAvoidanceDurationThreshold { get; set; }
    
    public double TemperatureThreshold { get; set; }
    
    public double MinSoilHumidityThreshold { get; set; }
    
    public double OptimalSoilHumidityThreshold { get; set; }
    
    public double RainAmountThreshold { get; set; }
}