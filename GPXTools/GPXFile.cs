using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPXTools
{
    public class GPXFile
    {
        public String fileName { get; set; }
        public List<GPXPoint> points { get; set; }

        public GPXFile()
        {
            this.fileName = "";
            this.points = new List<GPXPoint>();
        }
    }
}
