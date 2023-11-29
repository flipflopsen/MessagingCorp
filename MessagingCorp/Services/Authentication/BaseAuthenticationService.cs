using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using MessagingCorp.Services.Core;
using MessagingCorp.Utils.Logger;
using Ninject;
using Serilog;
using Serilog.Events;

namespace MessagingCorp.Services.Authentication
{
    public class BaseAuthenticationService : IAuthenticationGovernment
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private IDatabaseAccess databaseAccess;
        private ICachingProvider cachingProvider;
        //private ICryptoProvider cryptoProvider;

        public BaseAuthenticationService(IKernel kernel)
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
                // todo: crypto management hash password 
                if (databaseAccess.AuthenticateUser(uid, uniquePassword).Result)
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
