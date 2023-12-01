using MessagingCorp.Crypto.Symmetric;
using MessagingCorp.EntityManagement.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Crypto.Asymmetric
{
    public interface IAsymmetricEncryption
    {
        string DecryptMessage(EEncryptionStrategyAsymmetric strategy, string fromUserUid, string toUserUid);
        string EncryptMessage(EEncryptionStrategyAsymmetric strategy, string fromUserUid, string toUserUid);
        void GenerateNewKeyPair(EEncryptionStrategyAsymmetric strategy, string forUserUid);
    }
}
