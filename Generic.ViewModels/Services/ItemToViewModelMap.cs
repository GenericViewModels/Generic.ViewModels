using System.Collections.Concurrent;

namespace Generic.ViewModels.Services
{
    /// <summary>
    /// For using a view-model for items (e.g. items in a list with associated commands, this class can be used.
    /// </summary>
    /// <typeparam name="T">The type of the item</typeparam>
    /// <typeparam name="TViewModel">The type of the associated view-model class</typeparam>
    public abstract class ItemToViewModelMap<T, TViewModel> : IItemToViewModelMap<T, TViewModel>
        where T : class
        where TViewModel: class
    {
        private static readonly ConcurrentDictionary<T, TViewModel> s_map = new ConcurrentDictionary<T, TViewModel>();
       
        /// <summary>
        /// Adds the view-model instance to the map to later retrieve the view-model for the instance.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="viewModel"></param>
        public void Add(T item, TViewModel viewModel)
        {
            if (item == default) return;

            if (!s_map.ContainsKey(item))
            {
                s_map.TryAdd(item, viewModel);
            }
        }

        /// <summary>
        /// Returns the view-model instance assoiated with the item.
        /// </summary>
        /// <param name="item">The item instance</param>
        /// <returns>The cached view-model instance</returns>
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

        /// <summary>
        /// Implement this method to create a view-model instance based on the item.
        /// </summary>
        /// <param name="item">The item to return a view-model instance</param>
        /// <returns>The view-model instance</returns>
        protected abstract TViewModel CreateViewModel(T item);

        /// <summary>
        /// Clears the underlying map for the view-model instances.
        /// </summary>
        public void Reset() => s_map.Clear();
    }
}
