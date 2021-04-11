
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
        BitmapImage _Camera1;
        BitmapImage _Camera2;
        string checkCameras="Загрузка камер";
        string checkConection= "Установка соединения";
        VideoStream videoStream1;
        VideoStream videoStream2;
        delegate void DowloadStartSetting(string message);
        event DowloadStartSetting Cameras;
        event DowloadStartSetting Connection;
        SettingWindowVM settingWindowVM;
        bool _isEnabled=true;
        private Visibility _VisibilityLetGoImage = Visibility.Visible;
        private Visibility _VisibilityTakeImage = Visibility.Visible;
        private Visibility _VisibilityFrontImage = Visibility.Visible;
        private Visibility _VisibilityBackImage = Visibility.Visible;
        private Visibility _VisibilityCamerasGrid = Visibility.Collapsed;
        private Visibility _VisibilityMainGrid = Visibility.Visible;
        private Visibility _VisibilityFront = Visibility.Collapsed;
        private Visibility _VisibilityBack = Visibility.Collapsed;
        private Visibility _VisibilityTake = Visibility.Collapsed;
        private Visibility _VisibilityLetGo = Visibility.Collapsed;
        private bool _HelpParam = false;
        private ManipMove manip;


        #endregion
        public ICommand OpenSetting { get; }
        private bool CanOpenSettingExecute(object p) => true;
        private  void OnOpenSettingExecuted(object p)
        {
            if (manip != null)

            {
                manip.Dispose();
                manip = null;

            }
            IsEnabled = false;
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
            Cameras += MainWindowVM_Notify;
            Connection += MainWindowVM_Connection;
            try {
                string jsonString = File.ReadAllText("settingFile.json");
                StorageClassSetting storage = new StorageClassSetting();
                storage = JsonSerializer.Deserialize<StorageClassSetting>(jsonString);
                var task = Task.Run(() =>
                {
                    videoStream1 = new VideoStream(setCamera1, Convert.ToInt32(storage.camera1));
                    videoStream2 = new VideoStream(setCamera2, Convert.ToInt32(storage.camera2));
                    Cameras("Загрузка завершена");

                }
                );

                    if (manip != null)

                    {
                        manip.Dispose();
                        manip = null;

                    }

                    manip = new ManipMove(new ComConnection(storage));
                    Connection("Соединение установлено");

                

               


            }
            catch
            {
                OnOpenSettingExecuted(p);
            }
            
           
            
            
        }

        private void MainWindowVM_Connection(string message)
        {
            CheckConection = message;
        }

        private void MainWindowVM_Notify(string message)
        {
            CheckCameras = message;
        }
        #region Field for Binding
        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }
        public string CheckCameras
        {
            get => checkCameras;
            set => Set(ref checkCameras, value);
        }
        public string CheckConection
        {
            get => checkConection;
            set => Set(ref checkConection, value);
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
        public Visibility VisibilityFront
        {
            get => _VisibilityFront;
            set
            {
                Set(ref _VisibilityFront, value);
            }

        }
        public Visibility VisibilityBack
        {
            get => _VisibilityBack;
            set
            {
                Set(ref _VisibilityBack, value);
            }

        }
        public Visibility VisibilityTake
        {
            get => _VisibilityTake;
            set
            {
                Set(ref _VisibilityTake, value);
            }

        }
        public Visibility VisibilityLetGo
        {
            get => _VisibilityLetGo;
            set
            {
                Set(ref _VisibilityLetGo, value);
            }

        }
        public Visibility VisibilityLetGoImage
        {
            get => _VisibilityLetGoImage;
            set
            {
                Set(ref _VisibilityLetGoImage, value);
            }

        }
        public Visibility VisibilityTakeImage
        {
            get => _VisibilityTakeImage;
            set
            {
                Set(ref _VisibilityTakeImage, value);
            }

        }
        public Visibility VisibilityFrontImage
        {
            get => _VisibilityFrontImage;
            set
            {
                Set(ref _VisibilityFrontImage, value);
            }

        }
        public Visibility VisibilityBackImage
        {
            get => _VisibilityBackImage;
            set
            {
                Set(ref _VisibilityBackImage, value);
            }
        }
        public Visibility VisibilityMainGrid
        {
            get => _VisibilityMainGrid;
            set
            {
                Set(ref _VisibilityMainGrid, value);
            }

        }
        public Visibility VisibilityCamerasGrid
        {
            get => _VisibilityCamerasGrid;
            set
            {
                Set(ref _VisibilityCamerasGrid, value);
            }

        }
        #endregion
        #region Command
        public ICommand DownMove { get; }
        private bool CanDownMoveExecute(object p) => true;
        private void OnDownMoveExecuted(object p)
        {
            if (!_HelpParam) { _HelpParam = !_HelpParam;  manip.down = true; }
            else { _HelpParam = !_HelpParam;  manip.down = false; }
        }

        //движение вверх
        public ICommand UpMove { get; }
        private bool CanUpMoveExecute(object p) => true;
        private void OnUpMoveExecuted(object p)
        {
            if (!_HelpParam) { _HelpParam = !_HelpParam; manip.up = true; }
            else { _HelpParam = !_HelpParam; manip.up = false; }
        }
        //движение влево
        public ICommand LeftMove { get; }
        private bool CanLeftMoveExecute(object p) => true;
        private void OnLeftMoveExecuted(object p)
        {
            if (!_HelpParam) { _HelpParam = !_HelpParam; manip.left = true; }
            else { _HelpParam = !_HelpParam; manip.left = false; }
        }
        //движение вправо
        public ICommand RightMove { get; }
        private bool CanRightMoveExecute(object p) => true;
        private void OnRightMoveExecuted(object p)
        {
            if (!_HelpParam) { _HelpParam = !_HelpParam; manip.right = true; }
            else { _HelpParam = !_HelpParam; manip.right = false; }
        }
        //движение вперед
        public ICommand FrontMove { get; }
        private bool CanFrontMoveExecute(object p) => true;
        private void OnFrontMoveExecuted(object p)
        {
            if (!_HelpParam) { _HelpParam = !_HelpParam; manip.front = true; VisibilityFront = Visibility.Visible; VisibilityFrontImage = Visibility.Collapsed; }
            else { _HelpParam = !_HelpParam; manip.front = false; VisibilityFront = Visibility.Collapsed; VisibilityFrontImage = Visibility.Visible; }
        }
        //движение назад
        public ICommand BackMove { get; }
        private bool CanBackMoveExecute(object p) => true;
        private void OnBackMoveExecuted(object p)
        {
            if (!_HelpParam) { _HelpParam = !_HelpParam; manip.back = true; VisibilityBack = Visibility.Visible; VisibilityBackImage = Visibility.Collapsed; }
            else { _HelpParam = !_HelpParam; manip.back = false; VisibilityBack = Visibility.Collapsed; VisibilityBackImage = Visibility.Visible; }
        }
        //захватить
        public ICommand TakeMove { get; }
        private bool CanTakeMoveExecute(object p) => true;
        private void OnTakeMoveExecuted(object p)
        {
            if (!_HelpParam) { _HelpParam = !_HelpParam; manip.take = true; VisibilityTake = Visibility.Visible; VisibilityTakeImage = Visibility.Collapsed; }
            else { _HelpParam = !_HelpParam; manip.take = false; VisibilityTake = Visibility.Collapsed; VisibilityTakeImage = Visibility.Visible; }
        }
        //отпустить
        public ICommand LetGoMove { get; }
        private bool CanLetGoMoveExecute(object p) => true;
        private void OnLetGoMoveExecuted(object p)
        {
            if (!_HelpParam) { _HelpParam = !_HelpParam; manip.letGo = true; VisibilityLetGo = Visibility.Visible; VisibilityLetGoImage = Visibility.Collapsed; }
            else { _HelpParam = !_HelpParam; manip.letGo = false; VisibilityLetGo = Visibility.Collapsed; VisibilityLetGoImage = Visibility.Visible; }
        }
        //сброс
         public ICommand ResetMove { get; }
         private bool CanResetMoveExecute(object p)
         {
            return true;
         }
         private void OnResetMoveExecuted(object p)
         {
             //helpReset++;
             manip.reset = true;


         }
        public ICommand BackToControll { get; }
        private bool CanBackToControllExecute(object p)
        {
            // if (helpShowCameras == 0)
            //{ helpShowCameras++; return false; }

            // else { return true; }
            return true;
        }
        private void OnBackToControllExecuted(object p)
        {
           // helpShowCameras = 0;
           // helpReset = 0;
            VisibilityMainGrid = Visibility.Visible;
            VisibilityCamerasGrid = Visibility.Collapsed;
        }

        //об авторах открытие html
        public ICommand AboutAuthors { get; }
        private bool CanAboutAuthorsExecute(object p) => true;
        private void OnAboutAuthorsExecuted(object p)
        {
            System.Diagnostics.Process.Start("html\\index.html");

        }
        //инструкция открытие html
        public ICommand OpenManual { get; }
        private bool CanOpenManualExecute(object p) => true;
        private void OnOpenManualExecuted(object p)
        {
            System.Diagnostics.Process.Start("html\\manual.html");

        }
        // калибровка трекера сообщение
        public ICommand Сalibration { get; }
        private bool CanСalibrationExecute(object p) => true;
        private void OnСalibrationExecuted(object p)
        {
            MessageBox.Show("Для калибровки прейдите в приложение Tobii Experience");

        }
        public ICommand ShowCamera { get; }
        private bool CanShowCameraExecute(object p) => true;
        private void OnShowCameraExecuted(object p)
        {
            VisibilityMainGrid = Visibility.Collapsed; VisibilityCamerasGrid = Visibility.Visible;

        }
        #endregion
        public void restart()
        {
            IsEnabled = true;
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            displayRootRegistry.HidePresentation(settingWindowVM);
            OnLoadExecuted(null);

        }
        public MainWindowVM()
        {
            Load = new LambdaCommand(OnLoadExecuted, CanLoadExecute);
            OpenSetting = new LambdaCommand(OnOpenSettingExecuted, CanOpenSettingExecute);

            DownMove = new LambdaCommand(OnDownMoveExecuted, CanDownMoveExecute);
            UpMove = new LambdaCommand(OnUpMoveExecuted, CanUpMoveExecute);
            LeftMove = new LambdaCommand(OnLeftMoveExecuted, CanLeftMoveExecute);
            RightMove = new LambdaCommand(OnRightMoveExecuted, CanRightMoveExecute);
            FrontMove = new LambdaCommand(OnFrontMoveExecuted, CanFrontMoveExecute);
            BackMove = new LambdaCommand(OnBackMoveExecuted, CanBackMoveExecute);
            TakeMove = new LambdaCommand(OnTakeMoveExecuted, CanTakeMoveExecute);
            LetGoMove = new LambdaCommand(OnLetGoMoveExecuted, CanLetGoMoveExecute);
            ResetMove = new LambdaCommand(OnResetMoveExecuted, CanResetMoveExecute);
            ShowCamera = new LambdaCommand(OnShowCameraExecuted, CanShowCameraExecute);
            AboutAuthors = new LambdaCommand(OnAboutAuthorsExecuted, CanAboutAuthorsExecute);
            OpenManual = new LambdaCommand(OnOpenManualExecuted, CanOpenManualExecute);
            Сalibration = new LambdaCommand(OnСalibrationExecuted, CanСalibrationExecute);
            BackToControll = new LambdaCommand(OnBackToControllExecuted, CanBackToControllExecute);
        }
    }
}
