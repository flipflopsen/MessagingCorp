using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Database.DAO
{
    public class AsymmetricSpecDao
    {
        public string? PubKey {  get; set; } = string.Empty;
        public string? PrivKey { get; set; } = string.Empty;
        public string? Method { get; set; } = string.Empty;

        public AsymmetricSpecDao(string? pubKey, string? privKey, string? method)
        {
            PubKey = pubKey;
            PrivKey = privKey;
            Method = method;
        }

    }
}
