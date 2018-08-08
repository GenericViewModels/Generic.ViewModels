using BooksLib.Models;
using BooksLib.Services;
using BooksLib.ViewModels;
using GenericViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBooksApp.Services;

namespace WPFBooksApp
{
    public class AppServices
    {
        private AppServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IBooksRepository, BooksSampleRepository>();
            services.AddTransient<IItemsService<Book>, BooksService>();
            services.AddTransient<BooksViewModel>();
            services.AddTransient<BookDetailViewModel>();
            services.AddSingleton<ISharedItems<Book>, SharedItems<Book>>();
            services.AddTransient<IShowProgressInfo, ShowProgressInfo>();
            services.AddTransient<IMessageService, WPFMessageService>();
            services.AddSingleton<INavigationService, WPFNavigationService>();
            services.AddSingleton<IEventAggregator, EventAggregator>();

            services.AddLogging(builder =>
            {
#if DEBUG
                builder.AddDebug().SetMinimumLevel(LogLevel.Trace);
#endif
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        private static AppServices _instance;
        private static object _instanceLock = new object();
        private static AppServices GetInstance()
        {
            lock (_instanceLock)
            {
                return _instance ?? (_instance = new AppServices());
            }
        }
        public static AppServices Instance => _instance ?? GetInstance();

        public IServiceProvider ServiceProvider { get; }
    }
}
