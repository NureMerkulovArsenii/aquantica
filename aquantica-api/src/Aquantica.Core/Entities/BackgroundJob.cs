using Aquantica.Core.Enums;

namespace Aquantica.Core.Entities;

public class BackgroundJob : BaseEntity
{
    public string Name { get; set; }
    
    public bool IsEnabled { get; set; }
    
    public JobRepetitionType JobRepetitionType { get; set; }
    
    public int JobRepetitionValue { get; set; }
    
    public JobMethodEnum JobMethod { get; set; }
    
    public string CronExpression { get; set; }

    public int? IrrigationSectionId { get; set; }
    public virtual IrrigationSection? IrrigationSection { get; set; }
    
    public virtual ICollection<BackgroundJobEvent>? BackgroundJobEvents { get; set; }
    
}