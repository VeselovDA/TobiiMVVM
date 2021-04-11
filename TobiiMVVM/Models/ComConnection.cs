using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TobiiMVVM.Models
{
    class ComConnection : AbstractConnection
    {
        byte[] Tx_Port_Buffer = new byte[10];


        public SerialPort serialPort { get; set; } = null;
        StorageClassSetting storage;

        public ComConnection(StorageClassSetting storage)
        {
            this.storage = storage;
            CreateComPort();
        }
        public void CreateComPort()
        {
            if (serialPort != null)
                if (serialPort.IsOpen) { serialPort.Close(); }
            Parity parity;
            StopBits stopBits;
            switch (storage.errors)
            {
                case "Odd": { parity = Parity.Odd; break; }
                case "Space": { parity = Parity.Space; break; }
                case "Even": { parity = Parity.Even; break; }
                case "Mark": { parity = Parity.Mark; break; }
                default: { parity = Parity.None; break; }
            }
            switch (storage.stopBits)
            {
                case "1": { stopBits = StopBits.One; break; }
                case "1.5": { stopBits = StopBits.OnePointFive; break; }
                case "2": { stopBits = StopBits.Two; break; }
                default: { stopBits = StopBits.None; break; }
            }
            serialPort = new SerialPort(storage.name, Convert.ToInt32(storage.speed), parity, Convert.ToInt32(storage.bits), stopBits);
            serialPort.Open();
        }

        override public void SendData(string comand)
        {
            try
            {
                if (comand == "reset")
                {
                    serialPort.Write(comandColection[comand], 0, 4);
                    Thread.Sleep(1000);
                }
                else
                {
                    serialPort.Write(comandColection[comand], 0, 4);
                    Thread.Sleep(25);
                }

            }
            catch (Exception) { }

        }

        override public void GetData(int length)
        {

            int offset = 0;
            while (offset < length)
                offset += serialPort.Read(Tx_Port_Buffer, offset, length - offset);



        }

        override public bool TestLink()
        {
            byte[] LinlOk = new byte[10]
            {
                (byte)'L',(byte)'i', (byte)'n',(byte)'k',(byte)'O',(byte)'k',0x6E,0x8E,0,0
            };
            try
            {
                serialPort.Write(comandColection["test"], 0, 10);

                Array.Clear(Tx_Port_Buffer, 0, Tx_Port_Buffer.Length);
                GetData(8);
                Thread.Sleep(100);
                if (Tx_Port_Buffer.SequenceEqual(LinlOk))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        override public void CloseConection()
        {
            if (serialPort.IsOpen)
                serialPort.Close();
        }
    }
}
