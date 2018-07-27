using System;
using System.Collections.Generic;
using System.Text;

namespace Generic.ViewModels.Services
{
    public class ItemToViewModelMap<T, TViewModel> : IItemToViewModelMap<T, TViewModel>
    {
        private readonly Dictionary<T, TViewModel> _map = new Dictionary<T, TViewModel>();
       
        public void Add(T item, TViewModel viewModel)
        {
            if (!_map.ContainsKey(item))
            {
                _map.Add(item, viewModel);
            }
        }

        public TViewModel GetViewModel(T item)
        {
            if (item == null)
            {
                return default;
            }

            return _map[item];
        }

        public void Reset() => _map.Clear();
    }
}
