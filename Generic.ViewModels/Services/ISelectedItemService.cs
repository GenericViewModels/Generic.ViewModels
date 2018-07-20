using System;

namespace GenericViewModels.Services
{
    public interface ISelectedItemService<T>
    {
        T SelectedItem { get; set; }
        event EventHandler<T> SelectedItemChanged;
    }
}
