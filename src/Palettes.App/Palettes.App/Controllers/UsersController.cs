using Microsoft.AspNetCore.Mvc;
using Palettes.Api;
using Palettes.Api.StampPaletteApi;
using Palettes.Api.UserApi;
using Palettes.App.Controllers.Helpers;

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

        [HttpGet]
        [Route("me/stamp-palettes")]
        [ProducesResponseType<GetStampPaletteListResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetStampPaletteListResult>> GetMyStampPalettesAsync()
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.GetMyStampPalettesAsync(ct), logger)
                : Unauthorized();
        }
    }
}
