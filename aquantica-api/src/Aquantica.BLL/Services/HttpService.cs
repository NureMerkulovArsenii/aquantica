using Aquantica.BLL.Interfaces;
using Newtonsoft.Json;

namespace Aquantica.BLL.Services;

public class HttpService : IHttpService
{
    public async Task<T> GetAsync<T>(string url)
    {
        using var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        using var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<T>(body);

        return result;
    }
}