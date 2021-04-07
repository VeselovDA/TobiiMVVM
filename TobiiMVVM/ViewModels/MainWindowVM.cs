using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TobiiMVVM.Models;
using TobiiMVVM.ViewModels.Base;

namespace TobiiMVVM.ViewModels
{
    class MainWindowVM : BaseVM
    {
        public ICommand OpenSetting { get; }
        private bool CanOpenSettingExecute(object p) => true;
        private async void OnOpenSettingExecuted(object p)
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            var newWindow = new SettingWindowVM();
            //displayRootRegistry.ShowPresentation(newWindow);
            await displayRootRegistry.ShowModalPresentation(newWindow);
        }
        public MainWindowVM()
        {
            OpenSetting = new LambdaCommand(OnOpenSettingExecuted, CanOpenSettingExecute);
        }
    }
}
