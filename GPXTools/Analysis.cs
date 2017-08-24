using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPXTools
{
    public class Analysis
    {
        private static int CIRCUMFERENCE_OF_EARTH_MILES = 24901;

        public List<double> getDistanceBetweenPoints(GPXFile file)
        {
            List<double> result = new List<double>();

            //Find the distance between each lat and long point
            for(int i = 1; i < file.points.Count; i++)
            {
                //Find the difference in latitude and longitude
                double latDiff = file.points[i].lat - file.points[i - 1].lat;
                double lonDiff = file.points[i].lon - file.points[i - 1].lon;

                //Do Pythagorus on the lat/long difference and then convert into miles
                double distanceSquared = Math.Pow(latDiff, 2.0) + Math.Pow(lonDiff, 2.0);
                double distanceDegrees = Math.Sqrt(distanceSquared);
                double distance = distanceDegrees * (CIRCUMFERENCE_OF_EARTH_MILES / 360);

                result.Add(distance);
            }

            return result;
        }

        public List<double> getSpeedBetweenPoints(GPXFile file)
        {
            List<double> distances = getDistanceBetweenPoints(file);
            List<double> result = new List<double>();

            //Find the time difference between each set of points
            for (int i = 1; i < file.points.Count; i++)
            {
                TimeSpan timeDifference = file.points[i].time.Subtract(file.points[i - 1].time);

                double speed = distances[i - 1] / timeDifference.TotalHours;

                result.Add(speed);
            }

            return result;
        }

        public List<double> getAllElevation(GPXFile file)
        {
            List<double> elevations = new List<double>();

            //Find the time difference between each set of points
            for (int i = 0; i < file.points.Count; i++)
            {
                elevations.Add(file.points[i].elevation / 7);
            }

            return elevations;
        }
    }
}
