namespace Aquantica.Contracts.Responses.Base;

public class ApiListResponse<T> : BaseResponse<IEnumerable<T>>
{
    public int? TotalCount { get; set; }

    
    public static ApiListResponse<T> CreateListResponse(IEnumerable<T> data, int? totalCount = null,
        string errors = null)
    {
        return new ApiListResponse<T>
            { Data = data, Error = errors, TotalCount = totalCount };
    }
    
}