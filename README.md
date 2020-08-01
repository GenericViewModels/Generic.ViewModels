# Generic.ViewModels

The library **Generic.ViewModels** offers core MVVM functionality and adds generic view-models that are typically used showing lists of items and editing items.

Add this link to your NuGet package feeds to use early versions of this library: https://pkgs.dev.azure.com/cninnovation/GenericViewModelsFeed/_packaging/GenericViewModels/nuget/v3/index.json

## Getting Started

Please read the [documentation with samples](https://github.com/GenericViewModels/Generic.ViewModels-Documentation) for information about using the library.

## Principles

* Visual Studio extensions are not needed. You can use the normal Visual Studio templates to create UWP, WinUI, WPF, Platform Uno, and Xamarin applications.
* The library is based on .NET Standard 2.0. Older technologies are not supported.
* New C# 8 syntax - and as soon it's released, C# 9 will be used.

## Libraries used

* [Microsoft.Extensions.Logging](https://github.com/aspnet/Logging) - Generic.ViewModels uses *ILogger* and *IloggerFactory* - the same logging facility that's used with .NET Core.
* [Microsoft.Extensions.DependencyInjection](https://github.com/aspnet/DependencyInjection) - The client samples make use of *Microsoft.Extensions.DependencyInjection*, the DI container used by ASP.NET Core and EF Core.

## Features

### Services

### View-Models


