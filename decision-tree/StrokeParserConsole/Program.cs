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
            string path = @"C:\Users\higin\Dropbox\To Joao";
            string[] folders = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.AllDirectories);
            List<int> features = new List<int>();
            for (int i = 1; i < 48; i++)
            {
                if(i != 43 && i!= 44 && i != 45 && i !=46)
                    features.Add(i);
            }
            string path2 = @"D:\teste\output.csv";
            // Create a file to write to.
            FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Write, path2);
            f2.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, path2);
            File.WriteAllText(path2, "Duration,Dist2prev,timeElappsed,xMin,xMax,xMean,xMedian,xSTD,yMin,yMax,yMean,yMedian,ySTD,pMin,pMax,pMean,pMedian,pSTD,lengthT,spanX,spanY,distanceX,distanceY,displacement,1stDVPCx,1stDVPCy,1stDVPP,2ndDVPCx,2ndDVPCy,2ndDVPP,velocity,acceleration,wj_Mean,wj_Min,wj_Max,curlX,curlY,1stDVcurlx,1stDVcurly,angleM,angle1,angle2,Area\n");

            for (int i = 0; i < folders.Count(); i++)
            {
                if (folders[i] != @"C:\Users\higin\Dropbox\To Joao\05") {
                    string[] files = System.IO.Directory.GetFiles(folders[i], "*.txt");
                    Parser cenas = new Parser(files[0]);
                    var str = "";
                    str += cenas.GetResults(features);
                    str = cenas.ConvertToCSV(str, '\t');
                    File.AppendAllText(path2, str);
                    Console.WriteLine("--------------------------------------------------------------------Written Session " + i);
                }
            }
            Console.ReadLine();
        }
    }
}
