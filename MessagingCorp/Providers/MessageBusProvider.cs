using MessagingCorp.Common.EventBus;
using MessagingCorp.Providers.API;

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
