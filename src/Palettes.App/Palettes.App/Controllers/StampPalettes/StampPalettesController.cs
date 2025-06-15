using Microsoft.AspNetCore.Mvc;
using Palettes.Api;
using Palettes.Api.StampPaletteApi;
using Palettes.App.Controllers.Helpers;

namespace Palettes.App.Controllers.StampPalettes
{
    [Route("api/stamp-palettes")]
    [ApiController]
    public class StampPalettesController(
        IApiClientFactory apiClientFactory,
        ILogger<StampPaletteSingleController> logger
        ) : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [ProducesResponseType<GetStampPaletteListResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetStampPaletteListResult>> GetPublicStampPalettesAsync()
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.GetPublicStampPalettesAsync(ct), logger)
                : Unauthorized();
        }
    }
}
