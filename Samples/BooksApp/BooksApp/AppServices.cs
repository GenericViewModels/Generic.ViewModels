using BooksApp.Services;
using BooksApp.ViewModels;
using BooksLib.Models;
using BooksLib.Services;
using BooksLib.ViewModels;
using GenericViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prism.Events;
using System;

namespace BooksApp
{
    public class AppServices
    {
        private AppServices()
        {
            var services = new ServiceCollection();

            // view-models
            services.AddTransient<BooksViewModel>();
            services.AddTransient<BookDetailViewModel>();
            services.AddTransient<MainPageViewModel>();

            // services
            services.AddTransient<IItemsService<Book>, BooksService>();
            services.AddTransient<IShowProgressInfo, ShowProgressInfo>();
            services.AddTransient<IMessageService, UWPMessageService>();
            services.AddSingleton<INavigationService, UWPNavigationService>();

            // stateful services
            services.AddSingleton<ISharedItems<Book>, SharedItems<Book>>();
            services.AddSingleton<IBooksRepository, BooksSampleRepository>();
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddSingleton<UWPInitializeNavigationService>();

            // logging configuration
            services.AddLogging(builder =>
            {
#if DEBUG
                builder.AddDebug().SetMinimumLevel(LogLevel.Trace);
#endif
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        public IServiceProvider ServiceProvider { get; }

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
    }
}
