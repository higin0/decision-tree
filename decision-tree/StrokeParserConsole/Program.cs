using System;
using System.IO;
using StrokeParser;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrokeParserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\higino\Dropbox\To Joao";
            string[] folders = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
            List<int> features = new List<int>();
            for (int i = 1; i < 48; i++)
            {
                features.Add(i);
            }
            for (int i = 0; i < folders.Count(); i++)
            {
                if (folders[i] != @"C:\Users\higino\Dropbox\To Joao\05") {
                    string[] files = System.IO.Directory.GetFiles(folders[i], "*.txt");
                    Parser cenas = new Parser(files[0]);
                    var str = "Session " + folders[i];
                    str += cenas.GetResults(features);
                    string path2 = @"F:\teste\" + i + ".txt";
                    // Create a file to write to.
                    FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Write, path2);
                    f2.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, path2);
                    File.WriteAllText(path2, str);
                    Console.WriteLine("--------------------------------------------------------------------Written Session " + i);
                }
            }
            Console.ReadLine();
        }
    }
}
