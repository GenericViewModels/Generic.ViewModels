using System;

namespace GenericViewModels.Events
{
    public class EventSubscription : IEventSubscription
    {
        private readonly IDelegateReference _actionReference;

        public EventSubscription(IDelegateReference actionReference)
        {
            _actionReference = actionReference;
        }

        public Action? Action => (Action?)_actionReference.Target;

        public SubscriptionToken? SubscriptionToken { get; set; }

        public virtual Action<object[]>? GetExecutionStrategy()
        {
            Action? action = this.Action;
            if (action != null)
            {
                return arguments =>
                {
                    InvokeAction(action);
                };
            }
            return null;
        }

        public virtual void InvokeAction(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            action.Invoke();
        }
    }

    public class EventSubscription<TPayload> : IEventSubscription
    {
        private readonly IDelegateReference _actionReference;
        private readonly IDelegateReference _filterReference;

        public EventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
        {
            _actionReference = actionReference;
            _filterReference = filterReference;
        }

        public Action<TPayload>? Action => (Action<TPayload>?)_actionReference.Target;

        public Predicate<TPayload>? Filter => (Predicate<TPayload>?)_filterReference.Target;

        public SubscriptionToken? SubscriptionToken { get; set; }

        public virtual Action<object[]>? GetExecutionStrategy()
        {
            Action<TPayload>? action = this.Action;
            Predicate<TPayload>? filter = this.Filter;
            if (action != null && filter != null)
            {
                return arguments =>
                {
                    TPayload argument = default;
                    if (arguments != null && arguments.Length > 0 && arguments[0] != null)
                    {
                        argument = (TPayload)arguments[0];
                    }

                    if (argument != null && filter(argument))
                    {
                        InvokeAction(action, argument);
                    }
                };
            }
            return null;
        }

        public virtual void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            action(argument);
        }
    }
}