using System;

namespace GenericViewModels.Extensions
{
    public sealed class StartEndInvoker : IDisposable
    {
        private Action _end;
        public StartEndInvoker(Action start, Action end)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (end == null) throw new ArgumentNullException(nameof(end));

            start.Invoke();
            _end = end;
        }
        public void Dispose() => _end.Invoke();
    }
}
