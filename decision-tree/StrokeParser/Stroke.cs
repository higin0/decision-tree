using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrokeParser
{
    public class Stroke
    {
        public Point[] _inputs { get; set; }

        public Stroke(Point[] inputs)
        {
            _inputs = inputs;
        }
    }
}
