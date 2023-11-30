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
        void GenerateNewKeyPair(User user, int keySize);
    }
}
