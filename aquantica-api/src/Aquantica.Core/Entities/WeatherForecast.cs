namespace Aquantica.Core.Entities;

public class WeatherForecast : BaseEntity
{
    public DateTime Time { get; set; }

    public double Temperature { get; set; }

    public int RelativeHumidity { get; set; }

    public int PrecipitationProbability { get; set; }

    public double Precipitation { get; set; }

    public double SoilMoisture { get; set; }
    
    public int LocationId { get; set; }
    
    public virtual Location Location { get; set; }
    
    public int WeatherRecordId { get; set; }
    
    public virtual WeatherRecord WeatherRecord { get; set; }

}