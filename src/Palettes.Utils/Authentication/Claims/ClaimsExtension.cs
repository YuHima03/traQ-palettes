using System.Security.Claims;

namespace Palettes.Utils.Authentication.Claims
{
    public static class ClaimsExtension
    {
        public static IEnumerable<Claim> ToTraqUserClaims(this AuthenticatedUserInfo user)
        {
            yield return new(ClaimTypes.NameIdentifier, user.Id.ToString());
            yield return new(ClaimTypes.Name, user.Name);
            yield return new(ClaimTypesInternal.TraqAccessToken, user.AccessToken);
        }

        public static AuthenticatedUserInfo ToTraqUserInfo(this ClaimsPrincipal claimsPrincipal)
        {
            return new AuthenticatedUserInfo
            {
                Id = Guid.Parse(claimsPrincipal.FindAll(ClaimTypes.NameIdentifier).Single().Value),
                Name = claimsPrincipal.FindAll(ClaimTypes.Name).Single().Value,
                AccessToken = claimsPrincipal.FindAll(ClaimTypesInternal.TraqAccessToken).Single().Value
            };
        }
    }
}
