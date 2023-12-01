using MessagingCorp.Crypto.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Crypto.BO
{
    public class AsymEncDataWrapper
    {
        public EEncryptionStrategyAsymmetric EncryptionStrategy { get; set; }
        public bool ProvideSpecs { get; set; } = false;

        // Common properties for asymmetric encryption
        public string? PrivateKey { get; set; }
        public string? PublicKey { get; set; }

        // Additional properties based on algorithm
        public string? OtherData1 { get; set; } // For RSA, ECC, DSA
        public string? OtherData2 { get; set; } // For DiffieHellmann
        public string? OtherData3 { get; set; } // For ECDSA
        public string? OtherData4 { get; set; } // For NTRUEncrypt
        public string? OtherData5 { get; set; } // For McEliece

        public AsymEncDataWrapper() { }
    }
}
