using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TobiiMVVM.ViewModels;
using TobiiMVVM.Views;

namespace TobiiMVVM
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        MainWindowVM mainWindowViewModel;

        public App()
        {
            displayRootRegistry.RegisterWindowType<MainWindowVM, MainWindow>();
            // displayRootRegistry.RegisterWindowType<OtherWindowViewModel, ChildWindow>();
            //displayRootRegistry.RegisterWindowType<DialogWindowViewModel, DialogWindow>();
            displayRootRegistry.RegisterWindowType<SettingWindowVM, SettingWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            //base.OnStartup(e);

            mainWindowViewModel = new MainWindowVM();

            await displayRootRegistry.ShowModalPresentation(mainWindowViewModel);

            Shutdown();
        }
    }
}
