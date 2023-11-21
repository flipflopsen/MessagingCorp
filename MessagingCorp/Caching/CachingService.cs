using MessagingCorp.Providers.API;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Caching
{
    public class CachingService : ICachingProvider
    {
        private readonly IMemoryCache cache;

        public CachingService() 
        {
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        public bool IsUserInCache(string uid, string pass)
        {
            var gotVal = cache.TryGetValue(uid, out var value);

            if (!gotVal)
                return false;

            return (string)value! == pass;
        }

        public void AddUserToCache(string uid, string pass)
        {
            cache.Set(uid, pass);
        }

    }
}
