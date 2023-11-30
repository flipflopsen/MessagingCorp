using MessagingCorp.Providers.API;
using Microsoft.Extensions.Caching.Memory;

namespace MessagingCorp.Services.Caching
{
    public class CachingService : ICachingProvider
    {
        private readonly IMemoryCache cache;

        public CachingService()
        {
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        public bool IsUserInCacheWithPassword(string uid, string pass)
        {
            var gotVal = cache.TryGetValue(uid, out var value);

            if (!gotVal)
                return false;

            return (string)value! == pass;
        }

        public bool IsUserInCache(string uid)
        {
            return cache.TryGetValue(uid, out var _);
        }

        public void AddUserToCache(string uid, string pass)
        {
            cache.Set(uid, pass);
        }

        public void RemoveUserFromCache(string uid) 
        { 
            cache.Remove(uid);
        }
    }
}
