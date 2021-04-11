using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tobii.Interaction;
using Tobii.Interaction.Wpf;
using TobiiMVVM.ViewModels;
using TobiiMVVM.Views;

namespace TobiiMVVM
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Host _host;
        private WpfInteractorAgent _wpfInteractorAgent;
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
            _host = new Host();
            _wpfInteractorAgent = _host.InitializeWpfAgent();
            base.OnStartup(e);

            mainWindowViewModel = new MainWindowVM();

            await displayRootRegistry.ShowModalPresentation(mainWindowViewModel);

            Shutdown();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            string target_name = "TobiiManip";
            System.Diagnostics.Process[] local_procs = System.Diagnostics.Process.GetProcesses();
            try
            {
                System.Diagnostics.Process target_proc = local_procs.First(p => p.ProcessName == target_name);
                target_proc.Kill();
            }
            catch (InvalidOperationException)
            {
               // MessageBox.Show("Process " + target_name + " not found!");

            }
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
