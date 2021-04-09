
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TobiiMVVM.Models;
using TobiiMVVM.ViewModels.Base;

namespace TobiiMVVM.ViewModels
{
    class MainWindowVM : BaseVM
    {
        #region fields
        private BitmapImage _Camera1;
        private BitmapImage _Camera2;
        private string checkCameras="Загрузка камер";
        VideoStream videoStream1;
        VideoStream videoStream2;
        delegate void DowloadStartSetting(string message);
        event DowloadStartSetting Notify;
        SettingWindowVM settingWindowVM;

        #endregion
        public ICommand OpenSetting { get; }
        private bool CanOpenSettingExecute(object p) => true;
        private  void OnOpenSettingExecuted(object p)
        {
            if(videoStream1!=null)
                videoStream1.Dispose();
            if (videoStream2 != null)
                 videoStream2.Dispose();
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            settingWindowVM = new SettingWindowVM(this);
            displayRootRegistry.ShowPresentation(settingWindowVM);
           // await displayRootRegistry.ShowModalPresentation(settingWindowVM);
            
        }
        public ICommand Load { get; }
        private bool CanLoadExecute(object p) => true;
        private void OnLoadExecuted(object p)
        {
            Notify += MainWindowVM_Notify;
            try {
                string jsonString = File.ReadAllText("settingFile.json");
                StorageClassSetting storage = new StorageClassSetting();
                storage = JsonSerializer.Deserialize<StorageClassSetting>(jsonString);
                var task = Task.Run(() =>
                {
                    videoStream1 = new VideoStream(setCamera1, Convert.ToInt32(storage.camera1));
                    videoStream2 = new VideoStream(setCamera2, Convert.ToInt32(storage.camera2));
                    Notify("Загрузка завершена");

                }
                );


            }
            catch
            {
                OnOpenSettingExecuted(p);
            }
            
           
            
            
        }

        private void MainWindowVM_Notify(string message)
        {
            CheckCameras = message;
        }
        #region Field for Binding
        public string CheckCameras
        {
            get => checkCameras;
            set => Set(ref checkCameras, value);
        }
        public BitmapImage Camera1
        {
            get => _Camera1;
            set => Set(ref _Camera1, value);
        }

        public BitmapImage Camera2
        {
            get => _Camera2;
            set => Set(ref _Camera2, value);
        }
        //упаковка ссвойства в метод для передачи в класс
        void setCamera1(BitmapImage im)
        {
            Camera1 = im;
        }
        void setCamera2(BitmapImage im)
        {
            Camera2 = im;
        }
        #endregion
        public void restart()
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            displayRootRegistry.HidePresentation(settingWindowVM);
        }
        public MainWindowVM()
        {
            Load = new LambdaCommand(OnLoadExecuted, CanLoadExecute);
            OpenSetting = new LambdaCommand(OnOpenSettingExecuted, CanOpenSettingExecute);
        }
    }
}
