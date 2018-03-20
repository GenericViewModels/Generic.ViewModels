using BooksLib.Services;
using System.Threading.Tasks;
using System.Windows;

namespace WPFBooksApp.Services
{
    public class WPFMessageService : IMessageService
    {
        public Task ShowMessageAsync(string message)
        {
            MessageBox.Show(message);
            return Task.CompletedTask;
        }
    }
}
