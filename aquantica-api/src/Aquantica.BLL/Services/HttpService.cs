using Aquantica.BLL.Interfaces;
using Newtonsoft.Json;

namespace Aquantica.BLL.Services;

public class HttpService : IHttpService
{
    //ToDO: add polly retry and timeout
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
    
    //ToDO: add polly retry and timeout
    public T Get<T>(string url)
    {
        using var httpClient = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        using var response = httpClient.Send(request);

        response.EnsureSuccessStatusCode();
        var body = response.Content.ReadAsStream();
        var contentString = new StreamReader(body).ReadToEnd();
        var result = JsonConvert.DeserializeObject<T>(contentString);

        return result;
    }
    
    
}