namespace Palettes.Api.StampPaletteApi
{
    public interface IStampPaletteApi
    {
        /// <summary>
        /// Deletes the subscription to a stamp palette.
        /// </summary>
        /// <param name="stampPaletteId"></param>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>204 No Content</term> The subscription is deleted.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        ///     <item><term>404 Not Found</term> The stamp palette or subscription is not found.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult> DeleteStampPaletteSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default);

        /// <summary>
        /// Gets the public stamp palettes of all users.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>200 OK</term> OK.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult<GetStampPaletteListResult>> GetPublicStampPalettesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the subscription to a stamp palette of the user by its ID.
        /// </summary>
        /// <param name="stampPaletteId"></param>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>200 OK</term> OK.</item>
        ///     <item><term>400 Bad Request</term> The user is an owner of the stamp palette.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        ///     <item><term>404 Not Found</term> The stamp palette is not found or is private.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult<GetStampPaletteSubscriptionResult>> GetStampPaletteSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default);

        /// <summary>
        /// Gets the stamp palette by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>200 OK</term> OK.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        ///     <item><term>403 Forbidden</term> The stamp palette is private and the user is not the owner.</item>
        ///     <item><term>404 Not Found</term> The stamp palette is not found.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult<GetStampPaletteResult>> GetStampPaletteAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Patches the stamp palette with the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>204 No Content</term> The stamp palette is updated.</item>
        ///     <item><term>400 Bad Request</term> The request is invalid or malformed.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        ///     <item><term>403 Forbidden</term> The user is not the owner of the stamp palette.</item>
        ///     <item><term>404 Not Found</term> The stamp palette is not found.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult> PatchStampPaletteAsync(Guid id, PatchStampPaletteRequest request, CancellationToken ct = default);

        /// <summary>
        /// Posts a subscription to a stamp palette.
        /// </summary>
        /// <param name="stampPaletteId"></param>
        /// <param name="ct"></param>
        /// <returns>
        /// <list type="bullet">
        ///     <item><term>201 Created</term> The subscription is created.</item>
        ///     <item><term>400 Bad Request</term> The user is an owner of the stamp palette.</item>
        ///     <item><term>401 Unauthorized</term> The user is not authenticated.</item>
        ///     <item><term>404 Not Found</term> The stamp palette is not found or private.</item>
        ///     <item><term>409 Conflict</term> A subscription to the stamp palette already exists.</item>
        /// </list>
        /// </returns>
        public ValueTask<ApiResult<PostStampPaletteSubscriptionResult>> PostStampPalletSubscriptionAsync(Guid stampPaletteId, CancellationToken ct = default);
    }
}
