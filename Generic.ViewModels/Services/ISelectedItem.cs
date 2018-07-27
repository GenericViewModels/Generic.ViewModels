using System;

namespace GenericViewModels.Services
{
    public interface ISelectedItem<T>
    {
        T SelectedItem { get; set; }
        event EventHandler<T> SelectedItemChanged;
    }
}
