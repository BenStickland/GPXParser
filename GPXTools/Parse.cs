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
            GPXPoint tempPoint = new GPXPoint();

            //Loop through the main part of the file
            for (i = startMain; i < elements.Length; i++)
            {
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
                else if (elements[i].StartsWith("ele>"))
                {
                    String elevation = elements[i].Remove(0, 4);

                    try
                    {
                        tempPoint.elevation = Convert.ToDouble(elevation);
                    }
                    catch 
                    {
                        Console.WriteLine("Error converting to float - Line No : " + i.ToString());
                    }
                }
                else if (elements[i].StartsWith("time>"))
                {
                    String time = elements[i].Remove(0, 5);

                    try
                    {
                        tempPoint.time = Convert.ToDateTime(time);
                    }
                    catch
                    {
                        Console.WriteLine("Error converting time to DateTime object - Line No : " + i.ToString());
                    }
                }
                else if (elements[i].StartsWith("power>"))
                {
                    String power = elements[i].Remove(0, 6);

                    try
                    {
                        tempPoint.power = Convert.ToInt32(power);
                    }
                    catch
                    {
                        Console.WriteLine("Error converting power to int - Line No : " + i.ToString());
                    }
                }
                else if (elements[i].StartsWith("gpxtpx:hr>"))
                {
                    String heartRate = elements[i].Remove(0, 10);

                    try
                    {
                        tempPoint.heartRate = Convert.ToInt32(heartRate);
                    }
                    catch
                    {
                        Console.WriteLine("Error converting heart rate to int - Line No : " + i.ToString());
                    }
                }
                else if (elements[i].StartsWith("gpxtpx:cad>"))
                {
                    String cadence = elements[i].Remove(0, 11);

                    try
                    {
                        tempPoint.cadence = Convert.ToInt32(cadence);
                    }
                    catch
                    {
                        Console.WriteLine("Error converting cadence to int - Line No : " + i.ToString());
                    }
                }
                else if (elements[i].ToString() == "/trkpt>\n")
                {
                    parsedFile.points.Add(tempPoint);

                    tempPoint = new GPXPoint();
                }
            }
            
            return parsedFile;
        }
    }
}
