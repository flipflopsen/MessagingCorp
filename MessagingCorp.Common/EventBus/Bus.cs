using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Common.EventBus
{
    internal sealed class Bus : IBus
    {
        private readonly List<ISubscription> _subscriptions;

        public Bus()
        {
            _subscriptions = new List<ISubscription>();
        }

        /// <inheritdoc />
        public IObservable<T> Observe<T>()
        {
            var subscription = new ObservableSubscription<T>();
            subscription.Disposed += (s, e) => _subscriptions.Remove(subscription);
            _subscriptions.Add(subscription);

            return subscription;
        }

        public async Task Publish(object message, CancellationToken cancellationToken = default)
        {
            var temp = _subscriptions.ToList();

            foreach (var subscription in temp)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (subscription.CanProcessMessage(message))
                {
                    await subscription.ProcessMessage(message, cancellationToken);
                }
            }
        }

        public IDisposable Subscribe<T>(Func<T, CancellationToken, Task> callback)
        {
            var subscription = new ActionSubscription<T>(callback);
            subscription.Disposed += (s, e) => _subscriptions.Remove(subscription);
            _subscriptions.Add(subscription);

            return subscription;
        }

        public IDisposable Subscribe<T>(Func<T, Task> callback) =>
            Subscribe<T>((message, cancellationToken) => callback(message));

        public IDisposable SubscribeSync<T>(Action<T> callback) =>
            Subscribe<T>(message => Task.Run(() => callback(message)));
    }
}
