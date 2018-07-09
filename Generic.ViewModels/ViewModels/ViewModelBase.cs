using Prism.Mvvm;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    /// <summary>
    /// Base class for View-models with progress and error information
    /// </summary>
    public abstract class ViewModelBase : BindableBase
    {
        /// <summary>
        /// Override for special initialization.
        /// Empty implementation with ViewModelBase
        /// </summary>
        /// <returns>a task</returns>
        public virtual Task InitAsync() => Task.CompletedTask;

        #region Progress Information
        private class StateSetter : IDisposable
        {
            private Action _end;
            public StateSetter(Action start, Action end)
            {
                start?.Invoke();
                _end = end;
            }
            public void Dispose() => _end?.Invoke();
        }

        private int _inProgressCounter = 0;
        protected void SetInProgress(bool set = true)
        {
            if (set)
            {
                Interlocked.Increment(ref _inProgressCounter);
                RaisePropertyChanged(nameof(InProgress));
            }
            else
            {
                Interlocked.Decrement(ref _inProgressCounter);
                RaisePropertyChanged(nameof(InProgress));
            }
        }

        public IDisposable StartInProgress() => 
            new StateSetter(() => SetInProgress(), () => SetInProgress(false));

        public bool InProgress => _inProgressCounter != 0;
        #endregion

        #region Error Information
        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        #endregion
    }
}
