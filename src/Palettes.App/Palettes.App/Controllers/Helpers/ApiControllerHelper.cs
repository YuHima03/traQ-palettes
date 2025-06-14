using Microsoft.AspNetCore.Mvc;
using Palettes.Api;

namespace Palettes.App.Controllers.Helpers
{
    static class ApiControllerHelper
    {
        public static bool IsUserAuthenticated(this ControllerBase controller)
        {
            return controller.User.Identity is { IsAuthenticated: true };
        }

        public static async ValueTask<ActionResult> GetActionResultAsync<TResponse>(this ControllerBase controller, ValueTask<ApiResult<TResponse>> apiResultTask, ILogger logger)
        {
            try
            {
                var result = await apiResultTask;
                return result.StatusCode switch
                {
                    System.Net.HttpStatusCode.OK => controller.Ok(result.Result),
                    _ => controller.StatusCode((int)result.StatusCode)
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred in a API handler.");
                return controller.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public static async ValueTask<StatusCodeResult> GetActionResultAsync(this ControllerBase controller, ValueTask<ApiResult> apiResultTask, ILogger logger)
        {
            try
            {
                var result = await apiResultTask;
                return controller.StatusCode((int)result.StatusCode);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred in a API handler.");
                return controller.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
