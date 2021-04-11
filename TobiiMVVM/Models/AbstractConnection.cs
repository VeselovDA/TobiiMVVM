using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobiiMVVM.Models
{
    public abstract class AbstractConnection
    {
        protected Dictionary<string, byte[]> comandColection = new Dictionary<string, byte[]>()
        {
            { "up",new byte[10]{ (byte)'U', 0x33,0x75,0xEA,0,0,0,0,0,0 } },//
            { "down",new byte[10]{ (byte)'D', 0x77, 0x77, 0xD2, 0,0,0,0,0,0 } },//
            { "right",new byte[10]{ (byte)'R', 0x55, 0x82, 0x7F, 0,0,0,0,0,0 } },//
            { "left",new byte[10]{ (byte)'L', 0xAA, 0x0E, 0x41, 0,0,0,0,0,0 } },//
            { "take",new byte[10]{ (byte)'H', 0x44, 0x2A, 0x91, 0,0,0,0,0,0 } },//
            { "letGo",new byte[10]{ (byte)'O', 0x88, 0x7D, 0x10, 0,0,0,0,0,0 } },//
            { "front",new byte[10]{ (byte)'F', 0x22, 0x45, 0xBE, 0,0,0,0,0,0 } },//
            { "back",new byte[10]{ (byte)'B', 0x99, 0x31, 0x64, 0,0,0,0,0,0 } },//
            { "reset",new byte[10]{ (byte)'S', 0xFF, 0x13, 0x58, 0,0,0,0,0,0 } },//
            { "test",new byte[10]{ (byte)'T', (byte)'e', (byte)'s', (byte)'t', (byte)'L', (byte)'i', (byte)'n', (byte)'k', 0xCA, 0xF1} }//
        };
        public abstract void SendData(string comand);
        public abstract void GetData(int lengch);
        public abstract bool TestLink();
        public abstract void CloseConection();

    }
}
