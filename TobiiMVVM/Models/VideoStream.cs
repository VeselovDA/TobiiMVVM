using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows;

namespace TobiiMVVM.Models
{
   
    class VideoStream: IDisposable
    {
        VideoCapture videoCapture;
        Action<BitmapImage> toSetterCamera;
        
        public  VideoStream  (Action<BitmapImage> toSetterCamera,int deviseCameraNum)
        {
            this.toSetterCamera = toSetterCamera;
            if (deviseCameraNum != -1)
            {
                videoCapture = new VideoCapture(deviseCameraNum);
                videoCapture.ImageGrabbed += VideoCapture_ImageGrabbed;
                videoCapture.Start();
            }
            else
                MessageBox.Show("Камера не выбрана ");




        }
        private void VideoCapture_ImageGrabbed(object sender, EventArgs e)
        {

            Mat mat = new Mat();
            videoCapture.Retrieve(mat);
            BitmapImage bi = ToBitmapImage(mat.ToImage<Bgr, byte>().ToBitmap());
            bi.Freeze();
            toSetterCamera(bi);
        }







        BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        public void Dispose()
        {
            if (videoCapture != null)
            {
                videoCapture.Stop();
                videoCapture.Dispose();
            }
        }
    }
}
