namespace MessagingCorp.Crypto.Symmetric.API
{
    public interface ISymmetricEncryption
    {
        EEncryptionStrategySymmetric EncryptionStrategy { get; }
        string EncryptMessage(string message, byte[] key, byte[] iv);

        //IMPORTANT: Decrypt wont be done on server, ONLY for debug!!
        string DecryptMessage(string encryptedMessage, byte[] key, byte[] iv);
    }
}
