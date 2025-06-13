using Microsoft.AspNetCore.Mvc;
using Palettes.Api;

namespace Palettes.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        IApiClientFactory apiClientFactory,
        ILogger<UsersController> logger
        )
        : ControllerBase
    {
        [HttpGet]
        [Route("me")]
        public async Task<IActionResult> GetMeAsync()
        {
            try
            {
                var handler = await apiClientFactory.CreateApiClientAsync(HttpContext.RequestAborted);
                var result = await handler.GetMeAsync(HttpContext.RequestAborted);
                return result.StatusCode switch
                {
                    System.Net.HttpStatusCode.OK => Ok(result.Result),
                    System.Net.HttpStatusCode.Unauthorized => Unauthorized(),
                    _ => StatusCode((int)result.StatusCode)
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching user details");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
