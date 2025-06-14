using Microsoft.AspNetCore.Mvc;
using Palettes.Api.StampPaletteApi;
using Palettes.App.Controllers.Helpers;

namespace Palettes.App.Controllers
{
    [Route("api/stamp-palettes/{id:guid}")]
    [ApiController]
    public class StampPalettesController(
        Api.IApiClientFactory apiClientFactory,
        ILogger<StampPalettesController> logger
        ) : ControllerBase
    {
        [HttpDelete]
        [Route("subscription")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<StatusCodeResult> DeleteStampPaletteSubscriptionAsync(Guid id)
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.DeleteStampPaletteSubscriptionAsync(id, ct), logger)
                : Unauthorized();
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType<GetStampPaletteResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id)
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.GetStampPaletteAsync(id, ct), logger)
                : Unauthorized();
        }

        [HttpPost]
        [Route("ubscription")]
        [ProducesResponseType<PostStampPaletteSubscriptionResult>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PostStampPaletteSubscriptionResult>> PostStampPaletteSubscriptionAsync(Guid id)
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.PostStampPalletSubscriptionAsync(id, ct), logger)
                : Unauthorized();
        }
    }
}
