﻿using Microsoft.AspNetCore.Mvc;
using Palettes.Api.StampPaletteApi;
using Palettes.App.Controllers.Helpers;

namespace Palettes.App.Controllers.StampPalettes
{
    [Route("api/stamp-palettes/{id:guid}")]
    [ApiController]
    public class StampPaletteSingleController(
        Api.IApiClientFactory apiClientFactory,
        ILogger<StampPaletteSingleController> logger
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

        [HttpGet]
        [Route("subscription")]
        [ProducesResponseType<GetStampPaletteSubscriptionResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetStampPaletteSubscriptionResult>> GetStampPaletteSubscriptionAsync(Guid id)
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.GetStampPaletteSubscriptionAsync(id, ct), logger)
                : Unauthorized();
        }

        [HttpPatch]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<StatusCodeResult> PatchStampPaletteAsync(Guid id, [FromBody] PatchStampPaletteRequest request)
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.PatchStampPaletteAsync(id, request, ct), logger)
                : Unauthorized();
        }

        [HttpPost]
        [Route("subscription")]
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

        [HttpPost]
        [Route("sync")]
        [ProducesResponseType<GetStampPaletteSubscriptionResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetStampPaletteSubscriptionResult>> SyncStampPaletteAsync(Guid id)
        {
            var ct = HttpContext.RequestAborted;
            await using var handler = await apiClientFactory.CreateApiClientAsync(ct);
            return this.IsUserAuthenticated()
                ? await this.GetActionResultAsync(handler.SyncCloneStampPaletteAsync(id, ct), logger)
                : Unauthorized();
        }
    }
}
