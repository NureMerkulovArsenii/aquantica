namespace Aquantica.Contracts.Responses;

public class RuleSetResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsEnabled { get; set; }
    public double WaterConsumptionThreshold { get; set; }
    
    public bool IsIrrigationDurationEnabled { get; set; }
    
    public TimeSpan IrrigationDuration { get; set; }
    
    public bool RainAvoidanceEnabled { get; set; }
    
    public double RainProbabilityThreshold { get; set; }
    
    public TimeSpan RainAvoidanceDurationThreshold { get; set; }
    
    public double TemperatureThreshold { get; set; }
    
    public double MinSoilHumidityThreshold { get; set; }
    
    public double MaxSoilHumidityThreshold { get; set; }
    
    public int MaxWindSpeed { get; set; }
}