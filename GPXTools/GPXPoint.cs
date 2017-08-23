using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPXTools
{
    public class GPXPoint
    {
        public string value { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }

        public GPXPoint()
        {

        }

        public GPXPoint(String newValue)
        {
            this.value = newValue;
        }

        public GPXPoint(String newValue, double newLat, double newLong)
        {
            this.value = newValue;
            this.lat = newLat;
            this.lon = newLong;
        }
    }
}
