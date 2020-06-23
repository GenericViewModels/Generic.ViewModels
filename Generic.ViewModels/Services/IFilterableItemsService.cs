using System;

namespace GenericViewModels.Services
{
    public interface IFilterableItemsService<T> : IItemsService<T>
        where T : class
    {
        Func<T, bool>? Filter { get; set; }
        void ExecuteFilter();
    }
}
