namespace Aquantica.Core.ServiceResult;

public class ServiceResult<T>
{
    public ServiceResult()
    {
        
    }
    
    public ServiceResult(T data)
    {
        Data = data;
    }

    public ServiceResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public T Data { get; set; }

    public string? ErrorMessage { get; set; }

    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    
}