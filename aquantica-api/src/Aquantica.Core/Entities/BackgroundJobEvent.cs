namespace Aquantica.Core.Entities;

public class BackgroundJobEvent : BaseEntity
{
    public string? EventMessage { get; set; }
    public bool IsStart { get; set; }
    public int BackgroundJobId { get; set; }
    public virtual BackgroundJob BackgroundJob { get; set; }
}