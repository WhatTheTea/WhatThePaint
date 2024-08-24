using DependencyPropertyGenerator;

using Microsoft.UI.Xaml.Controls;

using WhatTheTea.Paint.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WhatTheTea.Paint.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [DependencyProperty<MainViewModel>("ViewModel")]
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
