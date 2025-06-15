using Palettes.Api.StampPaletteApi;

namespace Palettes.Api.UserApi
{
    public interface IUserApi
    {
        /// <summary>
        /// Gets the authenticated user's information.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>200 OK</term> OK.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult<GetMeResult>> GetMeAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the stamp palettes of the authenticated user.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>200 OK</term> OK.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult<GetStampPaletteListResult>> GetMyStampPalettesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the subscriptions to stamp palettes of the authenticated user.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>200 OK</term> OK.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult<GetMyStampPaletteSubscriptionsResult>> GetMyStampPaletteSubscriptionsAsync(CancellationToken ct = default);
    }
}
