using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrokeParser
{
    public class Stroke
    {
        public Point[] Points { get; set; }

        public Stroke(Point[] inputs)
        {
            Points = inputs;
        }
    }
}
