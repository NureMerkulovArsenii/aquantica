using System.ComponentModel.DataAnnotations.Schema;

namespace Aquantica.Core.Entities;

[Table("SectionRulesets")]
public class IrrigationRuleset : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public double WaterConsumptionThreshold { get; set; }
    
    public bool IsIrrigationDurationEnabled { get; set; }
    
    public TimeSpan IrrigationDuration { get; set; }
    
    public bool RainAvoidanceEnabled { get; set; }
    
    public double RainProbabilityThreshold { get; set; }
    
    public double RainAmountThreshold { get; set; }

    public double HumidityGrowthPerRainMm { get; set; }

    public double WaterConsumptionPerMinute { get; set; }
    
    public double HumidityGrowthPerLiterConsumed { get; set; }
    
    public TimeSpan RainAvoidanceDurationThreshold { get; set; }
    
    public double TemperatureThreshold { get; set; }
    
    public double MinSoilHumidityThreshold { get; set; }
    
    public double OptimalSoilHumidity { get; set; }

    public virtual ICollection<IrrigationSection> IrrigationSections { get; set; }
}