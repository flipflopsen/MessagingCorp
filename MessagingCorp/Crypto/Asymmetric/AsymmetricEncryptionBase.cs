using MessagingCorp.Crypto.Symmetric;
using MessagingCorp.EntityManagement.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Crypto.Asymmetric
{
    public abstract class AsymmetricEncryptionBase : IAsymmetricEncryption
    {
        public abstract string DecryptMessage(EEncryptionStrategyAsymmetric strategy, string fromUserUid, string toUserUid);
        public abstract string EncryptMessage(EEncryptionStrategyAsymmetric strategy, string fromUserUid, string toUserUid);
        public abstract void GenerateNewKeyPair(EEncryptionStrategyAsymmetric strategy, string forUserUid);
    }
}
