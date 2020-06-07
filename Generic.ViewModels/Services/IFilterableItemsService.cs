using System;

namespace GenericViewModels.Services
{
    public interface IFilterableItemsService<T> : IItemsService<T>
    {
        Func<T, bool>? Filter { get; set; }
        void ExecuteFilter();
    }
}
