namespace Aquantica.BLL.Interfaces;

public interface IHttpService
{
    Task<T> GetAsync<T>(string url);

    T Get<T>(string url);
}