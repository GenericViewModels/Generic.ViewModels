using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GenericViewModels.Net
{
    public interface IHttpHelper<T>
    {
        Task<T> GetItemAsync(string url);
        Task<IEnumerable<T>> GetItemsAsync(string url);
        Task<T> AddItemAsync(string url, T item);
        Task UpdateItemAsync(string url, T item);
        Task<T> DeleteItemAsync(string url);
    }
}
