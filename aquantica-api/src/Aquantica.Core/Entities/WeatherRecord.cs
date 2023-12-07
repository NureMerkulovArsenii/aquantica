namespace Aquantica.Core.Entities;

public class WeatherRecord : BaseEntity
{
    public DateTime Time { get; set; }

    public bool IsForecast { get; set; }

    public virtual ICollection<WeatherForecast> WeatherForecasts { get; set; }
    
}