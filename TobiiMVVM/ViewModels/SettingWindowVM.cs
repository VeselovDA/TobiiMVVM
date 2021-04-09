using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        VideoStream videoStream1;
        VideoStream videoStream2;
        private int _selectIndexCam1 = -1;
        private int _selectIndexCam2 = -1;
        private BitmapImage _Camera1;
        private BitmapImage _Camera2;
        private ObservableCollection<string> _webCams = new ObservableCollection<string>();
       
    public ObservableCollection<string> WebCams
        {
            get => _webCams;
            set => Set(ref _webCams, value);
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
        public ICommand Load { get; }
        private bool CanLoadExecute(object p) => true;
        private void OnLoadExecuted(object p)
        {
            var webCamsArray = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            WebCams.Clear();
            foreach (DsDevice webcam in webCamsArray)
            {
                WebCams.Add(webcam.Name);
            }
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
        public SettingWindowVM()
        {
            Load = new LambdaCommand(OnLoadExecuted, CanLoadExecute);
            CheckCameras= new LambdaCommand(OnCheckCamerasExecuted, CanCheckCamerasExecute);


        }
    }
}
