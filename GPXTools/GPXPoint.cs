using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPXTools
{
    public class GPXPoint
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public double elevation { get; set; }
        public DateTime time { get; set; }
        public int cadence { get; set; }
        public int heartRate { get; set; }
        public int power { get; set; }

        public GPXPoint()
        {

        }

        public GPXPoint(double newLat, double newLong)
        {
            this.lat = newLat;
            this.lon = newLong;
        }

        public String printString()
        {
            return "Lat : " + this.lat.ToString() + ", Long : " + this.lon.ToString();
        }
    }
}
