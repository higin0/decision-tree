using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrokeParser
{
    public class Stroke
    {
        private List<Point> _points;
        public List<Point> Points
        {
            get { return _points; }
            set { _points = value; }
        }

        public Stroke(List<Point> points)
        {
            List<Point> newpoints = new List<Point>(points);
            Points = newpoints;
        }
    }
}
