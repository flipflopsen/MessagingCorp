using MessagingCorp.Crypto.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Providers.API
{
    public interface ICryptoProvider
    {
        string EncryptSymmetric(EEncryptionStrategySymmetric cryptoStrategy, string uid, string message);
        string DecryptSymmetric(EEncryptionStrategySymmetric cryptoStrategy, string uid, string message);
        string EncryptAsymmetric(EEncryptionStrategyAsymmetric cryptoStrategy, string uid, string message);
        string DecryptAsymmetric(EEncryptionStrategyAsymmetric cryptoStrategy, string uidFrom, string uidTo, string message);
    }
}
