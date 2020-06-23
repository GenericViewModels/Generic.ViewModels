using System;
using System.Linq;

namespace GenericViewModels.Events
{
    public class PubSubEvent : EventBase
    {
        public SubscriptionToken Subscribe(Action action)
        {
            return Subscribe(action, ThreadOption.PublisherThread);
        }

        public SubscriptionToken Subscribe(Action action, ThreadOption threadOption) => 
            Subscribe(action, threadOption, false);

        public SubscriptionToken Subscribe(Action action, bool keepSubscriberReferenceAlive) => 
            Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);

        public virtual SubscriptionToken Subscribe(Action action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            DispatcherEventSubscription GetDispatcherEventSubscription(IDelegateReference actionReference)
            {
                if (SynchronizationContext == null) throw new InvalidOperationException("event aggregator not on UI thread");
                return new DispatcherEventSubscription(actionReference, SynchronizationContext);
            }

            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);

            EventSubscription subscription =
                threadOption switch
                {
                    ThreadOption.PublisherThread => new EventSubscription(actionReference),
                    ThreadOption.BackgroundThread => new BackgroundEventSubscription(actionReference),
                    ThreadOption.UIThread => GetDispatcherEventSubscription(actionReference),
                    _ => new EventSubscription(actionReference)
                };

            return InternalSubscribe(subscription);
        }

        public virtual void Publish()
        {
            InternalPublish();
        }

        public virtual void Unsubscribe(Action subscriber)
        {
            lock (Subscriptions)
            {
                IEventSubscription eventSubscription = Subscriptions.Cast<EventSubscription>().FirstOrDefault(evt => evt.Action == subscriber);
                if (eventSubscription != null)
                {
                    Subscriptions.Remove(eventSubscription);
                }
            }
        }

        public virtual bool Contains(Action subscriber)
        {
            IEventSubscription eventSubscription;
            lock (Subscriptions)
            {
                eventSubscription = Subscriptions.Cast<EventSubscription>().FirstOrDefault(evt => evt.Action == subscriber);
            }
            return eventSubscription != null;
        }
    }

    public class PubSubEvent<TPayload> : EventBase
    {
        public SubscriptionToken Subscribe(Action<TPayload> action) => 
            Subscribe(action, ThreadOption.PublisherThread);

        public virtual SubscriptionToken Subscribe(Action<TPayload> action, Predicate<TPayload> filter)
        {
            return Subscribe(action, ThreadOption.PublisherThread, false, filter);
        }

        public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption)
        {
            return Subscribe(action, threadOption, false);
        }

        public SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
        }

        public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
        }

        public virtual SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayload>? filter)
        {
            DispatcherEventSubscription<TPayload> GetDispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
            {
                if (SynchronizationContext == null) throw new InvalidOperationException("event aggregator not on UI thread");
                return new DispatcherEventSubscription<TPayload>(actionReference, filterReference, SynchronizationContext);
            }

            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
            IDelegateReference filterReference;
            if (filter != null)
            {
                filterReference = new DelegateReference(filter, keepSubscriberReferenceAlive);
            }
            else
            {
                filterReference = new DelegateReference(new Predicate<TPayload>(delegate { return true; }), true);
            }
            
            EventSubscription<TPayload> subscription =
                threadOption switch
                {
                    ThreadOption.PublisherThread => new EventSubscription<TPayload>(actionReference, filterReference),
                    ThreadOption.BackgroundThread => new BackgroundEventSubscription<TPayload>(actionReference, filterReference),
                    ThreadOption.UIThread => GetDispatcherEventSubscription(actionReference, filterReference),
                    _ => new EventSubscription<TPayload>(actionReference, filterReference)
                };

            return InternalSubscribe(subscription);
        }

        public virtual void Publish(TPayload payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            InternalPublish(payload);
        }

        public virtual void Unsubscribe(Action<TPayload> subscriber)
        {
            lock (Subscriptions)
            {
                IEventSubscription eventSubscription = Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);
                if (eventSubscription != null)
                {
                    Subscriptions.Remove(eventSubscription);
                }
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> if there is a subscriber matching <see cref="Action{TPayload}"/>.
        /// </summary>
        /// <param name="subscriber">The <see cref="Action{TPayload}"/> used when subscribing to the event.</param>
        /// <returns><see langword="true"/> if there is an <see cref="Action{TPayload}"/> that matches; otherwise <see langword="false"/>.</returns>
        public virtual bool Contains(Action<TPayload> subscriber)
        {
            IEventSubscription eventSubscription;
            lock (Subscriptions)
            {
                eventSubscription = Subscriptions.Cast<EventSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);
            }
            return eventSubscription != null;
        }
    }
}
