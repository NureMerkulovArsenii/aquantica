using Aquantica.Contracts.Responses;

namespace Aquantica.Contracts.Extensions;

public static class ResponseExtensions
{
    public static BaseResponse<T> ToApiResponse<T>(this T response)
    {
        return new BaseResponse<T> { Data = response, IsSuccess = response != null };
    }

    public static BaseResponse<string> ToErrorResponse(this string error)
    {
        return new BaseResponse<string>
        {
            Error = error,
            Data = null
        };
    }
}

