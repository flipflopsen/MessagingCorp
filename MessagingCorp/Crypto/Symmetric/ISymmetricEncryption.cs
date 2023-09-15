using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Crypto.Symmetric
{
    public interface ISymmetricEncryption
    {
        EEncryptionStrategySymmetric EncryptionStrategy { get; }
        Task<byte[]> EncryptMessage(byte[] message);
        Task<byte[]> DecryptMessage(byte[] encryptedMessage);
    }
}
