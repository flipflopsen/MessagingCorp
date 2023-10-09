using MessagingCorp.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace MessagingCorp.Crypto.Asymmetric.AsymmetricAlgorithms
{
    public class RSA : AsymmetricEncryptionBase
    {
        public override void GenerateNewKeyPair(User user, int keySize)
        {
            var random = new SecureRandom();
            var keyGenerationParameters = new KeyGenerationParameters(random, keySize);
            RsaKeyPairGenerator generator = new RsaKeyPairGenerator();
            generator.Init(keyGenerationParameters);

            var keyPair = generator.GenerateKeyPair();
            var privateKey = JsonConvert.SerializeObject(keyPair.Private);
            var publicKey = JsonConvert.SerializeObject(keyPair.Public);
        }
    }
}
