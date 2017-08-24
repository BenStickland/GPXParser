using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GPXTools
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();

            doAnalysis();            
        }

        /// <summary>
        /// Perform analysis to be displayed
        /// </summary>
        private void doAnalysis()
        {
            //GPX Files
            String gpxPorkHill = @"C:\Users\Ben\Downloads\course_16727476229.gpx";
            String gpxYTPYTriangle = @"C:\Users\Ben\Downloads\course_1705444855.gpx";
            String gpxBurrator = @"C:\Users\Ben\Downloads\course_25774985017.gpx";

            //Initialise the parser and parse the file
            Parse parser = new Parse();
            parser.fileName = gpxPorkHill;
            GPXFile parsedFile = parser.parseFile();

            //Initialise the analysis object
            Analysis analsyer = new Analysis(parsedFile);

            //Smooth the data
            List<double> speeds = analsyer.smoothSpeed(20);
            List<double> elevations = analsyer.smoothElevation(10);

            //Plot the speed and elevation data on a graph
            plotGraphData(speeds, elevations);

            //Plot map data
            plotMapData(analsyer.rawLatLong);
        }

        /// <summary>
        /// Plot speed and elevation data on the graph
        /// </summary>
        /// <param name="speeds">List of speed data to be shown</param>
        /// <param name="elevations">List of elevation data to be shown</param>
        private void plotGraphData(List<Double> speeds, List<Double> elevations)
        {
            //Create the series of data for the graph
            var seriesSpeed = new Series("Speed");
            var seriesElevation = new Series("Elevation");

            //Set the data to a line to get a line graph
            seriesSpeed.ChartType = SeriesChartType.Line;
            seriesElevation.ChartType = SeriesChartType.Line;

            //Add the y-axis data for the speed in order
            for (int i = 0; i < speeds.Count; i++)
            {
                seriesSpeed.Points.AddY(speeds[i]);
            }

            //Add the y-axis data for the elevation in order
            for (int i = 0; i < elevations.Count; i++)
            {
                seriesElevation.Points.AddY(elevations[i]);
            }

            //Add the data to the graph
            mainChart.Series.Add(seriesSpeed);
            mainChart.Series.Add(seriesElevation);

            //Set the width of the line graphs
            mainChart.Series[0].BorderWidth = 3;
            mainChart.Series[1].BorderWidth = 3;
        }

        /// <summary>
        /// Plot a series of lat/long data points on a map
        /// </summary>
        /// <param name="latLongData">A list of lat/long data points to be displayed</param>
        private void plotMapData(List<Double[]> latLongData)
        {
            //Create the overlay and point list
            GMapOverlay courseOverlay = new GMapOverlay("course");
            List<PointLatLng> points = new List<PointLatLng>();

            //Add all the lat/long data points
            for (int i = 0; i < latLongData.Count; i++)
            {
                points.Add(new PointLatLng(latLongData[i][0], latLongData[i][1]));
            }

            //Create and add the route to the map
            GMapRoute route = new GMapRoute(points, "Course");
            route.Stroke = new Pen(Color.Black, 2);
            courseOverlay.Routes.Add(route);
            map.Overlays.Add(courseOverlay);

            //Set the central position and provider for the map
            map.SetPositionByKeywords(latLongData[0][0].ToString() + ", " + latLongData[0][1].ToString());
            map.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
        }
    }
}
