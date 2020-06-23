using GenericViewModels.Core;
using System.Threading.Tasks;
using Xunit;

namespace Generic.ViewModels.Tests.Core
{
    public class AsyncEventSlimTests
    {
        [Fact]
        public async Task SignalIsSet()
        {
            var ev = new AsyncEventSlim();
            bool completed = false;

            Task t1 = Task.Run(async () =>
            {
                await Task.Delay(100);
                await ev.WaitAsync();
                completed = true;
            });

            ev.Signal();
            await t1; // event is set when task completes

            Assert.True(completed);
        }

        [Fact]
        public async Task SignalIsNotSet()
        {
            var ev = new AsyncEventSlim();
            bool completed = false;

            Task t1 = Task.Run(async () =>
            {
                await Task.Delay(100);
                await ev.WaitAsync();
                completed = true;
            });

            ev.Signal();
            await Task.Delay(10); // event should not be set at this time

            Assert.False(completed);
        }
    }
}
