using MessagingCorp.Crypto.Symmetric;

namespace MessagingCorp.Providers.API
{
    public interface IKeyProvider
    {
        byte[] GetSymmetricKey(EEncryptionStrategySymmetric strat, string uid);
        byte[] GetPublicKey(EEncryptionStrategyAsymmetric strat, string uid);
        byte[] GetPrivateKey(EEncryptionStrategyAsymmetric strat, string uid);
    }
}
