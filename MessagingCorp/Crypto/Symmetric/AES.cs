using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MessagingCorp.Crypto.Symmetric.API;
using MessagingCorp.Utils.Logger;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Serilog;

namespace MessagingCorp.Crypto.Symmetric
{
    public class AES : ISymmetricEncryption
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<ISymmetricEncryption>("./Logs/Encryption-Symmetric.log", true, Serilog.Events.LogEventLevel.Debug);

        private readonly EEncryptionStrategySymmetric _encryptionStrategy;

        public AES(EEncryptionStrategySymmetric encryptionStrategy)
        {
            _encryptionStrategy = encryptionStrategy;
        }
        public EEncryptionStrategySymmetric EncryptionStrategy => _encryptionStrategy;

        public string EncryptMessage(string message, byte[] key, byte[] iv)
        {
            var messageBytes = Convert.FromBase64String(message);

            var cipher = GetCipher(true, key, iv);
           
                var cipherText = new byte[cipher.GetOutputSize(message.Length)];
                var len = cipher.ProcessBytes(messageBytes, 0, message.Length, cipherText, 0);
                len += cipher.DoFinal(cipherText, len);

                // If len is less than the length of the output buffer, create a new array with the correct length
                if (len < cipherText.Length)
                {
                    var truncatedCipherText = new byte[len];
                    Array.Copy(cipherText, truncatedCipherText, len);
                    return Convert.ToBase64String(truncatedCipherText);
                }

                return Convert.ToBase64String(cipherText);
            }

        public string DecryptMessage(string encryptedMessage, byte[] key, byte[] iv)
        {
            var encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
            var cipher = GetCipher(false, key, iv);
            var decryptedText = new byte[cipher.GetOutputSize(encryptedMessage.Length)];
            var len = cipher.ProcessBytes(encryptedMessageBytes, 0, encryptedMessage.Length, decryptedText, 0);
            len += cipher.DoFinal(decryptedText, len);

            // If len is less than the length of the output buffer, create a new array with the correct length
            if (len < decryptedText.Length)
            {
                var truncatedDecryptedText = new byte[len];
                Array.Copy(decryptedText, truncatedDecryptedText, len);
                return Convert.ToBase64String(truncatedDecryptedText);
            }

            return Convert.ToBase64String(decryptedText);
        }

        private IBufferedCipher GetCipher(bool forEncryption, byte[] key, byte[] iv)
        {
            var keyParameter = ParameterUtilities.CreateKeyParameter("AES", key);

            IBufferedCipher cipher;
            switch (_encryptionStrategy)
            {
                case EEncryptionStrategySymmetric.AES_TWOFIVESIX:
                    cipher = CipherUtilities.GetCipher("AES/ECB/PKCS7Padding");
                    break;
                case EEncryptionStrategySymmetric.AES_GCM:
                    cipher = CipherUtilities.GetCipher("AES/GCM/NoPadding");
                    break;
                case EEncryptionStrategySymmetric.AES_XTS:
                    // XTS requires two keys, implement accordingly
                    throw new NotImplementedException("AES XTS is not implemented in this example.");
                case EEncryptionStrategySymmetric.AES_KW:
                    cipher = CipherUtilities.GetCipher("AES/ECB/NoPadding");
                    break;
                case EEncryptionStrategySymmetric.AES_SIV:
                    // Add implementation for AES SIV
                    throw new NotImplementedException("AES SIV is not implemented in this example.");
                default:
                    throw new ArgumentException("Invalid encryption strategy");
            }

            var parameters = new ParametersWithIV(keyParameter, iv);
            cipher.Init(forEncryption, parameters);

            return cipher;
        }
    }
}
