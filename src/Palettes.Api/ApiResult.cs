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
        public static ApiResult<T> InternalServerError<T>() => new() { Result = default!, StatusCode = HttpStatusCode.InternalServerError };

        public static ApiResult<T> NotFound<T>() => new() { Result = default!, StatusCode = HttpStatusCode.NotFound };

        public static ApiResult<T> Ok<T>(T result) => new() { Result = result, StatusCode = HttpStatusCode.OK };

        public static ApiResult<T> Unauthorized<T>() => new() { Result = default!, StatusCode = HttpStatusCode.Unauthorized };

    }
}
