using System.Net;

namespace Palettes.Api
{
    public sealed class ApiResult<TResult>
    {
        public bool IsSuccessStatusCode
        {
            get
            {
                int code = (int)StatusCode;
                return 200 <= code && code < 300;
            }
        }

        public required TResult? Result { get; init; }

        public required HttpStatusCode StatusCode { get; init; }
    }

    public static class ApiResult
    {
        public static ApiResult<T> NotFound<T>()
        {
            return new() { Result = default!, StatusCode = HttpStatusCode.NotFound };
        }

        public static ApiResult<T> Ok<T>(T result)
        {
            return new() { Result = result, StatusCode = HttpStatusCode.OK };
        }
    }
}
