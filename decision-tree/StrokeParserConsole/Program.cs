using System;
using StrokeParser;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrokeParserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parser cenas = new Parser("../../Resources/StrokesLog1.txt");
            List<int> features = new List<int>();
            //for(int i = 1; i < 48; i++)
            //{
            //    features.Add(i);
            //}
            features.Add(40);
            Parser cenas = new Parser("../../Resources/StrokesLog1.txt", features);
            Console.ReadLine();
        }
    }
}
