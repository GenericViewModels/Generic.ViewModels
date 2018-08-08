using BooksApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BooksApp
{
    public sealed partial class MainPage : Page
    {
        private IServiceScope _scope;
        public MainPage()
        {
            InitializeComponent();
            _scope = AppServices.Instance.ServiceProvider.CreateScope();

            Unloaded += (sender, e) => _scope.Dispose();

            ViewModel = _scope.ServiceProvider.GetRequiredService<MainPageViewModel>();
            ViewModel.SetNavigationFrame(ContentFrame);
        }

        public MainPageViewModel ViewModel { get; }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {

            // TODO: IEventAggregator
           // EventAggregator<NavigationInfoEvent>.Instance.Publish(this, new NavigationInfoEvent { UseNavigation = e.NewSize.Width < 1024 });
        }
    }
}
