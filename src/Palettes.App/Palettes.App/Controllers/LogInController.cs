using Microsoft.AspNetCore.Mvc;

namespace Palettes.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        public LocalRedirectResult Index()
        {
            return LocalRedirect("/auth/traq");
        }
    }
}
