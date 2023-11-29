using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Database.DAO
{
    public class SymmetricSpecDao
    {
        public string? Key { get; set; } = string.Empty;
        public string? IV { get; set; } = string.Empty;
        public string? Salt { get; set; } = string.Empty;

        public SymmetricSpecDao(string? key, string? iV, string? salt)
        {
            Key = key;
            IV = iV;
            Salt = salt;
        }
    }
}
