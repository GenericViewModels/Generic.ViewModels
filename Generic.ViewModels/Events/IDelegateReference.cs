using System;

namespace GenericViewModels.Events
{
    public interface IDelegateReference
    {
        Delegate? Target { get; }
    }
}