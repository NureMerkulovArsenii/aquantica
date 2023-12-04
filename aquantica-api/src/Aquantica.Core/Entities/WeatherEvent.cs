namespace Aquantica.Core.Entities;

public class WeatherEvent : BaseEntity
{
    public int LocationId { get; set; }
    public virtual Location Location { get; set; }
}