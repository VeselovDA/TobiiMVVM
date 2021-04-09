using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
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
    class SettingWindowVM :BaseVM
    {
        MainWindowVM parentWindowVM;
        VideoStream videoStream1;
        VideoStream videoStream2;
        int _selectIndexCam1 = -1;
        int _selectIndexCam2 = -1;
        BitmapImage _Camera1;
        BitmapImage _Camera2;
        ObservableCollection<string> _webCams = new ObservableCollection<string>();
        ObservableCollection<string> _comPorts = new ObservableCollection<string>();
        string _selectItemComNum;
        string _selectItemComSpeed;
        string _selectItemComBit;
        string _selectItemComErrors;
        string _selectItemComStopBit;
        delegate void CloseSettingWindow();
        event CloseSettingWindow closeWindow;
        bool AcceptSettingClose;

        public ObservableCollection<string> WebCams
        {
            get => _webCams;
            set => Set(ref _webCams, value);
        }
        public ObservableCollection<string> ComPorts
        {
            get => _comPorts;
            set => Set(ref _comPorts, value);
        }
        public int SelectIndexCam1
        {
            get => _selectIndexCam1;
            set => Set(ref _selectIndexCam1, value);
        }
        public int SelectIndexCam2
        {
            get => _selectIndexCam2;
            set => Set(ref _selectIndexCam2, value);
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
        public string SelectItemComNum
        {
            get => _selectItemComNum;
            set => Set(ref _selectItemComNum, value);
        }
        public string SelectItemComSpeed
        {
            get => _selectItemComSpeed;
            set => Set(ref _selectItemComSpeed, value);
        }
        public string SelectItemComBit
        {
            get => _selectItemComBit;
            set => Set(ref _selectItemComBit, value);
        }
        public string SelectItemComErrors
        {
            get => _selectItemComErrors;
            set => Set(ref _selectItemComErrors, value);
        }
        public string SelectItemComStopBit
        {
            get => _selectItemComStopBit;
            set => Set(ref _selectItemComStopBit, value);
        }
        public ICommand Load { get; }
        private bool CanLoadExecute(object p) => true;
        private void OnLoadExecuted(object p)
        {
            AcceptSettingClose = false;
            closeWindow +=parentWindowVM.restart;
            var webCamsArray = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            WebCams.Clear();
            foreach (DsDevice webcam in webCamsArray)
            {
                _webCams.Add(webcam.Name);
            }
            string[] listCOM = SerialPort.GetPortNames();
            foreach (string port in listCOM)
                _comPorts.Add(port);

        }
        public ICommand Close { get; }
        private bool CanCloseExecute(object p) => !AcceptSettingClose;
        private void OnCloseExecuted(object p)
        {
            Process.GetCurrentProcess().CloseMainWindow();
        }
        public ICommand CheckCameras { get; }
        private bool CanCheckCamerasExecute(object p) => true;
        private void OnCheckCamerasExecuted(object p)
        {
            if(videoStream1!=null&& videoStream2 != null)
            {
                videoStream1.Dispose();
                videoStream2.Dispose();
            }

            if(SelectIndexCam1!=SelectIndexCam2)
            {
                 videoStream1 = new VideoStream(setCamera1, SelectIndexCam1);
                 videoStream2 = new VideoStream(setCamera2, SelectIndexCam2);
            }
            else
            {
                MessageBox.Show("Выберите разные устройства");
            }
        }
        public ICommand AcceptSetting { get; }
        private bool CanAcceptSettingExecute(object p) => true;
        private void OnAcceptSettingExecuted(object p)
        {
            if (videoStream1 == null && videoStream2 == null)
                MessageBox.Show("Одна или несколько камер не выбраны");
            SerialisationJSON();
            AcceptSettingClose = true;
            closeWindow();
            



        }
        void SerialisationJSON()
        {
            StorageClassSetting storage = new StorageClassSetting(SelectItemComNum, SelectItemComSpeed, SelectItemComBit, SelectItemComErrors, SelectItemComStopBit, SelectIndexCam1.ToString(), SelectIndexCam2.ToString());
            string jsonString;
            jsonString = JsonSerializer.Serialize(storage);
            File.WriteAllText("settingFile.json", jsonString);
        }
        public SettingWindowVM(MainWindowVM parentWindowVM)
        {
            this.parentWindowVM = parentWindowVM;
            Load = new LambdaCommand(OnLoadExecuted, CanLoadExecute);
            Close = new LambdaCommand(OnCloseExecuted, CanCloseExecute);
            CheckCameras= new LambdaCommand(OnCheckCamerasExecuted, CanCheckCamerasExecute);
            AcceptSetting = new LambdaCommand(OnAcceptSettingExecuted, CanAcceptSettingExecute);



        }
    }
}
