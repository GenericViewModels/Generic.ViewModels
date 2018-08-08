using System;

namespace GenericViewModels.Services
{
    public interface IShowProgressInfo
    {
        event EventHandler<string> ProgressInformationChanged;

        bool InProgress(string name);
        IDisposable StartInProgress(string name);
    }
}