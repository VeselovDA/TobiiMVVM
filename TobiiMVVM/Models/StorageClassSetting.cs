using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobiiMVVM.Models
{
    public class StorageClassSetting
    {

        public string name { get; set; }
        public string speed { get; set; }
        public string bits { get; set; }
        public string errors { get; set; }
        public string stopBits { get; set; }
        public string camera1 { get; set; }
        public string camera2 { get; set; }

        public StorageClassSetting() { }
        public StorageClassSetting(string name, string speed, string bits, string errors, string stopBits, string camera1, string camera2)
        {
            this.name = name;
            this.speed = speed;
            this.bits = bits;
            this.errors = errors;
            this.stopBits = stopBits;
            this.camera1 = camera1;
            this.camera2 = camera2;


        }
    }
}