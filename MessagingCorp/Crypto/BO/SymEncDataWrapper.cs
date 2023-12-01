using MessagingCorp.Crypto.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Crypto.BO
{
    public class SymEncDataWrapper
    {
        public EEncryptionStrategySymmetric EncryptionStrategy { get; set; }

        // Additional properties based on symmetric encryption algorithm
        public string? Key { get; set; }
        public string? InitializationVector { get; set; }
        public string? AuthenticationTag { get; set; }

        // Additional properties based on algorithm
        public string? OtherData1 { get; set; } // For AES_TWOFIVESIX, AES_XTS, AES_KW
        public string? OtherData2 { get; set; } // For AES_GCM
        public string? OtherData3 { get; set; } // For AES_SIV
        public string? OtherData4 { get; set; } // For CHACHA_TWENTY, SERPENT

        public SymEncDataWrapper() { }
    }
}
