using BooksLib.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace WPFBooksApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IServiceScope _scope;
        public MainWindow()
        {
            InitializeComponent();
            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            BookDetailUC.ViewModel = _scope.ServiceProvider.GetRequiredService<BookDetailViewModel>();
            ViewModel = _scope.ServiceProvider.GetRequiredService<BooksViewModel>();
            Closed += (sender, e) => _scope.Dispose();

            DataContext = this;
            
        }

        public BooksViewModel ViewModel { get; }
    }
}
