using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrokeParser
{
    public class Point
    {
        public string[][] _point { get; set; }

        public Point(string[][] pointInfo)
        {
            _point = pointInfo;
        }
    }
}
