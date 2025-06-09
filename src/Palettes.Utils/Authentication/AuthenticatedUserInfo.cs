namespace Palettes.Utils.Authentication
{
    public sealed class AuthenticatedUserInfo
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }

        public required string AccessToken { get; init; }
    }
}
