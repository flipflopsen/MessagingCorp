using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingCorp.Utils;
using Serilog.Events;
using Serilog;
using MessagingCorp.Utils.Logger;
using MessagingCorp.Services;

namespace MessagingCorp.Auth
{
    public class BaseAuthenticator : IAuthenticationGovernment
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private IDatabaseAccess databaseAccess;
        private ICachingProvider cachingProvider;
        //private ICryptoProvider cryptoProvider;

        public BaseAuthenticator(IKernel kernel)
        {
            databaseAccess = kernel.Get<IDatabaseAccess>();
            cachingProvider = kernel.Get<ICachingProvider>();
            //cryptoProvider = kernel.Get<ICryptoProvider>();
        }

        public void InitializeService(IKernel kernel)
        {
            databaseAccess = kernel.Get<IDatabaseAccess>();
            cachingProvider = kernel.Get<ICachingProvider>();
            //cryptoProvider = kernel.Get<ICryptoProvider>();
        }

        public bool AuthenticateUser(string uid, string uniquePassword)
        {
            if (!cachingProvider.IsUserInCache(uid, uniquePassword))
            {
                if (databaseAccess.AuthenticateUser(uid, uniquePassword))
                {
                    Logger.Information($"Got user {uid} authenticated from db!");
                    if (!cachingProvider.IsUserInCache(uid, uniquePassword))
                        cachingProvider.AddUserToCache(uid, uniquePassword);

                    return true;
                }
                Logger.Warning($"failed auth for user {uid}!");
                return false;
            }
            Logger.Information($"Got user {uid} authenticated from cache!");
            return true;
        }

        public bool AuthorizeForLobby(string uid, string agreedKey)
        {
            throw new NotImplementedException();
        }
    }
}
