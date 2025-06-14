using System.Net;

namespace Palettes.Api
{
    public sealed class ApiResult<TResult> : ApiResult
    {
        public required TResult? Result { get; init; }
    }

    public class ApiResult
    {
        public bool IsSuccessStatusCode
        {
            get
            {
                int code = (int)StatusCode;
                return 200 <= code && code < 300;
            }
        }

        public required HttpStatusCode StatusCode { get; init; }

        public static ApiResult<T> InternalServerError<T>() => new() { Result = default!, StatusCode = HttpStatusCode.InternalServerError };

        public static ApiResult<T> NoContent<T>() => new() { Result = default!, StatusCode = HttpStatusCode.NoContent };

        public static ApiResult<T> NotFound<T>() => new() { Result = default!, StatusCode = HttpStatusCode.NotFound };

        public static ApiResult<T> Ok<T>(T result) => new() { Result = result, StatusCode = HttpStatusCode.OK };

        public static ApiResult<T> FromStatusCode<T>(HttpStatusCode code) => new() { Result = default!, StatusCode = code };

        public static ApiResult<T> Unauthorized<T>() => new() { Result = default!, StatusCode = HttpStatusCode.Unauthorized };
    }
}
