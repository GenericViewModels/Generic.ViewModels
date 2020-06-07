using System;
using System.Collections.Generic;
using System.Threading;

namespace GenericViewModels.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly object _lockEvents = new object();
        private readonly Dictionary<Type, EventBase> _events = new Dictionary<Type, EventBase>();

        private readonly SynchronizationContext _syncContext = SynchronizationContext.Current;

        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            lock (_lockEvents)
            {
                EventBase event1;

                if (_events.TryGetValue(typeof(TEventType), out event1))
                {
                    if (event1 is TEventType existingEvent)
                    {
                        return existingEvent;
                    }
                    else
                    {
                        throw new InvalidOperationException("Wrong event type in event list");
                    }
                }
                else
                {
                    TEventType newEvent = new TEventType();
                    newEvent.SynchronizationContext = _syncContext;
                    _events[typeof(TEventType)] = newEvent;

                    return newEvent;
                }
            }
        }
    }
}
