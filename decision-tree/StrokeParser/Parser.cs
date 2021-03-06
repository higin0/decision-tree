﻿using System;
using System.Collections.Generic;
using System.IO;
using Accord.Math;
using System.Linq;
using System.Text.RegularExpressions;

namespace StrokeParser
{
    public class Parser
    {
        private List<Stroke> strokes = new List<Stroke>();
        private string[][] emotionDetections;

        public Parser(string strokeFile, string emotionFile)
        {
            string[][] strokeData = ReadStrokeFile(strokeFile);
            string[][] emotionData = ReadEmotionFile(emotionFile);

            strokes = GetStrokes(strokeData);
            emotionDetections = ConvertTime(emotionData);
        }

        public Parser(string strokeFile)
        {
            string[][] strokeData = ReadStrokeFile(strokeFile);

            strokes = GetStrokes(strokeData);
            emotionDetections = null;
        }


        public string[][] ReadEmotionFile(string filePath)
        {
            string[][] parsedDataSet = null;
            try
            {
                var file = File.ReadAllText(filePath);
                file = file.Replace("FIND_FAILED", "0");
                file = file.Replace("FIT_FAILED", "0");
                var dataSet = file.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (filePath.Substring(filePath.Length - 3) == "txt")
                {
                    file = file.Replace("\t", ";");
                }
                List<string[]> clean = new List<string[]>();
                parsedDataSet = dataSet.Apply(x => x.Split(';'));
                return parsedDataSet;
            }
            catch (IOException e)
            {
                Console.WriteLine("{0}: The read operation could not be performed because the specified part of the file is locked.", e.GetType().Name);
            }
            return null;
        }

        public string[][] ReadStrokeFile(string filePath)
        {
            var file = File.ReadAllText(filePath);
            var dataSet = file.Split(new[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string[]> clean = new List<string[]>();
            int dataStart = 0;

            for (int i = 0; i < dataSet.Length(); i++)
            {
                dataSet[i] = dataSet[i].Replace("[", "").Replace("]", "");
                dataSet[i] = dataSet[i].Trim();
                dataSet[i] = Regex.Replace(dataSet[i], @"\s+", ",");
            }
            var parsedDataSet = dataSet.Apply(x => x.Split(','));

            for (int x = 0; x < parsedDataSet.Count(); x++)
            {
                if (parsedDataSet[x].Count() == 5 && parsedDataSet[x][4] == "DOWN")
                {
                    dataStart = x;
                    break;
                }
            }

            for (int j = dataStart; j < parsedDataSet.GetLength(0); j++)
            {
                clean.Add(parsedDataSet[j]);
            }
            return clean.ToArray();
        }

        private string[][] ConvertTime(String[][] emotionData)
        {
            double initialTime = 0;
            for(int i = 1; i < emotionData.GetLength(0); i++)
            {
                if (emotionData[i][0].Count() > 0)
                {
                    string time = emotionData[i][0];
                    string[] splitTime = time.Split(':');
                    double minutes = Double.Parse(splitTime[0]);
                    double seconds = Double.Parse(splitTime[1]);
                    double timeNum = minutes * 60 + seconds;
                    if (i == 1)
                    {
                        initialTime = timeNum;
                    }
                    emotionData[i][0] = Convert.ToString(timeNum);
                }
            }
            for (int j = 1; j < emotionData.GetLength(0); j++)
            {
                emotionData[j][0] = Convert.ToString(Math.Round(Double.Parse(emotionData[j][0]) - initialTime, 1));
            }
            return emotionData;
        }

        public List<Stroke> GetStrokes(string[][] strokeData)
        {
            List<Stroke> strokes = new List<Stroke>();
            List<Point> points = new List<Point>();
            string posX = "", posY = "", touch = "", width = "";

            for (int i = 0; i < strokeData.GetLength(0); i++)
            {
                if (strokeData[i][3] == "BTN_TOUCH")
                {
                    i++;
                }
                if (strokeData[i][3] == "ABS_MT_POSITION_X")
                {
                    posX = strokeData[i][4];
                }
                if (strokeData[i][3] == "ABS_MT_POSITION_Y")
                {
                    posY = strokeData[i][4];
                }
                if (strokeData[i][3] == "ABS_MT_TOUCH_MAJOR")
                {
                    touch = strokeData[i][4];
                }
                if (strokeData[i][3] == "ABS_MT_WIDTH_MAJOR")
                {
                    width = strokeData[i][4];
                }
                if (strokeData[i][3] == "SYN_REPORT" && strokeData[i - 1][4] != "UP")
                {
                    Point point = new Point(strokeData[i][0], posX, posY, touch, width);
                    points.Add(point);
                    posX = "";
                    posY = "";
                    touch = "";
                    width = "";
                }
                if (strokeData[i][3] == "SYN_REPORT" && strokeData[i - 1][4] == "UP")
                {
                    i += 1;
                    Stroke s = new Stroke(points);
                    strokes.Add(s);
                    points.Clear();
                }
            }
            return strokes;
        }

        public string ConvertToCSV(string data, char oldSeparator)
        {
            return data.Replace(',','.').Replace(oldSeparator, ';').Replace('.',',');
        }

        public string GetResults(List<int> featureNums, int session)
        {
            string result = "";
            //result += "Session " + session;
            double initTime = 0;
            initTime = strokes[0].Points[0].TimeStamp;
            for (int i = 0; i < strokes.Count(); i++)
            {
                //result += "\tStroke " + i + "\t";
                for (int f = 0; f < featureNums.Count(); f++)
                {
                    switch (featureNums[f])
                    {
                        case 1:
                            result += duration(strokes[i]) + "\t";
                            break;
                        case 2:
                            if (i > 0)
                            {
                                result +=  dist2prev(strokes[i - 1], strokes[i]) + "\t";
                            }
                            if (i == 0)
                                result += "0\t";
                            break;
                        case 3:
                            if (i > 0)
                            {
                                result += timeElapsed(strokes[i], initTime) + "\t";
                            }
                            if (i == 0)
                            {
                                result += "0\t";
                            }
                            break;
                        case 4:
                            result += xMin(strokes[i]) + "\t";
                            break;
                        case 5:
                            result += xMax(strokes[i]) + "\t";
                            break;
                        case 6:
                            result += xMean(strokes[i]) + "\t";
                            break;
                        case 7:
                            result += xMedian(strokes[i]) + "\t";
                            break;
                        case 8:
                            result += xStd(strokes[i]) + "\t";
                            break;
                        case 9:
                            result += yMin(strokes[i]) + "\t";
                            break;
                        case 10:
                            result += yMax(strokes[i]) + "\t";
                            break;
                        case 11:
                            result += yMean(strokes[i]) + "\t";
                            break;
                        case 12:
                            result += yMedian(strokes[i]) + "\t";
                            break;
                        case 13:
                            result += yStd(strokes[i]) + "\t";
                            break;
                        case 14:
                            result += pMin(strokes[i]) + "\t";
                            break;
                        case 15:
                            result += pMax(strokes[i]) + "\t";
                            break;
                        case 16:
                            result += pMean(strokes[i]) + "\t";
                            break;
                        case 17:
                            result += pMedian(strokes[i]) + "\t";
                            break;
                        case 18:
                            result += pStd(strokes[i]) + "\t";
                            break;
                        case 19:
                            result += lengthT(strokes[i]) + "\t";
                            break;
                        case 20:
                            result += spanX(strokes[i]) + "\t";
                            break;
                        case 21:
                            result += spanY(strokes[i]) + "\t";
                            break;
                        case 22:
                            result += distanceX(strokes[i]) + "\t";
                            break;
                        case 23:
                            result += distanceY(strokes[i]) + "\t";
                            break;
                        case 24:
                            result += displacement(strokes[i]) + "\t";
                            break;
                        case 25:
                            result += firstDVPCx(strokes[i]) + "\t";
                            break;
                        case 26:
                            result += firstDVPCy(strokes[i]) + "\t";
                            break;
                        case 27:
                            result += firstDVPP(strokes[i]) + "\t";
                            break;
                        case 28:
                            result += secondDVPCx(strokes[i]) + "\t";
                            break;
                        case 29:
                            result += secondDVPCy(strokes[i]) + "\t";
                            break;
                        case 30:
                            result += secondDVPP(strokes[i]) + "\t";
                            break;
                        case 31:
                            result += velocity(strokes[i]) + "\t";
                            break;
                        case 32:
                            result += acceleration(strokes[i]) + "\t";
                            break;
                        case 33:
                            result += wj(strokes[i], "mean") + "\t";
                            break;
                        case 34:
                            result += wj(strokes[i], "min") + "\t";
                            break;
                        case 35:
                            result += wj(strokes[i], "max") + "\t";
                            break;
                        case 36:
                            result += "0\t";
                            //result += "curlX\t" + curlX(strokes[i]) + "\t";
                            break;
                        case 37:
                            result += "0\t";
                            //result += "curlY\t" + curlY(strokes[i]) + "\t";
                            break;
                        case 38:
                            result += "0\t";
                            //result += "1stDVCurlx\t" + firstDVCurlx(strokes[i]) + "\t";
                            break;
                        case 39:
                            result += "0\t";
                            //result += "1stDVCurly\t" + firstDVCurly(strokes[i]) + "\t";
                            break;
                        case 40:
                            result += angleM(strokes[i]) + "\t";
                            break;
                        case 41:
                            result += angle1(strokes[i]) + "\t";
                            break;
                        case 42:
                            result += angle2(strokes[i]) + "\t";
                            break;
                        case 43:
                            result += numOfStrk(strokes, i, 1) + "\t";
                            break;
                        case 44:
                            result += numOfStrk(strokes, i, 3) + "\t";
                            break;
                        case 45:
                            result += numOfStrk(strokes, i, 5) + "\t";
                            break;
                        case 46:
                            result += numOfStrk(strokes, i, 10) + "\t";
                            break;
                        case 47:
                            result += area(strokes[i]) + "\t";
                            break;
                        case 48:
                            if (emotionDetections != null)
                            {
                                var va = getValenceData(strokes[i], strokes, emotionDetections);
                                if (va == "" || va == " ")
                                    result += "0" + "\t";
                                else
                                    result += va + "\t";
                                break;
                            }
                            else
                                result += "0" + "\t";
                            break;
                        case 49:
                            if (emotionDetections != null)
                            {
                                var ar = getArousalData(strokes[i], strokes, emotionDetections);
                                if (ar == "")
                                    result += "0" + "\t";
                                else
                                    result += ar + "\t";
                                break;
                            }
                            else
                                result += "0" + "\t";
                            break;
                        default:
                            break;
                    }
                    //Console.WriteLine("feature " + featureNums[f] + " done");
                }
                //Emotion Labeling
                if (emotionDetections != null)
                {
                    var em = getEmotionData(strokes[i], strokes, emotionDetections);
                    if (em == "")
                        result += "Unknown";
                    else
                        result += em;
                }
                else
                    result += "Unknown";
                result += "\n";
                //Console.WriteLine("stroke " + i + " done\n");

            }
            return result;
        }

        #region aux

        public double SignificantTruncate(double num, int significantDigits)
        {
            double y = Math.Pow(10, significantDigits);
            return Math.Truncate(num * y) / y;
        }

        private List<double> getXpos(Stroke stroke)
        {
            return getXpos(stroke, 0, stroke.Points.Count());
        }

        private List<double> getYpos(Stroke stroke)
        {
            return getYpos(stroke, 0, stroke.Points.Count());
        }

        private List<double> getXpos(Stroke stroke, decimal start, decimal range)
        {
            List<double> xPos = new List<double>();

            for (int i = (int) start; i < range; i++)
            {
                xPos.Add(stroke.Points[i].XPos);
            }
            return xPos;
        }

        private List<double> getYpos(Stroke stroke, decimal start, decimal range)
        {
            List<double> yPos = new List<double>();

            for (int i = (int) start; i < range; i++)
            {
                yPos.Add(stroke.Points[i].YPos);
            }
            return yPos;
        }

        private List<double> getPressures(Stroke stroke)
        {
            List<double> pressures = new List<double>();

            for (int i = 0; i < stroke.Points.Count(); i++)
            {
                pressures.Add(stroke.Points[i].Pressure);
            }
            return pressures;
        }

        private List<double> getTimestamps (Stroke stroke)
        {
            List<double> timeStamps = new List<double>();

            for (int i = 0; i < stroke.Points.Count(); i++)
            {
                timeStamps.Add(stroke.Points[i].TimeStamp);
            }
            return timeStamps;
        }

        private List<double> getTimestamps(Stroke stroke, decimal start, decimal range)
        {
            List<double> timeStamps = new List<double>();

            for (int i = (int) start; i < range; i++)
            {
                timeStamps.Add(stroke.Points[i].TimeStamp);
            }
            return timeStamps;
        }

        private string print(List<double> counts)
        {
            string result = "";
            foreach(int count in counts)
            {
                result += count + " ";
            }
            return result;
        }

        public double Variance(List<double> values)
        {
            double variance = 0;

            for (int i = 0; i < values.Count; i++)
            {
                variance += Math.Pow((values[i] - values.Average()), 2);
            }

            return variance / (values.Count-1);
        }

        public double StandardDeviation(List<double> values)
        {
            double average = values.Average();
            double variance = Variance(values);

            return values.Count == 0 ? 0 : Math.Sqrt(variance);
        }

        #endregion

        #region Time - F1, F2, F3
        //F1 - Duration = Duration of the stroke
        public double duration(Stroke stroke)
        {
            int numberOfPoints = stroke.Points.Count();
            double lastInstant = stroke.Points[numberOfPoints - 1].TimeStamp;
            double firstInstant = stroke.Points[0].TimeStamp;
            double duration = lastInstant - firstInstant;
            return Math.Round(duration, 6);
        }

        //F2 - Dist2Prev - subtracting the initial timestamp of the current stroke, from the initial timestamp of the previous stroke.
        public double dist2prev(Stroke previous, Stroke current)
        {
            double currentStrokeTime = current.Points[0].TimeStamp;
            double previousStrokeTime = previous.Points[previous.Points.Count()-1].TimeStamp;
            double duration = currentStrokeTime - previousStrokeTime;
            return Math.Round(duration,6);
        }

        //F3 - timeElapsed - time elapsed from starting
        public double timeElapsed(Stroke stroke, double initTime)
        {
            double currentStrokeTime = stroke.Points[0].TimeStamp;
            double duration = currentStrokeTime - initTime;
            return Math.Round(duration,6);
        }

        #endregion

        #region Location - F4, F5, F6, F7, F8, F9, F10, F11, F12, F13
        //F4 - xMin - Min recorded X
        public double xMin(Stroke stroke)
        {
            return getXpos(stroke).Min();
        }

        //F4 - xMax - Max recorded X
        public double xMax(Stroke stroke)
        {
            return getXpos(stroke).Max();
        }

        //F6 - xMean - for these two features, values of min, max and mean should be calculated.
        public double xMean(Stroke stroke)
        {
            return getXpos(stroke).Average();
        }

        //F7 - xMedian - the middle value of x coords (the average of the two middle ones in case of a even lenght)
        public double xMedian(Stroke stroke)
        {
            List<double> x = getXpos(stroke);
            x.Sort();
            int midPoint = x.Count()/2;

            if (x.Count % 2 == 0)
            {
                return (x[midPoint] + x[midPoint-1])/ 2;
            }
            else
                return x[midPoint];
        }

        //F8 - xStd - the standard deviation of x Coordinates 
        public double xStd(Stroke stroke)
        {
            return Math.Round(StandardDeviation(getXpos(stroke)),6);
        }

        //F9 - yMin - Min recorded Y
        public double yMin(Stroke stroke)
        {
            return getYpos(stroke).Min();
        }

        //F10 - yMax - Max recorded Y
        public double yMax(Stroke stroke)
        {
            return getYpos(stroke).Max();
        }

        //F11 - yMean - for these two features, values of min, max and mean should be calculated.
        public double yMean(Stroke stroke)
        {
            return getYpos(stroke).Average();
        }

        //F12 - yMedian - the middle value of y coords (the average of the two middle ones in case of a even lenght)
        public double yMedian(Stroke stroke)
        {
            List<double> y = getYpos(stroke);
            y.Sort();
            int midPoint = y.Count() / 2;

            if (y.Count % 2 == 0)
            {
                return (y[midPoint] + y[midPoint - 1]) / 2;
            }
            else
                return y[midPoint];
        }

        //F13 - yStd - the standard deviation of y Coordinates 
        public double yStd(Stroke stroke)
        {
            return Math.Round(StandardDeviation(getYpos(stroke)),6);
        }
        #endregion

        #region Pressure - F14, F15, F16, F17, F18
        //F14 - pMin - Min recorded Pressure
        public double pMin(Stroke stroke)
        {
            return getPressures(stroke).Min();
        }

        //F15 - pMax - Max recorded Pressure
        public double pMax(Stroke stroke)
        {
            return getPressures(stroke).Max();
        }

        //F16 - pMean
        public double pMean(Stroke stroke)
        {
            return Math.Round(getPressures(stroke).Average(),6);
        }

        //F17 - pMedian - the middle value of pressures (the average of the two middle ones in case of a even lenght)
        public double pMedian(Stroke stroke)
        {
            List<double> p = getPressures(stroke);
            p.Sort();
            int midPoint = p.Count() / 2;

            if (p.Count % 2 == 0)
            {
                return (p[midPoint] + p[midPoint - 1]) / 2;
            }
            else
                return p[midPoint];
        }

        //F18 - pStd - the standard deviation of pressures 
        public double pStd(Stroke stroke)
        {
            return Math.Round(StandardDeviation(getPressures(stroke)),6);
        }

        #endregion

        #region Positional Changes - F19, F20, F21, F22, F23, F24 
        //F19 - LenghtT - sum of pythagorean propositions
        public double lengthT(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            List<double> distances = new List<double>();
            double traveledPath = 0f;

            xPos = getXpos(stroke);
            yPos = getYpos(stroke);
            //in theory xPos.Count == yPos.Count
            for (int i = 1; i < xPos.Count(); i++)
            {
                traveledPath += (double)Math.Sqrt(Math.Pow(yPos[i] - yPos[i - 1], 2) + Math.Pow(xPos[i] - xPos[i - 1], 2));
            }
            return Math.Round(traveledPath,6);
        }

        //F20 - Relocation in X - xMax-Xmin;
        public double spanX(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> relocations = new List<double>();
            xPos = getXpos(stroke);
            return xPos.Max() - xPos.Min();
        }

        //F21 - Relocation in Y - yMax-yMin;
        public double spanY(Stroke stroke)
        {
            List<double> yPos = new List<double>();
            List<double> relocations = new List<double>();
            yPos = getYpos(stroke);
            return yPos.Max() - yPos.Min();
        }

        //F22 - DistanceX - difference of X position from start to the end
        public double distanceX(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> xDistances = new List<double>();
            xPos = getXpos(stroke);
            return xPos.Last() - xPos.First();
        }

        //F23 - DistanceY - difference of Y position from start to the end
        public double distanceY(Stroke stroke)
        {
            List<double> yPos = new List<double>();
            List<double> yDistances = new List<double>();
            yPos = getYpos(stroke);
            return yPos.Last() - yPos.First();
        }

        //F24- Distance - total displacement
        public double displacement(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();

            xPos = getXpos(stroke);
            yPos = getYpos(stroke);
            double distances = (double)Math.Sqrt(Math.Pow(yPos.Last() - yPos.First(), 2) + Math.Pow(xPos.Last() - xPos.First(), 2));
            return distances;
        }
        #endregion

        #region Posicional Changes First Derivation - F25, F26, F27
        //F25 - averageSpeedX - first derivation of X changes
        public double firstDVPCx(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> speeds = new List<double>();
            double deltaX = 0f, deltaT = duration(stroke);
            double sum = 0f;

            speeds.Add(0f);
            durations = getTimestamps(stroke);
            xPos = getXpos(stroke);

            //its safe to assume xPos.Count == durations.Count
            for (int j = 1; j < xPos.Count(); j++)
            {
                deltaX = Math.Abs(xPos[j] - xPos[j - 1]);
                speeds.Add(deltaX);
                //deltaT = Math.Abs(durations[j] - durations[j - 1]);
                //speeds.Add(deltaX / deltaT);
            }
            foreach (double d in speeds)
            {
                sum += d;
            }
            return sum / deltaT;
        }

        //F26 - averageSpeedY - first derivation of Y changes
        public double firstDVPCy(Stroke stroke)
        {
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> speeds = new List<double>();
            double deltaY = 0f, deltaT = duration(stroke);
            double sum = 0f;

            speeds.Add(0f);
            durations = getTimestamps(stroke);
            yPos = getYpos(stroke);

            //its safe to assume xPos.Count == durations.Count
            for (int j = 1; j < yPos.Count(); j++)
            {
                deltaY = Math.Abs(yPos[j] - yPos[j - 1]);
                speeds.Add(deltaY);
                //deltaT = Math.Abs(durations[j] - durations[j - 1]);
                //speeds.Add(deltaX / deltaT);
            }
            foreach (double d in speeds)
            {
                sum += d;
            }
            return sum / deltaT;
        }

        public double firstDVPP (Stroke stroke)
        {
            return velocity(stroke);
        }
        #endregion

        #region Posicional Changes Second Derivation - F28, F29, F30
        //F28 - averageAccelerationX - second derivation of X changes
        public double secondDVPCx(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> avgSpeed = new List<double>();
            List<double> avgAccel = new List<double>();
            double deltaX = 0f, deltaT = 0.0121f, sum = 0f, ds = duration(stroke);

            avgSpeed.Add(0f);
            durations = getTimestamps(stroke);
            xPos = getXpos(stroke);

            //its safe to assume xPos.Count == durations.Count
            for (int j = 1; j < xPos.Count(); j++)
            {
                deltaX = (xPos[j] - xPos[j - 1]);
                avgSpeed.Add(deltaX / deltaT);
            }
            foreach (double d in avgSpeed)
            {
                sum += d;
            }

            return sum / ds;
        }

        //F29 - averageAccelerationy - second derivation of Y changes
        public double secondDVPCy(Stroke stroke)
        {
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> avgSpeed = new List<double>();
            List<double> avgAccel = new List<double>();
            double deltaY = 0f, deltaT = 0.0121f, sum = 0f, ds = duration(stroke);

            avgSpeed.Add(0f);
            durations = getTimestamps(stroke);
            yPos = getYpos(stroke);

            //its safe to assume xPos.Count == durations.Count
            for (int j = 1; j < yPos.Count(); j++)
            {
                deltaY = (yPos[j] - yPos[j - 1]);
                avgSpeed.Add(deltaY / deltaT);
            }
            foreach (double d in avgSpeed)
            {
                sum += d;
            }

            return sum / ds;
        }

        public double secondDVPP(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            double durations = duration(stroke);
            List<double> avgAccel = new List<double>();
            List<double> vTmp = new List<double>();
            double deltaVx = 0f, deltaVy = 0f, sum = 0f;

            xPos = getXpos(stroke);
            yPos = getYpos(stroke);

            /*
             *      speedChangeX = positionchangeX./ADBFreq;
                    speedChangeY = positionchangeY./ADBFreq;
                    speedChange = ((speedChangeX .^ 2) + (speedChangeY .^ 2)) .^ 0.5;
                    avgAcc = sum(speedChange) ./ (dt);
             * */


            //its safe to assume xPos.Count == yPos.Count == durations.Count
            for (int j = 1; j < yPos.Count(); j++)
            {
                deltaVx = Math.Abs(xPos[j] - xPos[j - 1])/0.0121;
                deltaVy = Math.Abs(yPos[j] - yPos[j - 1])/0.0121;
                var distTmp = (double)Math.Sqrt(Math.Pow(deltaVx, 2) + (double)Math.Pow(deltaVy, 2));
                vTmp.Add(distTmp);
            }

            for (int v = 0; v < vTmp.Count; v++)
            {
                sum += vTmp[v];
            }

            var b = sum/durations;
            return b;
        }

        #endregion

        #region Velocity and Acceleration - F31, F32
        //F31 - velocity- average speed
        public double velocity(Stroke stroke)
        {
            return lengthT(stroke) / duration(stroke);

        }

        //F32 - acceleration - average acceleration
        public double acceleration(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> avgAccel = new List<double>();
            List<double> vTmp = new List<double>();
            double deltaX = 0f, deltaY = 0f;

            xPos = getXpos(stroke);
            yPos = getYpos(stroke);

            //its safe to assume xPos.Count == yPos.Count == durations.Count
            for (int j = 1; j < yPos.Count(); j++)
            {
                deltaX = Math.Abs(xPos[j] - xPos[j - 1]);
                deltaY = Math.Abs(yPos[j] - yPos[j - 1]);
                var distTmp = (double)Math.Sqrt(Math.Pow(deltaX, 2) + (double)Math.Pow(deltaY, 2));
                vTmp.Add(distTmp / 0.0121);
            }

            for(int v = 1; v < vTmp.Count; v++)
            {
                var a = vTmp[v] - vTmp[v-1];
                avgAccel.Add(a/(2*0.0121));
            }

            var b = avgAccel.Average();
            return avgAccel.Average();
        }
        #endregion

        #region Angular Velocity - F33, F34, F35
        //F33 - wj - angular velocity
        public double wj(Stroke stroke, string operation)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> dx = new List<double>();
            List<double> dy = new List<double>();
            List<double> angVel = new List<double>();

            durations = getTimestamps(stroke);
            xPos = getXpos(stroke);
            yPos = getYpos(stroke);

            for (int i = 1; i < xPos.Count(); i++)
            {
                dx.Add(xPos[i] - xPos[i - 1]);
                dy.Add(yPos[i] - yPos[i - 1]);
            }

            for (int j = 1; j < dx.Count(); j++)
            {
                var a = (Math.Sqrt(dx[j] * dx[j] + dy[j] * dy[j]) * Math.Sqrt(dx[j - 1] * dx[j - 1] + dy[j - 1] * dy[j - 1]));
                if (a != 0)
                {
                    var wj = SignificantTruncate(((dx[j] * dx[j - 1] + dy[j] * dy[j - 1]) / a), 6);
                    angVel.Add((double) Math.Acos(wj) / (durations[j] - durations[j - 1]));
                }
                else
                {
                    angVel.Add(0);
                }
            }
            switch (operation)
            {
                case "mean":
                    return angVel.Average();
                case "min":
                    return angVel.Min();
                case "max":
                    return angVel.Max();
                default:
                    return -1;
            }
        }
        #endregion

        #region Curl - F36, F37 - Unimplemented
        #endregion

        #region First Derivation of Curl - F38, F39 - Unimplemented
        #endregion

        #region Angle - F40, F41, F42
        //F40 - angleM the angle of the middle point of the stroke
        public double angleM(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            int midIndex = 0, firstQuarterPos = 0, lastQuarterPos = 0;
            double deltaX1 = 0f, deltaY1 = 0f;
            double deltaX2 = 0f, deltaY2 = 0f;
            double angle = 0f;

            xPos = getXpos(stroke);
            yPos = getYpos(stroke);

            midIndex = (int) Math.Floor((xPos.Count()) / 2f);
            firstQuarterPos = (int) Math.Floor((xPos.Count()) / 4f);
            lastQuarterPos = (int) Math.Floor((xPos.Count()) * (3f / 4));

            deltaX1 = xPos[0] - xPos[midIndex];
            deltaY1 = yPos[0] - yPos[midIndex];
            deltaX2 = xPos[xPos.Count() - 1] - xPos[midIndex];
            deltaY2 = yPos[xPos.Count() - 1] - yPos[midIndex];

            var dot = deltaX1 * deltaX2 + deltaY1 * deltaY2;
            var normU = Math.Round(Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1),4);
            var normV= Math.Round(Math.Sqrt(deltaX2 * deltaX2 + deltaY2 * deltaY2),4);
            if (normU*normV != 0)
            {
                angle = Math.Acos(Math.Round(dot / (normU*normV),4));
                angle = angle * 57.2958; //to degrees
                var result = Math.Round(angle, 4);
                if (Double.IsNaN(result) || Double.IsInfinity(result))
                {
                    return 0;
                }
                else
                    return result;
            }
            else
                return 0;
        }

        //F41 - angle1 the angle between mid point and starting point - in degrees
        public double angle1(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            int firstQuarterPos = 0, midPos = 0;
            double deltaX1 = 0f, deltaY1 = 0f;
            double deltaX2 = 0f, deltaY2 = 0f;
            double angle = 0f;

            xPos = getXpos(stroke);
            yPos = getYpos(stroke);

            midPos = (int)Math.Floor((xPos.Count()) / 2f);
            firstQuarterPos = (int)Math.Floor((xPos.Count()) / 4f);

            deltaX1 = xPos[firstQuarterPos] - xPos[0];
            deltaY1 = yPos[firstQuarterPos] - yPos[0];
            deltaX2 = xPos[firstQuarterPos] - xPos[midPos];
            deltaY2 = yPos[firstQuarterPos] - yPos[midPos];

            var dot = deltaX1 * deltaX2 + deltaY1 * deltaY2;
            var normU = Math.Round(Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1), 4);
            var normV = Math.Round(Math.Sqrt(deltaX2 * deltaX2 + deltaY2 * deltaY2), 4);
            if (normU * normV != 0)
            {
                angle = Math.Acos(Math.Round(dot / (normU * normV), 4));
                angle = angle * 57.2958; //to degrees
                return Math.Round(angle, 4);
            }
            else
                return 0;

        }

        //F42 - angle2 the angle between mid point and the ending point
        public double angle2(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            int lastQuarter = 0, midPos = 0;
            double deltaX1 = 0f, deltaY1 = 0f;
            double deltaX2 = 0f, deltaY2 = 0f;
            double angle = 0f;

            xPos = getXpos(stroke);
            yPos = getYpos(stroke);

            midPos = (int)Math.Floor((xPos.Count()) / 2f);
            lastQuarter = (int)Math.Floor((xPos.Count()) * (3f / 4));

            deltaX1 = xPos[lastQuarter] - xPos[midPos];
            deltaY1 = yPos[lastQuarter] - yPos[midPos];
            deltaX2 = xPos[lastQuarter] - xPos[xPos.Count - 1];
            deltaY2 = yPos[lastQuarter] - yPos[yPos.Count - 1];

            var dot = deltaX1 * deltaX2 + deltaY1 * deltaY2;
            var normU = Math.Round(Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1), 4);
            var normV = Math.Round(Math.Sqrt(deltaX2 * deltaX2 + deltaY2 * deltaY2), 4);
            if (normU * normV != 0)
            {
                angle = Math.Acos(Math.Round(dot / (normU * normV), 4));
                angle = angle * 57.2958; //to degrees
                return Math.Round(angle, 4);
            }
            else
                return 0;
        }
        #endregion

        #region # of strokes within time period - F43, F44, F45, F46
        /// <summary>
        /// Returns a list of the number of strokes within a given time frequency (How many strokes happened in each interval of 1, 3, 5 or 10 seconds).
        /// </summary>
        /// <param name="strokeList"></param> The List of strokes to be calculated.
        /// <param name="interval"></param> The time period in which the counting is calculated (1, 3, 5 and 10).
        /// <param name="inclusivity"></param> Tells if the algorithm only counts full strokes within the time period. 
        /// <returns></returns>
        /*public List<double> numOfStrk(List<Stroke> strokeList, int interval, bool inclusivity)
        {
            double elapsedTime = 0, firstInstant = 0;
            firstInstant = strokeList[0].Points[0].TimeStamp;
            HashSet<Stroke> strokes = new HashSet<Stroke>();
            List<double> strokeCounts = new List<double>();
            for (int i = 0; i < strokeList.Count(); i++)
            {
                int numOfPoints = strokeList[i].Points.Count();
                int pointsInsideFrame = 0;
                List<Point> points = new List<Point>();
                for (int j = 0; j < strokeList[i].Points.Count(); j++)
                {
                    double currentTime = strokeList[i].Points[j].TimeStamp;
                    elapsedTime = Math.Round((currentTime - firstInstant), 6);
                    if (elapsedTime < interval)
                    {
                        if (inclusivity == false)
                        {
                            strokes.Add(strokeList[i]);
                        }
                        else
                        {
                            points.Add(strokeList[i].Points[j]);
                            pointsInsideFrame++;
                            if(pointsInsideFrame == numOfPoints)
                            {
                                Stroke s = new Stroke(points);
                                strokes.Add(s);
                                points.Clear();
                            }
                        }
                    }
                    else if(elapsedTime > interval)
                    {
                        firstInstant += interval;
                        strokeCounts.Add(strokes.Count());
                        strokes.Clear();
                        j -= 1;
                    }
                }
            }
            return strokeCounts;
        }*/

        public int numOfStrk(List<Stroke> strokeList, int strokeIndex, int interval)
        {
            Stroke currentStroke = strokeList[strokeIndex];
            double strokeEndTime = 0f;
            int strokeCount = 0;
            int lastPoint = currentStroke.Points.Count() - 1;
            strokeEndTime = currentStroke.Points[lastPoint].TimeStamp;

            for(int i = strokeIndex-1; i >= 0; i--)
            {
                Stroke tmp = strokeList[i];
                double currentTime = tmp.Points[0].TimeStamp;
                var delta = strokeEndTime - currentTime;
                if(delta < interval)
                {
                    strokeCount++;
                }
                else
                {
                    return strokeCount;
                }
            }

            return strokeCount;
        }

        #endregion

        #region Area - F47
        public double area(Stroke stroke)
        {
            double distX = xMax(stroke) - xMin(stroke);
            double distY = yMax(stroke) - yMin(stroke);
            return distX * distY;
        }


        #endregion

        #region Emotion, Valence and Arousal Parsing
        public string getEmotionDebug(Stroke stroke, List<Stroke> strokeList, String[][] emotionData, int i)
        {
            List<string[]> strokeWindow = new List<string[]>();
            string emotion = "";
            double sessionStrokeStartTime = strokeList[0].Points[0].TimeStamp;
            double currentStrokeStartTime = Math.Round(stroke.Points[0].TimeStamp - sessionStrokeStartTime, 2);
            double currentStrokeEndTime = Math.Round(stroke.Points[stroke.Points.Count() - 1].TimeStamp - sessionStrokeStartTime, 2);
            for (int j = 1; j < emotionData.GetLength(0); j++)
            {
                double currentEmotionTime = Double.Parse(emotionData[j][0]);
                if (currentEmotionTime >= currentStrokeStartTime)
                {
                    if (currentEmotionTime <= currentStrokeEndTime)
                    {
                        strokeWindow.Add(emotionData[j]);
                    }
                    if (currentEmotionTime >= currentStrokeEndTime)
                    {
                        if (strokeWindow.Count > 0)
                        {
                            Console.WriteLine("Stroke " + i + " ---- apanhei " + strokeWindow.Count + " linhas de emotional data");
                            Console.WriteLine("Stroke " + i + " ---- Emotion Start Time : " + strokeWindow[0][0]);
                            Console.WriteLine("Stroke " + i + " ---- Stroke Time : " + currentStrokeStartTime + "      " + currentStrokeEndTime + "\n");
                            emotion = FindMaxEmotion(strokeWindow);
                            strokeWindow.Clear();
                            return emotion;
                        }
                        else
                        {
                            Console.WriteLine("Stroke " + i + " ---- nao apanhei emotional data");
                            Console.WriteLine("Stroke " + i + " ---- Stroke Time : " + currentStrokeStartTime + "      " + currentStrokeEndTime);
                            strokeWindow.Add(emotionData[j-1]);
                            emotion = FindMaxEmotion(strokeWindow);
                            strokeWindow.Clear();
                            return emotion;
                        }
                    }
                    else
                        continue;
                }
            }
            return emotion;
        }

        public string getEmotionData(Stroke stroke, List<Stroke> strokeList, String[][] emotionData)
        {
            List<string[]> strokeWindow = new List<string[]>();
            string emotion = "";
            double sessionStrokeStartTime = strokeList[0].Points[0].TimeStamp;
            double currentStrokeStartTime = Math.Round(stroke.Points[0].TimeStamp - sessionStrokeStartTime, 2);
            double currentStrokeEndTime = Math.Round(stroke.Points[stroke.Points.Count() - 1].TimeStamp - sessionStrokeStartTime, 2);
            for (int j = 1; j < emotionData.GetLength(0); j++)
            {
                double currentEmotionTime = Double.Parse(emotionData[j][0]);
                if (currentEmotionTime >= currentStrokeStartTime)
                {
                    if (currentEmotionTime <= currentStrokeEndTime)
                    {
                        strokeWindow.Add(emotionData[j]);
                    }
                    if (currentEmotionTime >= currentStrokeEndTime)
                    {
                        if (strokeWindow.Count > 0)
                        {
                            emotion = FindMaxEmotion(strokeWindow);
                            strokeWindow.Clear();
                            return emotion;
                        }
                        else
                        {
                            strokeWindow.Add(emotionData[j - 1]);
                            emotion = FindMaxEmotion(strokeWindow);
                            strokeWindow.Clear();
                            return emotion;
                        }
                    }
                    else
                        continue;
                }
            }
            return emotion;
        }

        public string getValenceData(Stroke stroke, List<Stroke> strokeList, String[][] emotionData)
        {
            List<string[]> strokeWindow = new List<string[]>();
            string valence = "";
            double sessionStrokeStartTime = strokeList[0].Points[0].TimeStamp;
            double currentStrokeStartTime = Math.Round(stroke.Points[0].TimeStamp - sessionStrokeStartTime, 2);
            double currentStrokeEndTime = Math.Round(stroke.Points[stroke.Points.Count() - 1].TimeStamp - sessionStrokeStartTime, 2);
            for (int j = 1; j < emotionData.GetLength(0); j++)
            {
                double currentEmotionTime = Double.Parse(emotionData[j][0]);
                if (currentEmotionTime >= currentStrokeStartTime)
                {
                    if (currentEmotionTime <= currentStrokeEndTime)
                    {
                        strokeWindow.Add(emotionData[j]);
                    }
                    if (currentEmotionTime >= currentStrokeEndTime)
                    {
                        if (strokeWindow.Count > 0)
                        {
                            valence = FindMaxValence(strokeWindow);
                            strokeWindow.Clear();
                            return valence;
                        }
                        else
                        {
                            strokeWindow.Add(emotionData[j - 1]);
                            valence = FindMaxValence(strokeWindow);
                            strokeWindow.Clear();
                            return valence;
                        }
                    }
                    else
                        continue;
                }
            }
            return valence;
        }

        public string getArousalData(Stroke stroke, List<Stroke> strokeList, String[][] emotionData)
        {
            List<string[]> strokeWindow = new List<string[]>();
            string arousal = "";
            double sessionStrokeStartTime = strokeList[0].Points[0].TimeStamp;
            double currentStrokeStartTime = Math.Round(stroke.Points[0].TimeStamp - sessionStrokeStartTime, 2);
            double currentStrokeEndTime = Math.Round(stroke.Points[stroke.Points.Count() - 1].TimeStamp - sessionStrokeStartTime, 2);
            for (int j = 1; j < emotionData.GetLength(0); j++)
            {
                double currentEmotionTime = Double.Parse(emotionData[j][0]);
                if (currentEmotionTime >= currentStrokeStartTime)
                {
                    if (currentEmotionTime <= currentStrokeEndTime)
                    {
                        strokeWindow.Add(emotionData[j]);
                    }
                    if (currentEmotionTime >= currentStrokeEndTime)
                    {
                        if (strokeWindow.Count > 0)
                        {
                            arousal = FindMaxArousal(strokeWindow);
                            strokeWindow.Clear();
                            return arousal;
                        }
                        else
                        {
                            strokeWindow.Add(emotionData[j - 1]);
                            arousal = FindMaxArousal(strokeWindow);
                            strokeWindow.Clear();
                            return arousal;
                        }
                    }
                    else
                        continue;
                }
            }
            return arousal;
        }

        private string FindMaxEmotion(List<string[]> emotionData)
        {
            List<double> happy = new List<double>();
            List<double> sad = new List<double>();
            List<double> angry = new List<double>();
            List<double> supr = new List<double>();
            List<double> scared = new List<double>();
            List<double> disgusted = new List<double>();

            foreach (string[] line in emotionData)
            {
                for(int i = 1; i < line.Count(); i++)
                {
                    switch (i)
                    {
                        case 2:
                            happy.Add(Double.Parse(line[i]));
                            break;
                        case 3:
                            sad.Add(Double.Parse(line[i]));
                            break;
                        case 4:
                            angry.Add(Double.Parse(line[i]));
                            break;
                        case 5:
                            supr.Add(Double.Parse(line[i]));
                            break;
                        case 6:
                            scared.Add(Double.Parse(line[i]));
                            break;
                        case 7:
                            disgusted.Add(Double.Parse(line[i]));
                            break;
                        default:
                            break;
                    }
                }
            }
            List<Double> averageList = new List<double>();
            var averages = new double[] { happy.Average(), sad.Average(), angry.Average(), supr.Average(), scared.Average(), disgusted.Average()};
            averageList.AddRange(averages);
            var maxi = averages.Max();
            var id = averages.ToList().IndexOf(maxi);

            string emotion = "";

            if (id > -1)
            {
                switch (id)
                {
                    case 0:
                        emotion = "happy";
                        //emotion = "1";
                        break;
                    case 1:
                        emotion = "sad";
                        //emotion = "2";
                        break;
                    case 2:
                        emotion = "angry";
                        //emotion = "3";
                        break;
                    case 3:
                        emotion = "surprised";
                        //emotion = "4";
                        break;
                    case 4:
                        emotion = "scared";
                        //emotion = "5";
                        break;
                    case 5:
                        emotion = "disgusted";
                        //emotion = "6";
                        break;
                    default:
                        emotion = "error - " + id.ToString();
                        break;
                }
            }

            return emotion;
        }

        private string FindMaxValence(List<string[]> emotionData)
        {
            List<double> valence = new List<double>();
            foreach (string[] line in emotionData)
            {
                for (int i = 1; i < line.Count(); i++)
                {
                    valence.Add(Double.Parse(line[9]));
                }
            }
            return valence.Average().ToString();
        }

        private string FindMaxArousal(List<string[]> emotionData)
        {
            List<double> arousal = new List<double>();
            foreach (string[] line in emotionData)
            {
                for (int i = 1; i < line.Count(); i++)
                {
                    arousal.Add(Double.Parse(line[10]));
                }
            }
            return arousal.Average().ToString();
        }

        #endregion
    }
}
