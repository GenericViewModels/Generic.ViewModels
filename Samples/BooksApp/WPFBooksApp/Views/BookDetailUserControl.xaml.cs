using BooksLib.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace WPFBooksApp.Views
{
    /// <summary>
    /// Interaction logic for BookDetailUserControl.xaml
    /// </summary>
    public partial class BookDetailUserControl : UserControl
    {
        public BookDetailUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public BookDetailViewModel ViewModel
        {
            get => (BookDetailViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(BookDetailViewModel), typeof(BookDetailUserControl), new PropertyMetadata(null));
    }
}
