using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Utils
{
    public static class Loggerino
    {
        public static ILogger log = new LoggerConfiguration().MinimumLevel.Debug().CreateLogger();
    }
}
