using MessagingCorp.Common.Logger;
using MessagingCorp.Database.API;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using MessagingCorp.Services.Core;
using Ninject;
using Serilog;
using Serilog.Events;

namespace MessagingCorp.Services.Authentication
{
    public class BaseAuthenticationService : IAuthenticationGovernment
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageCorpDriver>("./Logs/MessageCorpDriver.log", true, LogEventLevel.Debug);

        private readonly IDatabaseAccess databaseAccess;
        private readonly ICachingProvider cachingProvider;
        private readonly IUserManagement userManagement;
        private readonly ICryptoProvider cryptoProvider;

        [Inject]
        public BaseAuthenticationService(
            IDatabaseAccess databaseAccess,
            ICachingProvider cachingProvider,
            IUserManagement userManagement,
            ICryptoProvider cryptoProvider
            )
        {
            this.databaseAccess = databaseAccess;
            this.cachingProvider = cachingProvider;
            this.userManagement = userManagement;
            this.cryptoProvider = cryptoProvider;
        }

        public async Task<bool> AuthenticateUser(string uid, string uniquePassword)
        {
            if (!cachingProvider.IsUserInCacheWithPassword(uid, uniquePassword))
            {
                // todo: crypto management hash password 
                var surrealId = userManagement.GetSurrealIdFromUid(uid);
                if (surrealId == string.Empty)
                {
                    Logger.Warning($"[BaseAuthenticationService] > User with uid {uid} doesnt have a surreal id!");
                }
                if (await databaseAccess.AuthenticateUser(surrealId, uniquePassword))
                {
                    Logger.Information($"Got user {uid} authenticated from db!");
                    if (!cachingProvider.IsUserInCacheWithPassword(uid, uniquePassword))
                        cachingProvider.AddUserToCache(uid, uniquePassword);

                    return true;
                }
                Logger.Warning($"failed auth for user {uid}!");
                return false;
            }
            Logger.Information($"Got user {uid} authenticated from cache!");
            return true;
        }

        public Task<bool> AuthorizeForLobby(string uid, string agreedKey)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUserAuthenticated(string uid)
        {
            return await Task.Run(() => cachingProvider.IsUserInCache(uid));
        }

        public Task<bool> IsUserAuthorizedForLobby(string uid, string lobbyId)
        {
            throw new NotImplementedException();
        }
    }
}
