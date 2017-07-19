using System;
using System.Collections.Generic;
using System.IO;
using Accord.Math;
using System.Linq;
using System.Text.RegularExpressions;

namespace StrokeParser
{
    public class Parser
    {
        public List<Stroke> _strokes = new List<Stroke>();

        public Parser(string filePath)
        {
            string[][] data = ReadStrokeFile(filePath);
            _strokes = GetStrokes(data);
        }

        public Parser(List<Stroke> strokeList)
        {
            foreach (Stroke stroke in strokeList)
            {
                //var cena = wj1(stroke);
            }
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
                if (strokeData[i][3] == "SYN_REPORT")
                {
                    Point point = new Point(strokeData[i][0], posX, posY, touch, width);
                    points.Add(point);
                    posX = "";
                    posY = "";
                    touch = "";
                    width = "";
                    i++;
                }
                if (strokeData[i][4] == "UP")
                {
                    i +=1;
                    Stroke s = new Stroke(points);
                    strokes.Add(s);
                    points.Clear();
                }
            }
            return strokes;
        }

        public string GetResults(List<int> featureNums)
        {
            string result = "";
            double initTime = 0;
            bool stroke1 = false, stroke3 = false, stroke5 = false, stroke10 = false;
            initTime = _strokes[0].Points[0].TimeStamp;
            for (int i = 0; i < _strokes.Count(); i++)
            {
                result += "\nStroke " + i + "\n";
                for (int f = 0; f < featureNums.Count(); f++)
                {
                    switch (featureNums[f])
                    {
                        case 1:
                            result += "F" + featureNums[f] + " - Duration\t" + duration(_strokes[i]) + "\n";
                            break;
                        case 2:
                            if (i > 0)
                            {
                                result += "F" + featureNums[f] + " - Dist2Prev\t" + dist2prev(_strokes[i - 1], _strokes[i]) + "\n";
                            }
                            break;
                        case 3:
                            result += "F" + featureNums[f] + " - TimeElapsed\t" + timeElapsed(_strokes[i], initTime) + "\n";
                            break;
                        case 4:
                            result += "F" + featureNums[f] + " - xMin\t" + xMin(_strokes[i]) + "\n";
                            break;
                        case 5:
                            result += "F" + featureNums[f] + " - xMax\t" + xMax(_strokes[i]) + "\n";
                            break;
                        case 6:
                            result += "F" + featureNums[f] + " - xMean\t" + xMean(_strokes[i]) + "\n";
                            break;
                        case 7:
                            result += "F" + featureNums[f] + " - xMedian\t" + xMedian(_strokes[i]) + "\n";
                            break;
                        case 8:
                            result += "F" + featureNums[f] + " - xSTD\t" + xStd(_strokes[i]) + "\n";
                            break;
                        case 9:
                            result += "F" + featureNums[f] + " - yMin\t" + yMin(_strokes[i]) + "\n";
                            break;
                        case 10:
                            result += "F" + featureNums[f] + " - yMax\t" + yMax(_strokes[i]) + "\n";
                            break;
                        case 11:
                            result += "F" + featureNums[f] + " - yMean\t" + yMean(_strokes[i]) + "\n";
                            break;
                        case 12:
                            result += "F" + featureNums[f] + " - yMedian\t" + yMedian(_strokes[i]) + "\n";
                            break;
                        case 13:
                            result += "F" + featureNums[f] + " - ySTD\t" + yStd(_strokes[i]) + "\n";
                            break;
                        case 14:
                            result += "F" + featureNums[f] + " - pMin\t" + pMin(_strokes[i]) + "\n";
                            break;
                        case 15:
                            result += "F" + featureNums[f] + " - pMax\t" + pMax(_strokes[i]) + "\n";
                            break;
                        case 16:
                            result += "F" + featureNums[f] + " - pMean\t" + pMean(_strokes[i]) + "\n";
                            break;
                        case 17:
                            result += "F" + featureNums[f] + " - pMedian\t" + pMedian(_strokes[i]) + "\n";
                            break;
                        case 18:
                            result += "F" + featureNums[f] + " - pSTD\t" + pStd(_strokes[i]) + "\n";
                            break;
                        case 19:
                            result += "F" + featureNums[f] + " - lengthT\t" + lengthT(_strokes[i]) + "\n";
                            break;
                        case 20:
                            result += "F" + featureNums[f] + " - spanX\t" + spanX(_strokes[i]) + "\n";
                            break;
                        case 21:
                            result += "F" + featureNums[f] + " - spanY\t" + spanY(_strokes[i]) + "\n";
                            break;
                        case 22:
                            result += "F" + featureNums[f] + " - distanceX\t" + distanceX(_strokes[i]) + "\n";
                            break;
                        case 23:
                            result += "F" + featureNums[f] + " - distanceY\t" + distanceY(_strokes[i]) + "\n";
                            break;
                        case 24:
                            result += "F" + featureNums[f] + " - displacement\t" + displacement(_strokes[i]) + "\n";
                            break;
                        case 25:
                            result += "F" + featureNums[f] + " - 1stDVPCx\t" + firstDVPCx(_strokes[i]) + "\n";
                            break;
                        case 26:
                            result += "F" + featureNums[f] + " - 1stDVPCy\t" + firstDVPCy(_strokes[i]) + "\n";
                            break;
                        case 27:
                            result += "F" + featureNums[f] + " - 1stDVPP\t unimplemented \n";
                            //result += "F" + featureNums[f] + " - 1stDVPP\t" + firstDVPP(strokes[i]) + "\n";
                            break;
                        case 28:
                            result += "F" + featureNums[f] + " - 2ndDVPCx\t" + secondDVPCx(_strokes[i]) + "\n";
                            break;
                        case 29:
                            result += "F" + featureNums[f] + " - 2ndDVPCx\t" + secondDVPCy(_strokes[i]) + "\n";
                            break;
                        case 30:
                            result += "F" + featureNums[f] + " - 2ndDVPP\t unimplemented \n";
                            //result += "F" + featureNums[f] + " - 2ndDVPP\t" + secondDVPP(strokes[i]) + "\n";
                            break;
                        case 31:
                            result += "F" + featureNums[f] + " - velocity\t" + velocity(_strokes[i]) + "\n";
                            break;
                        case 32:
                            result += "F" + featureNums[f] + " - acceleration\t" + acceleration(_strokes[i]) + "\n";
                            break;
                        case 33:
                            result += "F" + featureNums[f] + " - wj_Mean\t" + wj(_strokes[i], "mean") + "\n";
                            break;
                        case 34:
                            result += "F" + featureNums[f] + " - wj_Min\t" + wj(_strokes[i], "min") + "\n";
                            break;
                        case 35:
                            result += "F" + featureNums[f] + " - wj_Max\t" + wj(_strokes[i], "max") + "\n";
                            break;
                        case 36:
                            result += "F" + featureNums[f] + " - CurlX\t unimplemented \n";
                            //result += "F" + featureNums[f] + " - curlX\t" + curlX(strokes[i]) + "\n";
                            break;
                        case 37:
                            result += "F" + featureNums[f] + " - CurlY\t unimplemented \n";
                            //result += "F" + featureNums[f] + " - curlY\t" + curlY(strokes[i]) + "\n";
                            break;
                        case 38:
                            result += "F" + featureNums[f] + " - 1stDVCurlx\t unimplemented \n";
                            //result += "F" + featureNums[f] + " - 1stDVCurlx\t" + firstDVCurlx(strokes[i]) + "\n";
                            break;
                        case 39:
                            result += "F" + featureNums[f] + " - 1stDVCurly\t unimplemented \n";
                            //result += "F" + featureNums[f] + " - 1stDVCurly\t" + firstDVCurly(strokes[i]) + "\n";
                            break;
                        case 40:
                            result += "F" + featureNums[f] + " - angleM\t" + angleM(_strokes[i]) + "\n";
                            break;
                        case 41:
                            result += "F" + featureNums[f] + " - angle1\t" + angle1(_strokes[i]) + "\n";
                            break;
                        case 42:
                            result += "F" + featureNums[f] + " - angle2\t" + angle2(_strokes[i]) + "\n";
                            break;
                        case 43:
                            stroke1 = true;
                            break;
                        case 44:
                            stroke3 = true;
                            break;
                        case 45:
                            stroke5 = true;
                            break;
                        case 46:
                            stroke10 = true;
                            break;
                        case 47:
                            result += "F" + featureNums[f] + " - area\t" + area(_strokes[i]) + "\n";
                            break;
                        default:
                            break;
                    }
                    Console.WriteLine("feature " + featureNums[f] + " done");
                }
                Console.WriteLine("\nstroke " + i + " done\n");

            }
            result += "\nSession Results\n";
            if (stroke1)
            {
                stroke1 = false;
                result += "F43 - numOfStrkW1\t" + print(numOfStrk(_strokes, 1, true)) + "\n";
            }
            if (stroke3)
            {
                stroke3 = false;
                result += "F44 - numOfStrkW3\t" + print(numOfStrk(_strokes, 3, true)) + "\n";
            }
            if (stroke5)
            {
                stroke5 = false;
                result += "F45 - numOfStrkW5\t" + print(numOfStrk(_strokes, 4, true)) + "\n";
            }
            if (stroke10)
            {
                stroke10 = false;
                result += "F46 - numOfStrkW10\t" + print(numOfStrk(_strokes, 10, true)) + "\n";
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

            return variance / values.Count;
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
            return duration;
        }

        //F2 - Dist2Prev - subtracting the initial timestamp of the current stroke, from the initial timestamp of the previous stroke.
        public double dist2prev(Stroke previous, Stroke current)
        {
            double currentStrokeTime = current.Points[0].TimeStamp;
            double previousStrokeTime = previous.Points[0].TimeStamp;
            double duration = currentStrokeTime - previousStrokeTime;
            return duration;
        }

        //F3 - timeElapsed - time elapsed from starting
        public double timeElapsed(Stroke stroke, double initTime)
        {
            double currentStrokeTime = stroke.Points[0].TimeStamp;
            double duration = currentStrokeTime - initTime;
            return duration;
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
            return StandardDeviation(getXpos(stroke));
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
            return StandardDeviation(getYpos(stroke));
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
            return getPressures(stroke).Average();
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
            return StandardDeviation(getPressures(stroke));
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
            return traveledPath;
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
            double deltaX = 0f, deltaT = 0f;

            speeds.Add(0f);
            durations = getTimestamps(stroke);
            xPos = getXpos(stroke);

            //its safe to assume xPos.Count == durations.Count
            for (int j = 1; j < xPos.Count(); j++)
            {
                deltaX = xPos[j] - xPos[j - 1];
                deltaT = durations[j] - durations[j - 1];
                speeds.Add(deltaX / deltaT);
            }
            return speeds.Average();
        }

        //F26 - averageSpeedY - first derivation of Y changes
        public double firstDVPCy(Stroke stroke)
        {
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> speeds = new List<double>();
            double deltaY = 0f, deltaT = 0f;

            speeds.Add(0f);
            durations = getTimestamps(stroke);
            yPos = getYpos(stroke);

            //its safe to assume yPos.Count == durations.Count
            for (int j = 1; j < yPos.Count(); j++)
            {
                deltaY = yPos[j] - yPos[j - 1];
                deltaT = durations[j] - durations[j - 1];
                speeds.Add(deltaY / deltaT);
            }
            return speeds.Average();
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
            double deltaX = 0f, deltaT = 0f, deltaV = 0f;

            avgSpeed.Add(0f);
            durations = getTimestamps(stroke);
            xPos = getXpos(stroke);

            //its safe to assume xPos.Count == durations.Count
            for (int j = 1; j < xPos.Count(); j++)
            {
                deltaX = xPos[j] - xPos[j - 1];
                deltaT = durations[j] - durations[j - 1];
                avgSpeed.Add(deltaX / deltaT);
                deltaV = avgSpeed[j] - avgSpeed[j - 1];
                avgAccel.Add(deltaV / deltaT);
            }

            return avgAccel.Average();
        }

        //F29 - averageAccelerationy - second derivation of Y changes
        public double secondDVPCy(Stroke stroke)
        {
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> avgSpeed = new List<double>();
            List<double> avgAccel = new List<double>();
            double deltaY = 0f, deltaT = 0f, deltaV = 0f;

            avgSpeed.Add(0f);
            durations = getTimestamps(stroke);
            yPos = getYpos(stroke);

            //its safe to assume yPos.Count == durations.Count
            for (int j = 1; j < yPos.Count(); j++)
            {
                deltaY = yPos[j] - yPos[j - 1];
                deltaT = durations[j] - durations[j - 1];
                avgSpeed.Add(deltaY / deltaT);
                deltaV = avgSpeed[j] - avgSpeed[j - 1];
                avgAccel.Add(deltaV / deltaT);
            }

            return avgAccel.Average();
        }
        #endregion

        #region Velocity and Acceleration - F31, F32
        //F31 - velocity- average speed
        public double velocity(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> speeds = new List<double>();
            double deltaX = 0f, deltaY = 0f, deltaT = 0f;

            speeds.Add(0f);
            xPos = getXpos(stroke);
            yPos = getYpos(stroke);
            durations = getTimestamps(stroke);

            //its safe to assume xPos.Count == yPos.Count == durations.Count
            for (int j = 1; j < yPos.Count(); j++)
            {
                deltaX = xPos[j] - xPos[j - 1];
                deltaY = yPos[j] - yPos[j - 1];
                deltaT = durations[j] - durations[j - 1];
                speeds.Add((double)Math.Sqrt(Math.Pow(deltaX, 2) + (double)Math.Pow(deltaY, 2)) / deltaT);
            }
            return speeds.Average();
        }

        //F32 - acceleration - average acceleration
        public double acceleration(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> avgSpeed = new List<double>();
            List<double> avgAccel = new List<double>();
            double deltaX = 0f, deltaY = 0f, deltaT = 0f, deltaV = 0f;

            avgSpeed.Add(0f);
            durations = getTimestamps(stroke);
            xPos = getXpos(stroke);
            yPos = getYpos(stroke);

            //its safe to assume xPos.Count == yPos.Count == durations.Count
            for (int j = 1; j < yPos.Count(); j++)
            {
                deltaX = xPos[j] - xPos[j - 1];
                deltaY = yPos[j] - yPos[j - 1];
                deltaT = durations[j] - durations[j - 1];
                avgSpeed.Add((double)Math.Sqrt(Math.Pow(deltaX, 2) + (double)Math.Pow(deltaY, 2)) / deltaT);
                deltaV = avgSpeed[j] - avgSpeed[j - 1];
                avgAccel.Add(deltaV / deltaT);
            }

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
            midIndex = (int)(xPos.Count() / 2);
            firstQuarterPos = (int)(xPos.Count() / 4);
            lastQuarterPos = (int)(xPos.Count() * (3/4));

            deltaX1 = Math.Abs(xPos[firstQuarterPos] - xPos[midIndex]);
            deltaY1 = Math.Abs(yPos[firstQuarterPos] - yPos[midIndex]);
            deltaX2 = Math.Abs(xPos[lastQuarterPos] - xPos[midIndex]);
            deltaY2 = Math.Abs(yPos[lastQuarterPos] - yPos[midIndex]);

            angle = ((Math.Atan2(deltaY1, deltaX1) - Math.Atan2(deltaY2, deltaX2)) * 180) / Math.PI;

            //var a = deltaX1 * deltaX2 + deltaY1 * deltaY2;
            //var b = Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1);
            //var c = Math.Sqrt(deltaX2 * deltaX2 + deltaY2 * deltaY2);
            //var d = a / (b + c);
            //angle = Math.Acos(d);

            return angle;
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
            firstQuarterPos = (int)(xPos.Count() / 4);
            midPos = (int)(xPos.Count() / 2);
            deltaX1 = Math.Abs(xPos[0] - xPos[firstQuarterPos]);
            deltaY1 = Math.Abs(yPos[0] - yPos[firstQuarterPos]);
            deltaX2 = Math.Abs(xPos[firstQuarterPos] - xPos[midPos]);
            deltaY2 = Math.Abs(yPos[firstQuarterPos] - yPos[midPos]);
            //in degrees
            angle = ((Math.Atan2(deltaY1, deltaX1) - Math.Atan2(deltaY2, deltaX2)) * 180) / Math.PI;
            return angle;
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
            lastQuarter = (int)(xPos.Count() * (3/4));
            midPos = (int)(xPos.Count() / 2);
            deltaX1 = Math.Abs(xPos[midPos] - xPos[lastQuarter]);
            deltaY1 = Math.Abs(yPos[midPos] - yPos[lastQuarter]);
            deltaX2 = Math.Abs(xPos[lastQuarter] - xPos[xPos.Count - 1]);
            deltaY2 = Math.Abs(yPos[lastQuarter] - yPos[yPos.Count - 1]);

            angle = ((Math.Atan2(deltaY1, deltaX1) - Math.Atan2(deltaY2, deltaX2)) * 180) / Math.PI;
            return angle;
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
        public List<double> numOfStrk(List<Stroke> strokeList, int interval, bool inclusivity)
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
    }
}
