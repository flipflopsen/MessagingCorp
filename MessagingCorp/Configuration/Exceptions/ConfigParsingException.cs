using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Configuration.Exceptions
{
    public class ConfigParsingException : Exception
    {
        public ConfigParsingException() { }
        public ConfigParsingException(string message) : base(message) { }
        public ConfigParsingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
