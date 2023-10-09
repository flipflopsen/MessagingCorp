using MessagingCorp.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Crypto.Asymmetric
{
    public abstract class AsymmetricEncryptionBase : IAsymmetricEncryption
    {
        public abstract void GenerateNewKeyPair(User user, int keySize);
    }
}
