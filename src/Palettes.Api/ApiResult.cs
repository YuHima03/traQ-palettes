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
}
