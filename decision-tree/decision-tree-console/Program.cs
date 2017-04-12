using decision_tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace decision_tree_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager cenas = new Manager("../../../decision-tree/Resources/strokes3.csv");
            Console.ReadLine();
        }
    }
}
