using Aquantica.Core.Entities;
using Aquantica.Core.Enums;

namespace Aquantica.Core.DTOs;

public class BackgroundJobDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public bool IsEnabled { get; set; }
    
    public JobRepetitionType JobRepetitionType { get; set; }
    
    public int JobRepetitionValue { get; set; }
    
    public JobMethodEnum JobMethod { get; set; }
    
    public string CronExpression { get; set; }

    public int? IrrigationSectionId { get; set; }
}