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

        public string? ErrorMessage { get; init; }

        public required HttpStatusCode StatusCode { get; init; }

        #region Static Constructors
        public static ApiResult<T> BadRequest<T>(string? errorMessage = null) => ErrorStatusCode<T>(HttpStatusCode.BadRequest, errorMessage ?? ApiResultDefaultMessages.BadRequest);

        public static ApiResult<T> Created<T>(T result) => new() { Result = result, StatusCode = HttpStatusCode.Created };

        public static ApiResult<T> Forbidden<T>(string? errorMessage = null) => ErrorStatusCode<T>(HttpStatusCode.Forbidden, errorMessage ?? ApiResultDefaultMessages.Forbidden);

        public static ApiResult<T> InternalServerError<T>(string? errorMessage = null) => ErrorStatusCode<T>(HttpStatusCode.InternalServerError, errorMessage ?? ApiResultDefaultMessages.InternalServerError);

        public static ApiResult<T> NoContent<T>() => new() { Result = default!, StatusCode = HttpStatusCode.NoContent };

        public static ApiResult<T> NotFound<T>(string? errorMessage = null) => ErrorStatusCode<T>(HttpStatusCode.NotFound, errorMessage ?? ApiResultDefaultMessages.NotFound);

        public static ApiResult<T> Ok<T>(T result) => new() { Result = result, StatusCode = HttpStatusCode.OK };

        public static ApiResult<T> ErrorStatusCode<T>(HttpStatusCode code, string? errorMessage = null) => new()
        {
            ErrorMessage = errorMessage,
            Result = default!,
            StatusCode = code
        };

        public static ApiResult<T> Unauthorized<T>(string? errorMessage = null) => ErrorStatusCode<T>(HttpStatusCode.Unauthorized, errorMessage ?? ApiResultDefaultMessages.Unauthorized);
        #endregion
    }

    file static class ApiResultDefaultMessages
    {
        public const string BadRequest = "The request is invalid or malformed.";

        public const string Forbidden = "You have no permission to get access to the contents.";

        public const string InternalServerError = "An internal server error occurred.";

        public const string NotFound = "The requested resource does not exist on the server.";

        public const string Unauthorized = "You are not authenticated.";
    }
}
