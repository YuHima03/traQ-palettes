using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Palettes.App.Controllers.Authentication
{
    [Route("auth/traq")]
    [ApiController]
    public class TraqAuthenticationController(
        IHttpClientFactory httpClientFactory,
        ILogger<TraqAuthenticationController> logger,
        IOptions<Configurations.TraqClientOptions> traqClientOptions) : ControllerBase
    {
        const string Session_TraqCodeVerifier = "auth.traq.code_verifier";
        const string Session_RedirectUri = "auth.redirect";

        [HttpGet]
        [Route("")]
        public IActionResult Index([FromQuery(Name = "redirect")] string? redirect = null)
        {
            if (!Url.IsLocalUrl(redirect))
            {
                redirect = null;
            }
            if (HttpContext.User.Identity is { IsAuthenticated: true })
            {
                return LocalRedirect(redirect ?? "/");
            }

            var traqOptions = traqClientOptions.Value;
            if (string.IsNullOrEmpty(traqOptions.ApiBaseAddress) || string.IsNullOrEmpty(traqOptions.ClientId))
            {
                logger.LogError("Required configurations are not set.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var verifier = GenerateCodeVerifier();
            var challenge = Base64Url.EncodeToString(SHA256.HashData(verifier));

            var sess = HttpContext.Session;
            if (!sess.IsAvailable)
            {
                logger.LogError("Session is not available");
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            sess.Set(Session_TraqCodeVerifier, verifier);
            sess.SetString(Session_RedirectUri, redirect ?? "/");

            var query = QueryString.Create([
                KeyValuePair.Create("client_id", traqOptions.ClientId)!,
                KeyValuePair.Create("code_challenge", challenge)!,
                KeyValuePair.Create("code_challenge_method", "S256")!,
                KeyValuePair.Create("response_type", "code")!,
                KeyValuePair.Create("scope", "read write")!,
                ]);

            UriBuilder ub = new(traqOptions.ApiBaseAddress);
            ub.Path = Path.Combine(ub.Path, "oauth2/authorize");
            ub.Query = query.ToUriComponent();

            return Redirect(ub.ToString());
        }

        [HttpGet]
        [Route("callback")]
        public async Task<IActionResult> CallbackAsync([FromQuery(Name = "code")] string? code)
        {
            var traqOptions = traqClientOptions.Value;
            if (string.IsNullOrEmpty(traqOptions.ApiBaseAddress) || string.IsNullOrEmpty(traqOptions.ClientId))
            {
                logger.LogError("Required configurations are not set.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var sess = HttpContext.Session;
            if (!sess.IsAvailable)
            {
                logger.LogError("Session is not available");
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            if (!sess.TryGetValue(Session_TraqCodeVerifier, out byte[]? verifier) || verifier is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            var redirect = sess.GetString(Session_RedirectUri);
            sess.Remove(Session_TraqCodeVerifier);
            sess.Remove(Session_RedirectUri);

            Traq.Api.Oauth2Api traqOAuth2Api = new(httpClientFactory.CreateClient("traq"));
            var tokenRes = await traqOAuth2Api.PostOAuth2TokenAsync(
                grantType: "authorization_code",
                clientId: traqOptions.ClientId,
                code: code,
                codeVerifier: Encoding.UTF8.GetString(verifier)
                );
            if (tokenRes.TokenType != "Bearer")
            {
                logger.LogError("Invalid token type: {TokenType}", tokenRes.TokenType);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Traq.ITraqApiClient traqClient = new Traq.TraqApiClient(Options.Create(new Traq.TraqApiClientOptions()
            {
                BaseAddress = traqOptions.ApiBaseAddress,
                BearerAuthToken = tokenRes.AccessToken,
            }));

            Traq.Model.MyUserDetail me;
            try
            {
                me = await traqClient.MeApi.GetMeAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get user info.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            if (tokenRes.AccessToken is null)
            {
                logger.LogError("Access token is null");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            ClaimsIdentity identity = new(
                claims: [
                    new Claim(ClaimTypes.Name, me.Name),
                    new Claim(ClaimTypes.NameIdentifier, me.Id.ToString()),
                    new Claim(ClaimTypesInternal.TraqAccessToken, tokenRes.AccessToken),
                    new Claim(ClaimTypes.Expiration, DateTimeOffset.UtcNow.AddSeconds(tokenRes.ExpiresIn).ToString())
                    ],
                authenticationType: CookieAuthenticationDefaults.AuthenticationScheme
                );

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(Math.Min(tokenRes.ExpiresIn, TimeSpan.SecondsPerDay * 7)), // 有効期限は最長で7日
                }
            );

            if (!Url.IsLocalUrl(redirect))
            {
                redirect = null;
            }
            return LocalRedirect(redirect ?? "/");
        }

        readonly static byte[] CodeVerifierChars = [.. "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~"u8];

        static byte[] GenerateCodeVerifier()
        {
            return Random.Shared.GetItems(CodeVerifierChars, 64);
        }
    }
}
