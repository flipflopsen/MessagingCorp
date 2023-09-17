using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Providers.API
{
    public interface ICryptoProvider
    {
        string Encrypt(string message);
        string Decrypt(string cipher);
    }
}
