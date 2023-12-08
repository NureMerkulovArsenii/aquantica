using Aquantica.Core.Enums;

namespace Aquantica.Contracts.Responses.JobControl;

public class JobResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public bool IsEnabled { get; set; }
    
    public JobRepetitionType JobRepetitionType { get; set; }
    
    public int JobRepetitionValue { get; set; }
    
    public JobMethodEnum JobMethod { get; set; }
    
}