using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Common.EventBus
{
    public interface IBus
    { 
        IObservable<T> Observe<T>();
        Task Publish([NotNull] object message, CancellationToken cancellationToken = default);
        IDisposable Subscribe<T>(Func<T, CancellationToken, Task> callback);
        IDisposable Subscribe<T>(Func<T, Task> callback);
        IDisposable SubscribeSync<T>(Action<T> callback);
    }
}
