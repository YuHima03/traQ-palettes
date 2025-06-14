using Microsoft.AspNetCore.Mvc;
using Palettes.Api.StampPaletteApi;
using Palettes.App.Controllers.Helpers;

namespace Palettes.App.Controllers
{
    [Route("api/stamp-palettes")]
    [ApiController]
    public class StampPalettesController(
        Api.IApiClientFactory apiClientFactory,
        ILogger<StampPalettesController> logger
        ) : ControllerBase
    {
        [HttpDelete]
        [Route("{id:guid}/subscription")]
        public async Task<StatusCodeResult> DeleteStampPaletteSubscriptionAsync(Guid id)
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.DeleteStampPaletteSubscriptionAsync(id, ct), logger)
                : Unauthorized();
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id)
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.GetStampPaletteAsync(id, ct), logger)
                : Unauthorized();
        }

        [HttpPost]
        [Route("{id:guid}/subscription")]
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
