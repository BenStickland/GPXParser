# GPXParser
A simple GPX parsing tool

Current features:
 - Read in GPX file
 - Parse out the following data:
    - Lat/Long data
    - Elevation
    - Time
    - Cadence
    - Heart Rate
    - Power
- Smooth data (elevation and speed) using a moving average 
- Display the gpx file on a map

Planned features : 
- Smarter smoothing (Ignoring anomalies)
- Parsing for various formats
- More smoothing (HR, cadence etc)
- More accurate speed readings (Overcome gps inaccuracies somehow)
