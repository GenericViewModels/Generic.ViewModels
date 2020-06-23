using BooksApp.ViewModels;
using BooksLib.Events;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BooksApp
{
    public sealed partial class MainPage : Page
    {
        private IServiceScope _scope;
        private IEventAggregator _eventAggregator;

        public MainPage()
        {
            InitializeComponent();
            _scope = AppServices.Instance.ServiceProvider.CreateScope();

            Unloaded += (sender, e) => _scope.Dispose();

            ViewModel = _scope.ServiceProvider.GetRequiredService<MainPageViewModel>();
            _eventAggregator = _scope.ServiceProvider.GetRequiredService<IEventAggregator>();
            ViewModel.SetNavigationFrame(ContentFrame);
        }

        public MainPageViewModel ViewModel { get; }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // width > 1024 - no navigation, use navigation with smaller windows
            _eventAggregator.GetEvent<UseNavigationEvent>().Publish(e.NewSize.Width < 1024);
        }
    }
}
