using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GenericViewModels.Events
{
    public abstract class EventBase
    {
        private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

        public SynchronizationContext? SynchronizationContext { get; set; }

        private object _lockSubscriptions = new object();
        protected ICollection<IEventSubscription> Subscriptions => _subscriptions;

        protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
        {
            if (eventSubscription == null) throw new ArgumentNullException(nameof(eventSubscription));

            eventSubscription.SubscriptionToken = new SubscriptionToken(Unsubscribe);

            lock (_lockSubscriptions)
            {
                Subscriptions.Add(eventSubscription);
            }
            return eventSubscription.SubscriptionToken;
        }

        protected virtual void InternalPublish(params object[] arguments)
        {
            List<Action<object[]>> executionStrategies = PruneAndReturnStrategies();
            foreach (var executionStrategy in executionStrategies)
            {
                executionStrategy(arguments);
            }
        }

        public virtual void Unsubscribe(SubscriptionToken token)
        {
            lock (_lockSubscriptions)
            {
                IEventSubscription subscription = Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                if (subscription != null)
                {
                    Subscriptions.Remove(subscription);
                }
            }
        }

        public virtual bool Contains(SubscriptionToken token)
        {
            lock (_lockSubscriptions)
            {
                IEventSubscription subscription = Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                return subscription != null;
            }
        }

        private List<Action<object[]>> PruneAndReturnStrategies()
        {
            List<Action<object[]>> returnList = new List<Action<object[]>>();

            lock (_lockSubscriptions)
            {
                for (var i = Subscriptions.Count - 1; i >= 0; i--)
                {
                    Action<object[]>? listItem =
                        _subscriptions[i].GetExecutionStrategy();

                    if (listItem == null)
                    {
                        // Prune from main list. Log?
                        _subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        returnList.Add(listItem);
                    }
                }
            }

            return returnList;
        }

        public void Prune()
        {
            lock (_lockSubscriptions)
            {
                for (var i = Subscriptions.Count - 1; i >= 0; i--)
                {
                    if (_subscriptions[i].GetExecutionStrategy() == null)
                    {
                        _subscriptions.RemoveAt(i);
                    }
                }
            }
        }
    }
}
