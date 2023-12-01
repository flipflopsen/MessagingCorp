using MessagingCorp.Crypto.Symmetric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.DAO
{

    public static class RecordDAOs
    {
        // activeLobbies is a list in a long string with ';' separator, like "lid;lid;lid"
        // user string same as above but with uid;uid;uid
        public record struct Lobby(int id, string name, string users);

        // I don't think this will be needed
        public record struct EncryptedCorpMessage(string contents, EEncryptionStrategyAsymmetric encAsym, EEncryptionStrategySymmetric encSym);
    }
}
