using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransferSystem
{
    public class Lynx
    {
        public int id { get; set; }
        public double xSendFirst { get; set; }
        public double ySendFirst { get; set; }
        public double xRecFirst { get; set; }
        public double yRecFirst { get; set; }
        public double distance { get; set; }
        public double heading { get; set; }
        public double tagX { get; set; }
        public double tagY { get; set; }

        public Lynx(int id)
        {
            this.id = id;
            this.distance = 18*2.22 ;
        }
    }
}
