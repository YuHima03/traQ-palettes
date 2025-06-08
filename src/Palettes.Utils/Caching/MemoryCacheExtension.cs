using Microsoft.Extensions.Caching.Memory;

namespace Palettes.Utils.Caching
{
    public static class MemoryCacheExtension
    {
        public static void Set<TKey, TValue>(this IMemoryCache cache, string section, TKey key, TValue value, TimeSpan absoluteExpirationRelativeToNow)
        {
            cache.Set($"{section}:{key}", value, absoluteExpirationRelativeToNow);
        }

        public static bool TryGetValue<TKey, TValue>(this IMemoryCache cache, string section, TKey key, out TValue? result)
        {
            return cache.TryGetValue($"{section}:{key}", out result);
        }
    }
}
