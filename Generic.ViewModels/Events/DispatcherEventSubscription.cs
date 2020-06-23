using System;
using System.Threading;

namespace GenericViewModels.Events
{
    public class DispatcherEventSubscription : EventSubscription
    {
        private readonly SynchronizationContext _syncContext;

        public DispatcherEventSubscription(IDelegateReference actionReference, SynchronizationContext syncContext)
            : base(actionReference)
        {
            _syncContext = syncContext;
        }

        public override void InvokeAction(Action action) => 
            _syncContext.Post(o => action(), null);
    }

    public class DispatcherEventSubscription<TPayload> : EventSubscription<TPayload>
    {
        private readonly SynchronizationContext _syncContext;

        public DispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, SynchronizationContext context)
            : base(actionReference, filterReference)
        {
            _syncContext = context;
        }

        public override void InvokeAction(Action<TPayload> action, TPayload argument) => 
            _syncContext.Post((o) => action((TPayload)o), argument);
    }
}