using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GenericViewModels.Services
{
    public class SharedItems<T> : BindableBase, ISharedItems<T>
    {
        public SharedItems()
        {
            Debug.WriteLine($"SharedItems ctor with {typeof(T).Name}");
        }

        private readonly static ObservableCollection<T> s_items = new ObservableCollection<T>();
        public virtual ObservableCollection<T> Items => s_items;

        public event EventHandler<EventArgs> ItemsRefreshed;

        public void RaiseItemsRefreshed() => ItemsRefreshed?.Invoke(this, new EventArgs());

        private static T s_selectedItem;
        private static object s_lockSelection = new object();
        public virtual T SelectedItem
        {
            get
            {
                lock (s_lockSelection)
                {
                    return s_selectedItem;
                }
            }

            set
            {
                lock (s_lockSelection)
                {
                    SetProperty(ref s_selectedItem, value);
                }
            }
        }
    }
}
