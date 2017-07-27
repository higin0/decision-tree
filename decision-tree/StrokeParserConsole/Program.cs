using System;
using System.IO;
using StrokeParser;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Linq;

namespace StrokeParserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //string inputPath = args[0];
            //string outputPath = args[1];
            //List<int> features = args[2];
            string inputPath = @"C:\Users\higin\Dropbox\To Joao";
            string outputPath = @"D:\teste\output.csv";
            string output = "";
            List<int> features = new List<int>();
            string[] folders = System.IO.Directory.GetDirectories(inputPath, "*", System.IO.SearchOption.AllDirectories);

            FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Write, outputPath);
            f2.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, outputPath);
            File.WriteAllText(outputPath, "Session;Stroke#;Duration;Dist2prev;timeElappsed;xMin;xMax;xMean;xMedian;xSTD;yMin;yMax;yMean;yMedian;ySTD;pMin;pMax;pMean;pMedian;pSTD;lengthT;spanX;spanY;distanceX;distanceY;displacement;1stDVPCx;1stDVPCy;1stDVPP;2ndDVPCx;2ndDVPCy;2ndDVPP;velocity;acceleration;wj_Mean;wj_Min;wj_Max;curlX;curlY;1stDVcurlx;1stDVcurly;angleM;angle1;angle2;numOfStrk1;numOfStrk3;numOfStrk5;numOfStrk10;Area\n");

            for (int i = 1; i < 48; i++)
            {
                features.Add(i);
            }

            for (int i = 0; i < folders.Count(); i++)
            {
                //session 5 is not uniform
                if (folders[i] != @"C:\Users\higin\Dropbox\To Joao\05")
                {
                    string[] strokeFiles = System.IO.Directory.GetFiles(folders[i], "*.txt");
                    string[] expressionFiles = System.IO.Directory.GetFiles(folders[i], "*.csv");

                    if (strokeFiles.Count() == 1 && expressionFiles.Count() == 1)
                    {
                        Parser strokes = new Parser(strokeFiles[0]);
                        Parser emotions = new Parser(expressionFiles[0]);
                        output = strokes.GetResults(features, i);
                        output = strokes.ConvertToCSV(output, '\t');
                        File.AppendAllText(outputPath, output);
                        Console.WriteLine("--------------------------------- Session " + i + " Written -----------------------------------");
                    }
                    else if (strokeFiles.Count() > 1)
                    {
                        Console.WriteLine("Too many txt files in folder "+ i + ". Unsure which one is the data to use");
                    }
                    else if (strokeFiles.Count() < 1)
                    {
                        Console.WriteLine("No txt files were found in Folder " + i);
                    }
                    if(expressionFiles.Count() > 1)
                    {
                        Console.WriteLine("Too many CSV files in folder " + i + ". Unsure which one is the data to use");
                    }
                    else if(expressionFiles.Count() < 1)
                    {
                        Console.WriteLine("No CSV files were found in Folder " + i);
                    }
                }
            }
            Console.WriteLine("Sessions Calculated");
            Console.ReadLine();
        }
    }
}
