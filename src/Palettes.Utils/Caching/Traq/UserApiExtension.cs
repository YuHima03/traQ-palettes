using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using Traq.Api;
using Traq.Model;

namespace Palettes.Utils.Caching.Traq
{
    public static class UserApiExtension
    {
        static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(1);
        static readonly TimeSpan UserNameExpiration = TimeSpan.FromDays(1);

        public static async ValueTask<Guid> GetCachedUserIdAsync(this IUserApiAsync api, IMemoryCache cache, string username, CancellationToken ct)
        {
            if (cache.TryGetValue<string, Guid>(CacheSections.UserNames, username, out var userId) && userId != Guid.Empty)
            {
                return userId;
            }
            var user = (await api.GetUsersAsync(true, username, ct)).First();
            cache.Set(CacheSections.UserNames, username, user.Id, UserNameExpiration);
            cache.Set(CacheSections.User, user.Id, user, DefaultExpiration);
            return userId;
        }

        public static async ValueTask<UserDetail> GetCachedUserDetailAsync(this IUserApiAsync api, IMemoryCache cache, Guid userId, CancellationToken ct)
        {
            if (cache.TryGetValue<Guid, UserDetail>(CacheSections.UserDetail, userId, out var user) && user is not null)
            {
                return user;
            }
            user = await api.GetUserAsync(userId, ct);
            cache.Set(CacheSections.UserNames, user.Name, user.Id, UserNameExpiration);
            cache.Set(CacheSections.UserDetail, userId, user, DefaultExpiration);
            cache.Set(CacheSections.User, userId, new User(user.Id, user.Name, user.DisplayName, user.IconFileId, user.Bot, user.State, user.UpdatedAt), DefaultExpiration);
            return user;
        }

        public static async ValueTask<ConcurrentDictionary<Guid, User>> GetCachedUsersAsync(this IUserApiAsync api, IMemoryCache cache, bool includeSuspended, CancellationToken ct)
        {
            if (cache.TryGetValue<bool, ConcurrentDictionary<Guid, User>>(CacheSections.AllUsers, includeSuspended, out var users) && users is not null)
            {
                return users;
            }
            users = new((await api.GetUsersAsync(includeSuspended, null, ct)).Select(u => KeyValuePair.Create(u.Id, u)));
            cache.Set(CacheSections.AllUsers, includeSuspended, users, DefaultExpiration);
            foreach (var (_, u) in users)
            {
                cache.Set(CacheSections.UserNames, u.Name, u.Id, UserNameExpiration);
                cache.Set(CacheSections.User, u.Id, u, DefaultExpiration);
            }
            return users;
        }
    }
}
