using System;
using System.ComponentModel;

namespace GenericViewModels.Services
{
    [Obsolete("Use ISharedItems instead", error: true)]
    public interface ISelectedItem<T> : INotifyPropertyChanged
    {
        T SelectedItem { get; set; }

    }
}
