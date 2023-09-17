using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Common.EventBus
{
    internal sealed class ActionSubscription<T> : ISubscription
    {
        public event EventHandler? Disposed;

        private readonly Func<T, CancellationToken, Task>? _callback;

        private bool _disposed;

        public ActionSubscription(Func<T, CancellationToken, Task> callback)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public Task ProcessMessage(object message, CancellationToken cancellationToken)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            return _callback!((T)message, cancellationToken);
        }

        public bool CanProcessMessage(object message) => message is T && !_disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Disposed?.Invoke(this, EventArgs.Empty);
            _disposed = true;
        }
    }
}
