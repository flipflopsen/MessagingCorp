using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Auth
{
    public class BaseAuthenticator : IAuthenticationGovernment
    {
        private IDatabaseAccess databaseAccess;
        private ICachingProvider cachingProvider;
        private ICryptoProvider cryptoProvider;

        public void InitializeService(IKernel kernel)
        {
            databaseAccess = kernel.Get<IDatabaseAccess>();
            cachingProvider = kernel.Get<ICachingProvider>();
            cryptoProvider = kernel.Get<ICryptoProvider>();
        }

        public bool AuthenticateUser(string uid, string uniquePassword)
        {
            throw new NotImplementedException();
        }

        public bool AuthorizeForLobby(string uid, string agreedKey)
        {
            throw new NotImplementedException();
        }
    }
}
