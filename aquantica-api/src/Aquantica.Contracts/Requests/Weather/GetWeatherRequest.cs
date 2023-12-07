namespace Aquantica.Contracts.Requests.Weather;

public class GetWeatherRequest
{
    public int? LocationId { get; set; }
    
    public bool IsForecast { get; set; }

    public bool IsFromRecentRecord { get; set; }

    public DateTime? TimeFrom { get; set; }
    
    public DateTime? TimeTo { get; set; }
}