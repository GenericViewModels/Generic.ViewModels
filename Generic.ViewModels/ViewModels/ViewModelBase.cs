using GenericViewModels.Core;
using GenericViewModels.Services;
using System;
using System.Threading.Tasks;

namespace GenericViewModels.ViewModels
{
    /// <summary>
    /// Base class for View-models with progress and error information
    /// </summary>
    public abstract class ViewModelBase : BindableBase
    {
        protected readonly AsyncEventSlim _initialized = new AsyncEventSlim();
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
            using (ShowProgressInfo.StartInProgress(ProgressInfoName))
            {
                await InitCoreAsync();
                _initialized.Signal();
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
