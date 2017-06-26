using System;
using System.Collections.Generic;
using System.IO;
using Accord.Math;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace StrokeParser
{
    public class Parser
    {
        public Parser(string filePath)
        {
            string[][] data = ReadStrokes(filePath);
            Stroke[] strokes = SeparateStrokes(data);
            numOfStrk(strokes,1 ,true);
            foreach (Stroke stroke in strokes)
            {
                //var cena = wj1(stroke);
            }
        }

        public string[][] ReadStrokes(string filePath)
        {
            var file = File.ReadAllText(filePath);
            var dataSet = file.Split(new[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dataSet.Length(); i++)
            {
                dataSet[i] = dataSet[i].Replace("[", "").Replace("]", "");
                dataSet[i] = dataSet[i].Trim();
                dataSet[i] = Regex.Replace(dataSet[i], @"\s+", ",");
            }
            var parsedDataSet = dataSet.Apply(x => x.Split(','));
            List<string[]> clean = new List<string[]>();
            for (int j = 24; j < parsedDataSet.GetLength(0); j++)
            {
                clean.Add(parsedDataSet[j]);
            }
            return clean.ToArray();
        }

        public Stroke[] SeparateStrokes(string[][] strokeData)
        {
            List<Stroke> strokes = new List<Stroke>();
            List<Point> points = new List<Point>();

            for (int i = 0; i < strokeData.GetLength(0); i++)
            {
                List<String[]> pointInfo = new List<string[]>();
                if (strokeData[i][4] == "DOWN")
                {
                    i++;
                    while (strokeData[i][4] != "UP")
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            pointInfo.Add(strokeData[i + j]);
                        }
                        points.Add(new Point(pointInfo.ToArray()));
                        pointInfo.Clear();
                        i += 7;
                    }
                    if (strokeData[i][4] == "UP")
                    {
                        i++;
                    }
                }

                strokes.Add(new Stroke(points.ToArray()));
                points.Clear();
            }
            return strokes.ToArray();
        }

        #region aux

        private List<double> getXpos(Stroke stroke)
        {
            return getXpos(stroke, 0, stroke.Points.Length());
        }

        private List<double> getXpos(Stroke stroke, decimal start, decimal range)
        {
            List<double> xPos = new List<double>();

            for (int i = (int) start; i < range; i++)
            {
                xPos.Add(Convert.ToInt32(stroke.Points[i].PointInfo[0][4], 16));
            }
            return xPos;
        }

        private List<double> getYpos(Stroke stroke)
        {
            List<double> yPos = new List<double>();

            for (int i = 0; i < stroke.Points.Length(); i++)
            {
                yPos.Add(Convert.ToInt32(stroke.Points[i].PointInfo[1][4], 16));
            }
            return yPos;
        }

        private List<double> getYpos(Stroke stroke, decimal start, decimal range)
        {
            List<double> yPos = new List<double>();

            for (int i = (int) start; i < range; i++)
            {
                yPos.Add(Convert.ToInt32(stroke.Points[i].PointInfo[1][4], 16));
            }
            return yPos;
        }

        private List<double> getPressures(Stroke stroke)
        {
            List<double> pressures = new List<double>();

            for (int i = 0; i < stroke.Points.Length(); i++)
            {
                pressures.Add(Convert.ToInt32(stroke.Points[i].PointInfo[2][4], 16));
            }
            return pressures;
        }

        private List<double> getTimestamps (Stroke stroke)
        {
            List<double> timeStamps = new List<double>();

            for (int i = 0; i < stroke.Points.Length(); i++)
            {
                timeStamps.Add(double.Parse(stroke.Points[i].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat));
            }
            return timeStamps;
        }

        private List<double> getTimestamps(Stroke stroke, decimal start, decimal range)
        {
            List<double> timeStamps = new List<double>();

            for (int i = (int) start; i < range; i++)
            {
                timeStamps.Add(double.Parse(stroke.Points[i].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat));
            }
            return timeStamps;
        }

        #endregion

        #region Time - F1, F2, F3
        //F1 - Duration = Duration of the stroke
        public double duration(Stroke stroke)
        {
            int numberOfPoints = stroke.Points.Length();
            double lastInstant = Math.Round(double.Parse(stroke.Points[numberOfPoints - 1].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat), 6);
            double firstInstant = Math.Round(double.Parse(stroke.Points[0].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat), 6);
            double duration = lastInstant - firstInstant;
            return duration;
        }

        //F2 - Dist2Prev - subtracting the initial timestamp of the current stroke, from the initial timestamp of the previous stroke.
        public double dist2prev(Stroke previous, Stroke current)
        {
            double currentStrokeTime = Math.Round(double.Parse(current.Points[0].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat), 6);
            double previousStrokeTime = Math.Round(double.Parse(previous.Points[0].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat), 6);
            double duration = currentStrokeTime - previousStrokeTime;
            return duration;
        }

        //F3 - dist2start - time elapsed from starting
        public double dist2start(Stroke stroke, double initTime)
        {
            double currentStrokeTime = Math.Round(double.Parse(stroke.Points[0].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat), 6);
            double duration = currentStrokeTime - initTime;
            return duration;
        }

        #endregion

        #region Location - F4, F5, F6, F7, F8, F9
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

        //F7 - yMin - Min recorded Y
        public double yMin(Stroke stroke)
        {
            return getYpos(stroke).Min();
        }

        //F8 - yMax - Max recorded Y
        public double yMax(Stroke stroke)
        {
            return getYpos(stroke).Max();
        }

        //F9 - yMean - for these two features, values of min, max and mean should be calculated.
        public double yMean(Stroke stroke)
        {
            return getYpos(stroke).Average();
        }
        #endregion

        #region Pressure - F10, F11, F12
        //F10 - pMin - Min recorded Pressure
        public double pMin(Stroke stroke)
        {
            return getPressures(stroke).Min();
        }

        //F11 - pMax - Max recorded Pressure
        public double pMax(Stroke stroke)
        {
            return getPressures(stroke).Max();
        }

        //F12 - pMean
        public double pMean(Stroke stroke)
        {
            return getPressures(stroke).Average();
        }
        #endregion

        #region Positional Changes - F13, F14, F15, F16, F17, F18 
        //F13 - LenghtT - sum of pythagorean propositions
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

        //F14 - Relocation in X - xMax-Xmin;
        public double relocationX(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> relocations = new List<double>();
            xPos = getXpos(stroke);
            return xPos.Max() - xPos.Min();
        }

        //F15 - Relocation in Y - yMax-yMin;
        public double relocationY(Stroke stroke)
        {
            List<double> yPos = new List<double>();
            List<double> relocations = new List<double>();
            yPos = getYpos(stroke);
            return yPos.Max() - yPos.Min();
        }

        //F16 - DistanceX - difference of X position from start to the end
        public double distanceX(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> xDistances = new List<double>();
            xPos = getXpos(stroke);
            return xPos.Last() - xPos.First();
        }

        //F17 - DistanceY - difference of Y position from start to the end
        public double distanceY(Stroke stroke)
        {
            List<double> yPos = new List<double>();
            List<double> yDistances = new List<double>();
            yPos = getYpos(stroke);
            return yPos.Last() - yPos.First();
        }

        //F18- Distance - total displacement
        public double distance(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();

            xPos = getXpos(stroke);
            yPos = getYpos(stroke);
            double distances = (double)Math.Sqrt(Math.Pow(yPos.Last() - yPos.First(), 2) + Math.Pow(xPos.Last() - xPos.First(), 2));
            return distances;
        }
        #endregion

        #region Posicional Changes First Derivation
        //F19 - averageSpeedX - first derivation of X changes
        public double averageSpeedX(Stroke stroke)
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

        //F20 - averageSpeedY - first derivation of Y changes
        public double averageSpeedY(Stroke stroke)
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

        //F21 - averageSpeed - first derivation of the diameter of the rectangular span
        public double averageSpeed(Stroke stroke)
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

        #endregion

        #region Posicional Changes Second Derivation
        //F22 - averageAccelerationX - second derivation of X changes
        public double averageAccelerationX(Stroke stroke)
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

        public double averageAccelerationY(Stroke stroke)
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

        public double averageAcceleration(Stroke stroke)
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

        #region Angular Velocity
        //F25 - wj1 - Angular velocity of the first part of the stroke
        public double wj1 (Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> dx = new List<double>();
            List<double> dy = new List<double>();
            List<double> angVel = new List<double>();

            durations = getTimestamps(stroke, 0, Math.Floor((decimal)(stroke.Points.Length() / 2)));
            xPos = getXpos(stroke, 0, Math.Floor((decimal)(stroke.Points.Length() / 2)));
            yPos = getYpos(stroke, 0, Math.Floor((decimal)(stroke.Points.Length() / 2)));

            for (int i = 1; i < xPos.Count(); i++)
            {
                dx.Add(xPos[i] - xPos[i - 1]);
                dy.Add(yPos[i] - yPos[i - 1]);
            }

            for(int j = 1; j < dx.Count(); j++)
            {
                var cena = (Math.Sqrt(dx[j] * dx[j] + dy[j] * dy[j]) * Math.Sqrt(dx[j - 1] * dx[j - 1] + dy[j - 1] * dy[j - 1]));
                var wj = ((dx[j]*dx[j-1] + dy[j]*dy[j-1])/cena);
                angVel.Add((double) Math.Acos(wj)/(durations[j] - durations[j-1]));
            }
            return angVel.Average();
        }
        //F26 - wj2 - Angular velocity of the second part of the stroke
        public double wj2(Stroke stroke)
        {
            List<double> xPos = new List<double>();
            List<double> yPos = new List<double>();
            List<double> durations = new List<double>();
            List<double> dx = new List<double>();
            List<double> dy = new List<double>();
            List<double> angVel = new List<double>();
            double wj = 0f;

            dx.Add(0f);
            dy.Add(0f);
            durations = getTimestamps(stroke, Math.Floor((decimal)(stroke.Points.Length() / 2)), stroke.Points.Length());
            xPos = getXpos(stroke, Math.Floor((decimal)(stroke.Points.Length() / 2)), stroke.Points.Length());
            yPos = getYpos(stroke, Math.Floor((decimal)(stroke.Points.Length() / 2)), stroke.Points.Length());

            for (int i = 1; i < xPos.Count(); i++)
            {
                dx.Add(xPos[i] - xPos[i - 1]);
                dy.Add(yPos[i] - yPos[i - 1]);
            }

            for (int j = 1; j < dx.Count(); j++)
            {
                wj = (dx[j] * dx[j - 1] + dy[j] * dy[j - 1]) / ((double)Math.Sqrt(dx[j] * dx[j] + dy[j] * dy[j]) * (double)Math.Sqrt(dx[j - 1] * dx[j - 1] + dy[j - 1] * dy[j - 1]));
                angVel.Add((double)Math.Acos(wj) / (durations[j] - durations[j - 1]));
            }
            return angVel.Average();
        }

        #endregion

        #region Angle
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
            deltaX2 = Math.Abs(xPos[midIndex] - xPos[lastQuarterPos]);
            deltaY2 = Math.Abs(yPos[midIndex] - yPos[lastQuarterPos]);
            //in radians
            angle = (double)Math.Acos((deltaX1 * deltaX2 + deltaY1 * deltaY2) / (Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1) + Math.Sqrt(deltaX2 * deltaX2 + deltaY2 * deltaY2)));

            //in degrees
            angle = (double) (angle * 180 / Math.PI);
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
            //in radians
            angle = (double)Math.Acos((deltaX1 * deltaX2 + deltaY1 * deltaY2) / (Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1) + Math.Sqrt(deltaX2 * deltaX2 + deltaY2 * deltaY2)));

            //in degrees
            angle = (double)(angle * 180 / Math.PI);
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
            //in radians
            angle = (double)Math.Acos((deltaX1 * deltaX2 + deltaY1 * deltaY2) / (Math.Sqrt(deltaX1 * deltaX1 + deltaY1 * deltaY1) + Math.Sqrt(deltaX2 * deltaX2 + deltaY2 * deltaY2)));

            //in degrees
            angle = (double)(angle * 180 / Math.PI);
            return angle;
        }
        #endregion

        #region # of strokes within time period
        /// <summary>
        /// Returns a list of the number of strokes within a given time frequency (How many strokes happened in each interval of 1, 3, 5 or 10 seconds).
        /// </summary>
        /// <param name="strokeList"></param> The List of strokes to be calculated.
        /// <param name="interval"></param> The time period in which the counting is calculated (1, 3, 5 and 10).
        /// <param name="inclusivity"></param> Tells if the algorithm only counts full strokes within the time period. 
        /// <returns></returns>
        public List<double> numOfStrk(Stroke[] strokeList, int interval, bool inclusivity)
        {
            double elapsedTime = 0, firstInstant = 0;
            firstInstant = Math.Round(double.Parse(strokeList[0].Points[0].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat), 6);
            HashSet<Stroke> strokes = new HashSet<Stroke>();
            List<double> strokeCounts = new List<double>();
            for (int i = 0; i < strokeList.Count(); i++)
            {
                int numOfPoints = strokeList[i].Points.Count();
                int pointsInsideFrame = 0;
                List<Point> points = new List<Point>();
                for (int j = 0; j < strokeList[i].Points.Count(); j++)
                {
                    double currentTime = Math.Round(double.Parse(strokeList[i].Points[j].PointInfo[0][0], CultureInfo.InvariantCulture.NumberFormat), 6);
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
                                Stroke s = new Stroke(points.ToArray());
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
    }
}
