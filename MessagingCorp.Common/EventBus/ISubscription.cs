using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Common.EventBus
{
    internal interface ISubscription : IDisposable
    {
        event EventHandler Disposed;
        bool CanProcessMessage(object message);
        Task ProcessMessage(object message, CancellationToken cancellationToken);
    }
}
