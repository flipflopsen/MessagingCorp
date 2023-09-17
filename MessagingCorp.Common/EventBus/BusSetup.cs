using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Common.EventBus
{
    public static class BusSetup
    {
        public static IBus CreateBus() => new Bus();
    }
}
