using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPXTools
{
    public class Program
    {
        static void Main(string[] args)
        {
            Parse parser = new Parse();

            parser.fileName = @"C:\Users\Ben\Downloads\course_16727476229.gpx";
            GPXFile parsedData = parser.parseFile();

            Console.WriteLine("Some Data");
        }
    }
}
