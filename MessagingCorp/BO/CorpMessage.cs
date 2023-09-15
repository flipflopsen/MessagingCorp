using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.BO
{
    public class CorpMessage
    {
        private long _from;
        private long _to;

        private bool _IsSymmetricEncrypted = false;
        private bool _GotAsymmetricEncrypted = false;

        // Let's see later
        private byte[] _data;
        private string contents;

        public CorpMessage()
        {

        }
    }
}
