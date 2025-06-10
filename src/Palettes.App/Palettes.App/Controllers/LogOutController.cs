using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Palettes.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogOutController : ControllerBase
    {
        [HttpGet]
        public async Task<LocalRedirectResult> IndexAsync([FromQuery(Name = "redirect")] string? redirect = null)
        {
            if (HttpContext.User.Identity is { IsAuthenticated: true })
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            redirect = $"/{redirect?.TrimStart('/')}";
            return LocalRedirect(Url.IsLocalUrl(redirect) ? redirect : "/");
        }
    }
}
