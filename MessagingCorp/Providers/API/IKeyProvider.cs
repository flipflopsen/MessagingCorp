using MessagingCorp.Crypto.Symmetric;

namespace MessagingCorp.Providers.API
{
    public interface IKeyProvider
    {
        // Symmetric Key Operations
        byte[] GetSymmetricKey(EEncryptionStrategySymmetric strategySymmetric, string uid);
        void SetSymmetricKeySpecs(EEncryptionStrategySymmetric strategySymmetric, byte[] key, byte[] salt = null!, byte[] iv = null!);

        // Asymmetric Key Operations
        byte[] GetPublicKey(EEncryptionStrategyAsymmetric strategyAsymmetric, string uid);
        byte[] GetPrivateKey(EEncryptionStrategyAsymmetric strategyAsymmetric, string uid);
        void SetAsymmetricKeySpecs(EEncryptionStrategyAsymmetric strategyAsymmetric, byte[] privKey, byte[] pubKey, byte[] salt = null!);
    }

}
