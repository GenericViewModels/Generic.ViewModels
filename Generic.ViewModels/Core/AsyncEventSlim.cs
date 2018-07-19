using System.Threading;
using System.Threading.Tasks;

namespace GenericViewModels.Core
{
    /// <summary>
    /// AsyncEventSlim allows non-blocking wait for a signal to be set
    /// With the implementation, <see cref="ManualResetEventSlim"/> is used
    /// </summary>
    public class AsyncEventSlim
    {
        private ManualResetEventSlim _event = new ManualResetEventSlim(false);

        /// <summary>
        /// Sets the event
        /// </summary>
        public void Signal() => _event.Set();

        /// <summary>
        /// non-blocking wait for the event to be set
        /// </summary>
        /// <returns>a <see cref="Task"/> that completes when the event is set</returns>
        public async Task WaitAsync()
            => await Task.Run(() => _event.Wait());
    }
}
