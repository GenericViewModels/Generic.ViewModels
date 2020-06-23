using GenericViewModels.Core;
using GenericViewModels.Services;
using System;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    /// <summary>
    /// Base class for View-models with progress and error information
    /// </summary>
    public abstract class ViewModelBase : BindableBase, IDisposable
    {
        protected AsyncEventSlim InitializedEvent { get; } = new AsyncEventSlim();
        protected IShowProgressInfo ShowProgressInfo { get; }

        public ViewModelBase(IShowProgressInfo showProgressInfo)
        {
            ShowProgressInfo = showProgressInfo ?? throw new ArgumentNullException(nameof(showProgressInfo));

            ShowProgressInfo.ProgressInformationChanged += (sender, name) =>
            {
                if (name == ProgressInfoName)
                {
                    RaisePropertyChanged(nameof(InProgress));
                }
            };
        }

        public string ProgressInfoName { get; set; } = "Default";

        /// <summary>
        /// Override for special initialization.
        /// Empty implementation with ViewModelBase
        /// </summary>
        /// <returns>a <see cref="Task"/></returns>
        protected virtual Task InitCoreAsync() => Task.CompletedTask;

        public async Task InitAsync()
        {
            using var progress = ShowProgressInfo.StartInProgress(ProgressInfoName);
            await InitCoreAsync();
            InitializedEvent.Signal();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                InitializedEvent.Dispose();
            }
        }

        public bool InProgress => ShowProgressInfo.InProgress(ProgressInfoName);

        #region Error Information
        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        #endregion
    }
}
