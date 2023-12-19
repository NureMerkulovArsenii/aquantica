namespace Aquantica.Contracts.Requests.Rulesets;

public class CreateRuleSetRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public double WaterConsumptionThreshold { get; set; }

    public bool IsIrrigationDurationEnabled { get; set; }

    public TimeSpan IrrigationDuration { get; set; }
    //public int IrrigationDuration { get; set; }

    public bool RainAvoidanceEnabled { get; set; }

    public double RainProbabilityThreshold { get; set; }

    public TimeSpan RainAvoidanceDurationThreshold { get; set; }
    //public int RainAvoidanceDurationThreshold { get; set; }

    public double TemperatureThreshold { get; set; }

    public double MinSoilHumidityThreshold { get; set; }

    public double MaxSoilHumidityThreshold { get; set; }

    public double RainAmountThreshold { get; set; }
}