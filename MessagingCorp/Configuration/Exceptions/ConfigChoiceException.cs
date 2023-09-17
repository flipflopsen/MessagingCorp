using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Configuration.Exceptions
{
    public class ConfigChoiceException : Exception
    {
        public ConfigChoiceException() { }
        public ConfigChoiceException(string message) : base(message) { }
        public ConfigChoiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
