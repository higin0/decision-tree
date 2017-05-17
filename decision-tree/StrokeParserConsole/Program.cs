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
            Parser cenas = new Parser("../../Resources/StrokesLog1.txt");

            Console.ReadLine();
        }
    }
}
