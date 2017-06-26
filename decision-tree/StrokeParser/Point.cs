using System;
using System.Globalization;

namespace StrokeParser
{
    public class Point
    {

        private double _timeStamp;
        public double TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        private int _xPos;
        public int XPos
        {
            get { return _xPos; }
            set { _xPos = value; }
        }

        private int _yPos;
        public int YPos
        {
            get { return _yPos; }
            set { _yPos = value; }
        }

        private int _pressure;
        public int Pressure
        {
            get { return _pressure; }
            set { _pressure = value; }
        }

        private int _width;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public Point(string timeStamp, string x, string y, string pressure, string width)
        {
            TimeStamp = Math.Round(double.Parse(timeStamp, CultureInfo.InvariantCulture.NumberFormat),6);
            XPos = int.Parse(x, System.Globalization.NumberStyles.HexNumber);
            YPos = int.Parse(y, System.Globalization.NumberStyles.HexNumber);
            Pressure = int.Parse(pressure, System.Globalization.NumberStyles.HexNumber);
            Width = int.Parse(width, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
