using MessagingCorp.Crypto.Symmetric;
using MessagingCorp.Providers.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Providers
{
    public class CryptoProvider : ICryptoProvider
    {
        public CryptoProvider() { }

        public string DecryptAsymmetric(EEncryptionStrategyAsymmetric cryptoStrategy, string uidFrom, string uidTo, string message)
        {
            throw new NotImplementedException();
        }

        public string DecryptSymmetric(EEncryptionStrategySymmetric cryptoStrategy, string uid, string message)
        {
            throw new NotImplementedException();
        }

        public string EncryptAsymmetric(EEncryptionStrategyAsymmetric cryptoStrategy, string uid, string message)
        {
            throw new NotImplementedException();
        }

        public string EncryptSymmetric(EEncryptionStrategySymmetric cryptoStrategy, string uid, string message)
        {
            throw new NotImplementedException();
        }
    }
}
