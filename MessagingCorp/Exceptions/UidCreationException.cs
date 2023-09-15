using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Exceptions
{
    public class UidCreationException : Exception
    {
        public UidCreationException() { }
        public UidCreationException(string message) : base(message) { }
        public UidCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
