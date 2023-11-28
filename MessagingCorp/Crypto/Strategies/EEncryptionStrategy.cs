using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Crypto.Symmetric
{

    public enum EEncryptionStrategySymmetric
    {
        AES_TWOFIVESIX,
        AES_GCM,
        AES_XTS,
        AES_KW,
        AES_SIV,
        CHACHA_TWENTY,
        SERPENT,
    }

    public enum EEncryptionStrategyAsymmetric
    {
        RSA,
        ECC,
        DSA,
        DiffieHellmann,
        ECDSA,
        NTRUEncrypt,
        McEliece
    }
}

