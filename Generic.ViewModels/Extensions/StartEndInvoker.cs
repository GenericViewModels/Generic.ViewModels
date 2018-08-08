using System;

namespace GenericViewModels.Extensions
{
    public class StartEndInvoker : IDisposable
    {
        private Action _end;
        public StartEndInvoker(Action start, Action end)
        {
            start?.Invoke();
            _end = end;
        }
        public void Dispose() => _end?.Invoke();
    }
}
