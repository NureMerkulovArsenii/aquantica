namespace Aquantica.Core.DTOs.Weather;

public class WeatherDTO
{
    public int Id { get; set; }
    
    public DateTime Time { get; set; }

    public double Temperature { get; set; }

    public int RelativeHumidity { get; set; }

    public int PrecipitationProbability { get; set; }

    public double Precipitation { get; set; }

    public double SoilMoisture { get; set; }

    public int LocationId { get; set; }

    public int WeatherRecordId { get; set; }
}