using System;
using System.Threading.Tasks;

namespace GenericViewModels.Events
{
    public class BackgroundEventSubscription : EventSubscription
    {
        public BackgroundEventSubscription(IDelegateReference actionReference)
            : base(actionReference)
        {
        }

        public override void InvokeAction(Action action) => Task.Run(action);
    }

    public class BackgroundEventSubscription<TPayload> : EventSubscription<TPayload>
    {
        public BackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
            : base(actionReference, filterReference)
        {
        }

        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            Task.Run(() => action(argument));
        }
    }
}