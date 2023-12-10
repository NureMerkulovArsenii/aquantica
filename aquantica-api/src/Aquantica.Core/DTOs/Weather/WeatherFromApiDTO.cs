using Newtonsoft.Json;

namespace Aquantica.Core.DTOs.Weather;

public class WeatherFromApiDTO
{
    [JsonProperty("generationtime_ms")]
    public double GenerationTimeMs { get; set; }

    [JsonProperty("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonProperty("hourly_units")]
    public HourlyUnits HourlyUnits { get; set; }

    [JsonProperty("hourly")]
    public HourlyData Hourly { get; set; }
}

public class HourlyUnits
{
    [JsonProperty("time")]
    public string Time { get; set; }

    [JsonProperty("temperature_2m")]
    public string Temperature2m { get; set; }

    [JsonProperty("relative_humidity_2m")]
    public string RelativeHumidity2m { get; set; }

    [JsonProperty("precipitation_probability")]
    public string PrecipitationProbability { get; set; }

    [JsonProperty("precipitation")]
    public string Precipitation { get; set; }

    [JsonProperty("soil_moisture_3_to_9cm")]
    public string SoilMoisture3To9cm { get; set; }
}

public class HourlyData
{
    [JsonProperty("time")]
    public List<string> Time { get; set; }

    [JsonProperty("temperature_2m")]
    public List<double> Temperature2m { get; set; }

    [JsonProperty("relative_humidity_2m")]
    public List<int> RelativeHumidity2m { get; set; }

    [JsonProperty("precipitation_probability")]
    public List<int> PrecipitationProbability { get; set; }

    [JsonProperty("precipitation")]
    public List<double> Precipitation { get; set; }

    [JsonProperty("soil_moisture_3_to_9cm")]
    public List<double> SoilMoisture3To9cm { get; set; }
}