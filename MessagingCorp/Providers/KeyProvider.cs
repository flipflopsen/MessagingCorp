using MessagingCorp.Crypto.Symmetric;
using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Providers
{
    public class KeyProvider : IKeyProvider
    {
        private readonly IDatabaseAccess db;

        public KeyProvider(
            IDatabaseAccess dbAccess
            ) 
        { 
        }

        public byte[] GetPrivateKey(EEncryptionStrategyAsymmetric strat, string uid)
        {
            throw new NotImplementedException();
        }

        public byte[] GetPublicKey(EEncryptionStrategyAsymmetric strat, string uid)
        {
            throw new NotImplementedException();
        }

        public byte[] GetSymmetricKey(EEncryptionStrategySymmetric strat, string uid)
        {
            throw new NotImplementedException();
        }
    }
}
