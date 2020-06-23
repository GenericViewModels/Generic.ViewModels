using System.Collections.Concurrent;

namespace Generic.ViewModels.Services
{
    public abstract class ItemToViewModelMap<T, TViewModel> : IItemToViewModelMap<T, TViewModel>
        where T : class
        where TViewModel: class
    {
        private static readonly ConcurrentDictionary<T, TViewModel> s_map = new ConcurrentDictionary<T, TViewModel>();
       
        public void Add(T item, TViewModel viewModel)
        {
            if (item == default) return;

            if (!s_map.ContainsKey(item))
            {
                s_map.TryAdd(item, viewModel);
            }
        }

        public TViewModel? GetViewModel(T? item)
        {
            if (item == null)
            {
                return default;
            }

            if (s_map.TryGetValue(item, out TViewModel value))
            {
                return value;
            }
            else
            {
                s_map.TryAdd(item, CreateViewModel(item));
                return s_map[item];
            }
        }

        protected abstract TViewModel CreateViewModel(T item);

        public void Reset() => s_map.Clear();
    }
}
