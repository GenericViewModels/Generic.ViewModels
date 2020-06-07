using System;
using System.Reflection;

namespace GenericViewModels.Events
{
    internal class DelegateReference : IDelegateReference
    {
        private readonly Delegate? _delegate;
        private readonly WeakReference? _weakReference;
        private readonly MethodInfo? _method;
        private readonly Type? _delegateType;

        public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
        {
            if (keepReferenceAlive)
            {
                _delegate = @delegate;
            }
            else
            {
                _weakReference = new WeakReference(@delegate.Target);
                _method = @delegate.GetMethodInfo();
                _delegateType = @delegate.GetType();
            }
        }
        
        public Delegate? Target
        {
            get => _delegate switch
            {
                Delegate d when d != null => d,
                _ => TryGetDelegate()
            };
        }
        
        public bool TargetEquals(Delegate @delegate)
        {
            if (_delegate != null)
            {
                return _delegate == @delegate;
            }
            if (@delegate == null && _method != null && _weakReference != null)
            {
                return !_method.IsStatic && !_weakReference.IsAlive;
            }
            return _weakReference?.Target == @delegate?.Target && Equals(_method, @delegate.GetMethodInfo());
        }
        
        private Delegate? TryGetDelegate()
        {
            if (_method != null && _method.IsStatic)
            {
                return _method.CreateDelegate(_delegateType, null);
            }
            object? target = _weakReference?.Target;
            if (target != null && _method != null)
            {
                return _method.CreateDelegate(_delegateType, target);
            }
            return null;
        }
    }
}