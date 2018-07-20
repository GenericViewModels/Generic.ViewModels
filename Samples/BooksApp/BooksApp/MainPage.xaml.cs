﻿using System;
using BooksApp.ViewModels;
using BooksLib.Events;
using GenericViewModels.Services;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BooksApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModel = (Application.Current as App).AppServices.GetService<MainPageViewModel>();
            ViewModel.SetNavigationFrame(ContentFrame);
        }

        public MainPageViewModel ViewModel { get; }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            EventAggregator<NavigationInfoEvent>.Instance.Publish(this, new NavigationInfoEvent { UseNavigation = e.NewSize.Width < 1024 });
        }
    }
}
