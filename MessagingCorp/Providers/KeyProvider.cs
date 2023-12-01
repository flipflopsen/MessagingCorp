using MessagingCorp.Crypto.Symmetric;
using MessagingCorp.Database.API;
using MessagingCorp.Providers.API;
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

        byte[] IKeyProvider.GetPrivateKey(EEncryptionStrategyAsymmetric strategyAsymmetric, string uid)
        {
            throw new NotImplementedException();
        }

        byte[] IKeyProvider.GetPublicKey(EEncryptionStrategyAsymmetric strategyAsymmetric, string uid)
        {
            throw new NotImplementedException();
        }

        byte[] IKeyProvider.GetSymmetricKey(EEncryptionStrategySymmetric strategySymmetric, string uid)
        {
            throw new NotImplementedException();
        }

        void IKeyProvider.SetAsymmetricKeySpecs(EEncryptionStrategyAsymmetric strategyAsymmetric, byte[] privKey, byte[] pubKey, byte[] salt)
        {
            throw new NotImplementedException();
        }

        void IKeyProvider.SetSymmetricKeySpecs(EEncryptionStrategySymmetric strategySymmetric, byte[] key, byte[] salt, byte[] iv)
        {
            throw new NotImplementedException();
        }
    }
}
