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

        //Raw Data
        private GPXFile file { get; set; }
        public List<Double> rawDistances { get; set; }
        public List<Double> rawSpeeds { get; set; }
        public List<Double> rawElevations { get; set; }
        public List<Double[]> rawLatLong { get; set; }

        //Analysed Data
        //Speed
        private List<Double> smoothedSpeed { get; set; }
        private int speedSmoothingAmount { get; set; }

        //Speed
        private List<Double> smoothedElevation { get; set; }
        private int elevationSmoothingAmount { get; set; }


        //Graph Settings
        private int scaleRatio { get; set; }

        /// <summary>
        /// Perform an initial analysis of the GPX data to get the basic data
        /// </summary>
        /// <param name="fileToAnalyse">File to be analysed</param>
        public Analysis(GPXFile fileToAnalyse)
        {
            //Initialise variables
            scaleRatio = 7;
            file = fileToAnalyse;

            rawDistances = new List<double>();
            rawElevations = new List<double>();
            rawSpeeds = new List<double>();
            smoothedElevation = new List<double>();
            smoothedSpeed = new List<double>();
            rawLatLong = new List<double[]>();

            elevationSmoothingAmount = 0;
            speedSmoothingAmount = 0;

            //Add the first lat/long point
            Double[] firstLatLong = { fileToAnalyse.points[0].lat, fileToAnalyse.points[0].lon };
            rawLatLong.Add(firstLatLong);

            //Find the distance between each lat and long point
            for (int i = 1; i < fileToAnalyse.points.Count; i++)
            {
                Double[] latLong = { fileToAnalyse.points[i].lat, fileToAnalyse.points[i].lon };
                rawLatLong.Add(latLong);

                #region Get distance data

                //Find the difference in latitude and longitude
                double latDiff = fileToAnalyse.points[i].lat - fileToAnalyse.points[i - 1].lat;
                double lonDiff = fileToAnalyse.points[i].lon - fileToAnalyse.points[i - 1].lon;

                //Do Pythagorus on the lat/long difference and then convert into miles
                double distanceSquared = Math.Pow(latDiff, 2.0) + Math.Pow(lonDiff, 2.0);
                double distanceDegrees = Math.Sqrt(distanceSquared);
                double distance = distanceDegrees * (CIRCUMFERENCE_OF_EARTH_MILES / 360);

                //Add the distance to the array
                rawDistances.Add(distance);

                #endregion

                #region Get speed data

                //Find the time difference between the current and previous points
                TimeSpan timeDifference = fileToAnalyse.points[i].time.Subtract(fileToAnalyse.points[i - 1].time);

                //Calculate the speed
                double speed = distance / timeDifference.TotalHours;

                //Add it to the array
                rawSpeeds.Add(speed);

                #endregion

                #region Get elevation data

                double elevation = fileToAnalyse.points[i].elevation;

                //Add to the array
                rawElevations.Add(elevation);

                #endregion
            }
        }

        public List<Double> smoothSpeed(int smoothingAmount)
        {
            //If the smoothedSpeed data has been generated before, return it
            if (speedSmoothingAmount == smoothingAmount)
            {
                return smoothedSpeed;
            }
            else
            {
                //Set analysis properties
                speedSmoothingAmount = smoothingAmount;

                int i;
                double currentSpeedTotal = 0.0;

                //Get the total for the first values before averaging them
                for (i = 0; i < smoothingAmount && i < rawSpeeds.Count; i++)
                {
                    currentSpeedTotal = currentSpeedTotal + rawSpeeds[i];
                }

                //Average all the rawSpeed data
                for (i = smoothingAmount; i < rawSpeeds.Count; i++)
                {
                    //Add the next speed value
                    currentSpeedTotal = currentSpeedTotal + rawSpeeds[i];

                    //Remove the first speed value outside of the smoothing range
                    currentSpeedTotal = currentSpeedTotal - rawSpeeds[i - smoothingAmount];

                    smoothedSpeed.Add(currentSpeedTotal / smoothingAmount);
                }

                return smoothedSpeed;
            }
        }

        public List<Double> smoothElevation(int smoothingAmount)
        {
            //If the smoothedSpeed data has been generated before, return it
            if (elevationSmoothingAmount == smoothingAmount)
            {
                return smoothedElevation;
            }
            else
            {
                //Set analysis properties
                elevationSmoothingAmount = smoothingAmount;

                int i;
                double currentElevationTotal = 0.0;

                //Get the total for the first values before averaging them
                for (i = 0; i < smoothingAmount && i < rawElevations.Count; i++)
                {
                    currentElevationTotal = currentElevationTotal + rawElevations[i];
                }

                //Average all the rawSpeed data
                for (i = smoothingAmount; i < rawElevations.Count; i++)
                {
                    //Add the next speed value
                    currentElevationTotal = currentElevationTotal + rawElevations[i];

                    //Remove the first speed value outside of the smoothing range
                    currentElevationTotal = currentElevationTotal - rawElevations[i - smoothingAmount];

                    smoothedElevation.Add(currentElevationTotal / (smoothingAmount * scaleRatio));
                }

                return smoothedElevation;
            }
        }
    }
}
