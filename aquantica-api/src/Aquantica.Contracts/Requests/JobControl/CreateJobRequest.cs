using Aquantica.Core.Enums;

namespace Aquantica.Contracts.Requests.JobControl;

public class CreateJobRequest
{
    public string Name { get; set; }
    
    public bool IsEnabled { get; set; }

    public JobRepetitionType JobRepetitionType { get; set; }

    public JobMethodEnum JobMethod { get; set; }
    
    public int JobRepetitionValue { get; set; }
    
}