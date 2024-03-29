﻿using Ninject.Activation.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Providers.API
{
    public interface ICachingProvider
    {
        bool IsUserInCacheWithPassword(string uid, string pass);

        void AddUserToCache(string uid, string pass);

        bool IsUserInCache(string uid);
        void RemoveUserFromCache(string uid);
    }
}
