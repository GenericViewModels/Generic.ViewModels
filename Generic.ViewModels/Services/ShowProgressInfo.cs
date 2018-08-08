using GenericViewModels.Extensions;
using System;
using System.Collections.Concurrent;

namespace GenericViewModels.Services
{
    public class ShowProgressInfo : IShowProgressInfo
    {
        private readonly ConcurrentDictionary<string, int> _progressCountersDict = new ConcurrentDictionary<string, int>();

        public event EventHandler<string> ProgressInformationChanged;

        private readonly object _lockSetInProgress = new object();

        protected virtual void SetInProgress(string name, bool set = true)
        {
            lock (_lockSetInProgress)
            {
                if (set)
                {
                    int count = _progressCountersDict.GetOrAdd(name, 0);
                    count++;
                    _progressCountersDict[name] = count;
                }
                else
                {
                    int count = _progressCountersDict[name];
                    count--;
                    _progressCountersDict[name] = count;
                }
            }

            RaiseProgressInformationChanged(name);
        }

        protected virtual void RaiseProgressInformationChanged(string name) => ProgressInformationChanged?.Invoke(this, name);

        public IDisposable StartInProgress(string name) =>
            new StartEndInvoker(() => SetInProgress(name), () => SetInProgress(name, false));


        public bool InProgress(string name)
        {
            if (_progressCountersDict.Keys.Contains(name))
            {
                return _progressCountersDict[name] > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
