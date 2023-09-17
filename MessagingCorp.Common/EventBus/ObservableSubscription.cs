using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Common.EventBus
{
    internal sealed class ObservableSubscription<T> : ISubscription, IObservable<T>
    {
        public event EventHandler? Disposed;

        private bool _disposed;
        private IObserver<T>? _observer;

        public bool CanProcessMessage(object message) => message is T && !_disposed;

        public Task ProcessMessage(object message, CancellationToken cancellationToken)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            _observer?.OnNext((T)message);

            return Task.CompletedTask;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_observer != null)
            {
                throw new InvalidOperationException(
                    "This observable already has an observer and does not support multiple observers");
            }

            _observer = observer;
            return this;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Disposed?.Invoke(this, EventArgs.Empty);
            _observer = null;
            _disposed = true;
        }
    }
}
