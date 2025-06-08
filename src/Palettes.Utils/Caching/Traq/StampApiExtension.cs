using Microsoft.Extensions.Caching.Memory;
using Traq.Api;
using Traq.Model;

namespace Palettes.Utils.Caching.Traq
{
    public static class StampApiExtension
    {
        static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

        public static async ValueTask<Stamp> GetCachedStampAsync(this IStampApiAsync api, IMemoryCache cache, Guid id, CancellationToken ct = default)
        {
            if (cache.TryGetValue<Guid, Stamp>(CacheSections.Stamp, id, out var stamp) && stamp is not null)
            {
                return stamp;
            }
            stamp = await api.GetStampAsync(id, ct);
            cache.Set(CacheSections.Stamp, id, stamp, DefaultExpiration);
            return stamp;
        }

        public static async ValueTask<Stamp> GetCachedStampAsync(this IStampApiAsync api, IMemoryCache cache, string name, CancellationToken ct = default)
        {
            if (cache.TryGetValue<string, Guid>(CacheSections.NamedStamp, name, out var id) && id != Guid.Empty)
            {
                return await api.GetCachedStampAsync(cache, id, ct);
            }
            var s = (await api.GetCachedStampsAsync(cache, null, ct)).First(s => s.Name == name); // GetCachedStampsAsync method updates all caches related to stamps.
            return new(s.Id, s.Name, s.CreatorId, s.CreatedAt, s.UpdatedAt, s.FileId, s.IsUnicode);
        }

        public static async ValueTask<List<StampWithThumbnail>> GetCachedStampsAsync(this IStampApiAsync api, IMemoryCache cache, string? type, CancellationToken ct = default)
        {
            if (cache.TryGetValue<string?, List<StampWithThumbnail>>(CacheSections.AllStamps, type, out var stamps) && stamps is not null)
            {
                return stamps;
            }
            stamps = await api.GetStampsAsync(null, type, ct);
            cache.Set(CacheSections.AllStamps, type, stamps, DefaultExpiration);
            foreach (var s in stamps)
            {
                Stamp stampAbstraction = new(s.Id, s.Name, s.CreatorId, s.CreatedAt, s.UpdatedAt, s.FileId, s.IsUnicode);
                cache.Set(CacheSections.Stamp, s.Id, stampAbstraction, DefaultExpiration);
                cache.Set(CacheSections.NamedStamp, s.Name, s.Id, DefaultExpiration);
            }
            return stamps;
        }
    }
}
