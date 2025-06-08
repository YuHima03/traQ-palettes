using Microsoft.AspNetCore.Mvc;

namespace Palettes.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        public LocalRedirectResult Index([FromQuery(Name = "redirect")] string? redirect = null)
        {
            if (!Url.IsLocalUrl(redirect))
            {
                redirect = null;
            }
            return LocalRedirect($"/auth/traq?redirect={Uri.EscapeDataString(redirect ?? "/")}");
        }
    }
}
