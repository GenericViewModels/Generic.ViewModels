using BooksLib.Models;
using BooksLib.Services;
using BooksLib.ViewModels;
using GenericViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFBooksApp.Services;

namespace WPFBooksApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            RegisterServices();
            base.OnStartup(e);
        }
        private void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IBooksRepository, BooksSampleRepository>();
            services.AddSingleton<IItemsService<Book>, BooksService>();
            services.AddTransient<BooksViewModel>();
            services.AddTransient<BookDetailViewModel>();
            services.AddSingleton<IMessageService, WPFMessageService>();
            services.AddSingleton<INavigationService, WPFNavigationService>();
            services.AddLogging(builder =>
            {
#if DEBUG
                builder.AddDebug();
#endif
            });
            AppServices = services.BuildServiceProvider();
        }

        public IServiceProvider AppServices { get; private set; }
    }
}
