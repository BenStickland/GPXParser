using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPXTools
{
    public class Parse
    {
        public String fileName { get; set; }

        public GPXFile parseFile()
        {
            GPXFile parsedFile = new GPXFile();
            String allData = File.ReadAllText(fileName);
            String[] elements = allData.Split('<');

            //Parse the intro details
            int i = 0;
            for(i = 0; !elements[i].StartsWith("trkseg"); i++)
            {

            }
            i++;

            int startMain = i;
            String tempString = "";
            GPXPoint tempPoint = new GPXPoint();

            //Loop through the main part of the file
            for (i = startMain; i < 10000; i++)
            {
                
                //Remove final 2 letters if it has a \n at the end
                if (elements[i].EndsWith("\\n")) { 
                    tempString = tempString + elements[i].Remove(elements[i].Length - 2).ToString();
                } else {
                    tempString = tempString + elements[i].ToString();
                }


                //Get lat/long
                if (elements[i].StartsWith("trkpt"))
                {
                    String[] latLongSplit = elements[i].Split(' ');

                    //Remove the first 6 characters ('lat=\"') and final 2 (\")
                    String lat = latLongSplit[1].Remove(0, 5);
                    lat = lat.Remove(lat.Length - 1);
                    tempPoint.lat = Convert.ToDouble(lat);

                    //Remove the first 6 characters ('lon=\"') and final 6 (\">\n")
                    String lon = latLongSplit[2].Remove(0, 5);
                    lon = lon.Remove(lon.Length - 3);
                    tempPoint.lon = Convert.ToDouble(lon);
                }

                //End the trackpoint
                if(elements[i].ToString() == "/trkpt>\n")
                {
                    tempPoint.value = tempString;

                    parsedFile.points.Add(tempPoint);

                    tempPoint = new GPXPoint();
                    tempString = "";
                }
            }


            return parsedFile;
        }
    }
}
