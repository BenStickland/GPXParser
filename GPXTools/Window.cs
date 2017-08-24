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
            //Initialise variables
            Parse parser = new Parse();
            Analysis analsyer = new Analysis();

            //Pork Hill
            //parser.fileName = @"C:\Users\Ben\Downloads\course_16727476229.gpx";

            //YTPY triangle 
            parser.fileName = @"C:\Users\Ben\Downloads\course_1705444855.gpx";

            //Parse the gpx file into an array of objects
            GPXFile parsedData = parser.parseFile();


            List<double> speeds = analsyer.getSpeedBetweenPoints(parsedData);
            List<double> elevations = analsyer.getAllElevation(parsedData);

            var seriesSpeed = new Series("Speed");
            var seriesElevation = new Series("Elevation");

            seriesSpeed.ChartType = SeriesChartType.Line;
            seriesElevation.ChartType = SeriesChartType.Line;

            for (int i = 0; i < speeds.Count; i++) { 
                seriesSpeed.Points.AddY(speeds[i]);
                seriesElevation.Points.AddY(elevations[i]);
                mainTable.Rows.Add(speeds[i].ToString());
            }

            mainChart.Series.Add(seriesSpeed);
            mainChart.Series.Add(seriesElevation);

            mainChart.Series[0].BorderWidth = 3;
            mainChart.Series[1].BorderWidth = 3;
        }
    }
}
