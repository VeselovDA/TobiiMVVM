using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobiiMVVM.Models
{
    class ManipMove : IDisposable
    {
       
        public bool up { get; set; }
        public bool down { get; set; }
        public bool left { get; set; }
        public bool right { get; set; }
        public bool front { get; set; }
        public bool back { get; set; }
        public bool take { get; set; }
        public bool letGo { get; set; }
        public bool reset { get; set; }
        public bool testLink { get; set; }

        AbstractConnection Conection;
        Task task;
        public ManipMove(AbstractConnection Conection)
        {

            this.Conection = Conection;
            if (Conection.TestLink())
                task = Task.Run(() => Move());
            else
                throw new Exception("no connection");






        }
        void Move()
        {
            while (true)
            {
                try
                {
                    if (Conection != null)
                        if (up)
                        {
                            Conection.SendData("up");
                        }

                    if (down)
                    {
                        Conection.SendData("down");
                    }
                    if (left)
                    {
                        Conection.SendData("left");

                    }
                    if (right)
                    {
                        Conection.SendData("right");

                    }
                    if (take)
                    {
                        Conection.SendData("take");
                    }
                    if (letGo)
                    {
                        Conection.SendData("letGo");
                    }
                    if (front)
                    {
                        Conection.SendData("front");

                    }
                    if (back)
                    {
                        Conection.SendData("back");

                    }
                    if (reset)
                    {
                        Conection.SendData("reset");

                        reset = false;
                    }
                }
                catch (Exception) { /*MessageBox.Show("Порт был отключен. Проверьте настройки"); */}
            }
        }

        public void Dispose()
        {
            Conection.CloseConection();
            // task.Sto

        }
    }
}
