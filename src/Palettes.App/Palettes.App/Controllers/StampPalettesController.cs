using Microsoft.AspNetCore.Mvc;
using Palettes.Utils.Authentication.Claims;

namespace Palettes.App.Controllers
{
    [Route("api/stamp-palettes")]
    [ApiController]
    public class StampPalettesController(
        Api.IApiClientFactory apiClientFactory,
        ILogger<StampPalettesController> logger
        ) : ControllerBase
    {
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetStampPaletteAsync(Guid id)
        {
            try
            {
                var user = User.ToTraqUserInfo();
                if (user is null)
                {
                    return Unauthorized();
                }
                var handler = await apiClientFactory.CreateApiClientAsync(HttpContext.RequestAborted);
                var result = await handler.GetStampPaletteAsync(id, HttpContext.RequestAborted);
                return result.StatusCode switch
                {
                    System.Net.HttpStatusCode.OK => Ok(result.Result),
                    System.Net.HttpStatusCode.NotFound => NotFound(),
                    _ => StatusCode((int)result.StatusCode),
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while getting stamp palette {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
