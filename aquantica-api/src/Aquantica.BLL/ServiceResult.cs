namespace Aquantica.BLL;

public class ServiceResult<T>
{
    public ServiceResult(T data)
    {
        Data = data;
    }

    public ServiceResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public T Data { get; }

    public string ErrorMessage { get; }

    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    
}