using MessagingCorp.Common.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Providers.API
{
    public interface IMessageBusProvider
    {
        IBus GetMessageBus();
    }
}
