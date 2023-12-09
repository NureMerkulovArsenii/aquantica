using Aquantica.Contracts.Responses;
using Aquantica.Contracts.Responses.Base;

namespace Aquantica.Contracts.Extensions;

public static class ResponseExtensions
{
    public static BaseResponse<T> ToApiResponse<T>(this T response)
    {
        return new BaseResponse<T> { Data = response, IsSuccess = response != null };
    }

    public static ApiListResponse<T> ToApiListResponse<T>(this IEnumerable<T> response)
    {
        return new ApiListResponse<T>() { IsSuccess = response.Any(), Data = response, TotalCount = response.Count() };
    }

    public static BaseResponse<string> ToApiErrorResponse(this string error)
    {
        return new BaseResponse<string>
        {
            Error = error,
            Data = null
        };
    }
}