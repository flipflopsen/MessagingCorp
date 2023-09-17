using MessagingCorp.Common.EventBus;
using MessagingCorp.Providers.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Providers
{
    public class MessageBusProvider : IMessageBusProvider
    {
        private readonly IBus _bus;

        public MessageBusProvider() 
        {
            _bus = BusSetup.CreateBus();
        }
        public IBus GetMessageBus()
        {
            return _bus;
        }
    }
}
