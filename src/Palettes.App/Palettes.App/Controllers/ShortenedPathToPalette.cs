using Microsoft.AspNetCore.Mvc;

namespace Palettes.App.Controllers
{
    [Route("p")]
    [ApiController]
    public class ShortenedPathToPalette : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public LocalRedirectResult RedirectToPalette(string id)
        {
            Span<byte> bytes = stackalloc byte[16];
            if (SimpleBase.Base58.Bitcoin.TryDecode(id, bytes, out var len) && len == bytes.Length)
            {
                var guid = new Guid(bytes);
                return LocalRedirect($"/stamp-palettes/{guid}");
            }
            return LocalRedirect("/");
        }
    }
}
