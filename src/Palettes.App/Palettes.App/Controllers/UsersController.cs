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
        [ProducesResponseType<GetMeResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetMeResult>> GetMeAsync()
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.GetMeAsync(ct), logger)
                : Unauthorized();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching user details");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
