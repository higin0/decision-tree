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
            xMean(strokes);
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

        private List<float> getXpos(Stroke stroke)
        {
            List<float> xPos = new List<float>();

            for (int i = 0; i < stroke._inputs.Length(); i++)
            {
                xPos.Add(Convert.ToInt32(stroke._inputs[i]._point[0][4], 16));
            }
            return xPos;
        }

        private List<float> getYpos(Stroke stroke)
        {
            List<float> yPos = new List<float>();

            for (int i = 0; i < stroke._inputs.Length(); i++)
            {
                yPos.Add(Convert.ToInt32(stroke._inputs[i]._point[1][4], 16));
            }
            return yPos;
        }

        private List<float> getPressures(Stroke stroke)
        {
            List<float> pressures = new List<float>();

            for (int i = 0; i < stroke._inputs.Length(); i++)
            {
                pressures.Add(Convert.ToInt32(stroke._inputs[i]._point[2][4], 16));
            }
            return pressures;
        }

        #region Time - F1, F2, F3
        //F2 - Duration = subtracting the first timestamp of the current stroke, from the last timestamp of the current stroke.
        public List<float> duration(Stroke[] strokeArray)
        {
            List<float> durations = new List<float>();

            for (int i = 0; i < strokeArray.Length(); i++)
            {
                int numberOfPoints = strokeArray[i]._inputs.Length();
                float lastInstant = float.Parse(strokeArray[i]._inputs[numberOfPoints - 1]._point[0][0], CultureInfo.InvariantCulture.NumberFormat);
                float firstInstant = float.Parse(strokeArray[i]._inputs[0]._point[0][0], CultureInfo.InvariantCulture.NumberFormat);
                float duration = lastInstant - firstInstant;
                durations.Add(duration);
            }
            return durations;
        }

        //F3 - Dist2Prev - subtracting the initial timestamp of the current stroke, from the initial timestamp of the previous stroke.
        public List<float> dist2prev(Stroke[] strokeArray)
        {
            List<float> durations = new List<float>();
            for (int i = 1; i < strokeArray.Length(); i++)
            {
                float currentStrokeTime = float.Parse(strokeArray[i]._inputs[0]._point[0][0], CultureInfo.InvariantCulture.NumberFormat);
                float previousStrokeTime = float.Parse(strokeArray[i - 1]._inputs[0]._point[0][0], CultureInfo.InvariantCulture.NumberFormat);
                float duration = currentStrokeTime - previousStrokeTime;
                durations.Add(duration);
            }
            return durations;
        }

        #endregion

        #region Location - F4, F5, F6, F7, F8, F9
        //F4 - xMin - Min recorded X
        public List<float> xMin(Stroke[] strokeArray)
        {
            List<float> xMins = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                xMins.Add(getXpos(s).Min());
            }
            return xMins;
        }

        //F4 - xMax - Max recorded X
        public List<float> xMax(Stroke[] strokeArray)
        {
            List<float> xMaxs = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                xMaxs.Add(getXpos(s).Max());
            }
            return xMaxs;
        }

        //F6 - xMean - for these two features, values of min, max and mean should be calculated.
        public List<float> xMean(Stroke[] strokeArray)
        {
            List<float> xMeans = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                xMeans.Add(getXpos(s).Average());
            }
            return xMeans;
        }

        //F7 - yMin - Min recorded Y
        public List<float> yMin(Stroke[] strokeArray)
        {
            List<float> yMins = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                yMins.Add(getYpos(s).Min());
            }
            return yMins;
        }

        //F8 - yMax - Max recorded Y
        public List<float> yMax(Stroke[] strokeArray)
        {
            List<float> yMaxs = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                yMaxs.Add(getYpos(s).Max());
            }
            return yMaxs;
        }

        //F9 - yMean - for these two features, values of min, max and mean should be calculated.
        public List<float> yMean(Stroke[] strokeArray)
        {
            List<float> yMeans = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                yMeans.Add(getYpos(s).Average());
            }
            return yMeans;
        }
        #endregion

        #region Pressure - F10, F11, F12
        //F10 - pMin - Min recorded Pressure
        public List<float> pMin(Stroke[] strokeArray)
        {
            List<float> pMins = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                pMins.Add(getPressures(s).Min());
            }
            return pMins;
        }

        //F11 - pMax - Max recorded Pressure
        public List<float> pMax(Stroke[] strokeArray)
        {
            List<float> pMaxs = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                pMaxs.Add(getPressures(s).Max());
            }
            return pMaxs;
        }

        //F12 - pMean
        public List<float> pMean(Stroke[] strokeArray)
        {
            List<float> pMeans = new List<float>();
            foreach (Stroke s in strokeArray)
            {
                pMeans.Add(getPressures(s).Average());
            }
            return pMeans;
        }
        #endregion

        #region Positional Changes - F13, F14, F15, F16, F17, F18 
        //F13 - LenghtT - sum of pythagorean propositions
        public List<float> lengthT(Stroke[] strokeArray)
        {
            List<float> xPos = new List<float>();
            List<float> yPos = new List<float>();
            List<float> distances = new List<float>();
            float traveledPath = 0f;

            foreach (Stroke s in strokeArray)
            {
                xPos = getXpos(s);
                yPos = getYpos(s);
                for (int i = 1; i < xPos.Count(); i++)
                {
                    traveledPath += (float)Math.Sqrt(Math.Pow(yPos[i] - yPos[i - 1], 2) + Math.Pow(xPos[i] - xPos[i - 1], 2));
                }
                distances.Add(traveledPath);
            }
            return distances;
        }

        //F14 - Relocation in X - xMax-Xmin;
        public List<float> relocationX(Stroke[] strokeArray)
        {
            List<float> xPos = new List<float>();
            List<float> relocations = new List<float>();

            foreach(Stroke s in strokeArray) {
                xPos = getXpos(s);
                relocations.Add(xPos.Max() - xPos.Min());
            }
            return relocations;
        }

        //F15 - Relocation in Y - yMax-yMin;
        public List<float> relocationY(Stroke[] strokeArray)
        {
            List<float> yPos = new List<float>();
            List<float> relocations = new List<float>();

            foreach (Stroke s in strokeArray)
            {
                yPos = getYpos(s);
                relocations.Add(yPos.Max() - yPos.Min());
            }
            return relocations;
        }

        //F16 - DistanceX - difference of X position from start to the end
        public List<float> distanceX(Stroke[] strokeArray)
        {
            List<float> xPos = new List<float>();
            List<float> xDistances = new List<float>();

            foreach (Stroke s in strokeArray)
            {
                xPos = getXpos(s);
                xDistances.Add(xPos.Last() - xPos.First());
            }
            return xDistances;
        }

        //F17 - DistanceY - difference of Y position from start to the end
        public List<float> distanceY(Stroke[] strokeArray)
        {
            List<float> yPos = new List<float>();
            List<float> yDistances = new List<float>();

            foreach (Stroke s in strokeArray)
            {
                yPos = getYpos(s);
                yDistances.Add(yPos.Last() - yPos.First());
            }
            return yDistances;
        }

        //F18- Distance - total displacement
        public List<float> distance(Stroke[] strokeArray)
        {
            List<float> xPos = new List<float>();
            List<float> yPos = new List<float>();
            List<float> distances = new List<float>();

            foreach (Stroke s in strokeArray)
            {
                xPos = getXpos(s);
                yPos = getYpos(s);
                distances.Add((float)Math.Sqrt(Math.Pow(yPos.Last() - yPos.First(), 2) + Math.Pow(xPos.Last() - xPos.First(), 2)));
            }
            return distances;
        }
        #endregion

        #region Posicional Changes First Derivation
        //F19 - averageSpeedX - first derivation of X changes
        public List<float> averageSpeedX(Stroke[] strokeArray)
        {
            List<float> xPos = new List<float>();
            List<float> durations = duration(strokeArray);
            List<float> averageSpeeds = new List<float>();
            for(int i = 0; i < strokeArray.Length(); i++)
            {
                xPos = getXpos(strokeArray[i]);
                float positionChange = 0f, positionChangeSum = 0f;

                for(int j = 1; j < xPos.Count(); j++)
                {
                    positionChange = xPos[j] - xPos[j - 1];
                    positionChangeSum += positionChange;
                }
                averageSpeeds.Add(positionChangeSum / durations[i]);
            }
            return averageSpeeds;
        }

        //F20 - averageSpeedY - first derivation of Y changes
        public List<float> averageSpeedY(Stroke[] strokeArray)
        {
            List<float> yPos = new List<float>();
            List<float> durations = duration(strokeArray);
            List<float> averageSpeeds = new List<float>();
            for (int i = 0; i < strokeArray.Length(); i++)
            {
                yPos = getYpos(strokeArray[i]);
                float positionChange = 0f, positionChangeSum = 0f;

                for (int j = 1; j < yPos.Count(); j++)
                {
                    positionChange = yPos[j] - yPos[j - 1];
                    positionChangeSum += positionChange;
                }
                averageSpeeds.Add(positionChangeSum / durations[i]);
            }
            return averageSpeeds;
        }

        //F21 - averageSpeed - first derivation of the diameter of the rectangular span
        public List<float> averageSpeed(Stroke[] strokeArray)
        {
            List<float> xPos = new List<float>();
            List<float> yPos = new List<float>();
            List<float> durations = duration(strokeArray);
            List<float> averageSpeeds = new List<float>();
            for (int i = 0; i < strokeArray.Length(); i++)
            {
                xPos = getXpos(strokeArray[i]);
                yPos = getYpos(strokeArray[i]);
                float positionChangeX = 0f, positionChangeY = 0f, positionChangeSum = 0f;

                for (int j = 1; j < yPos.Count(); j++)
                {
                    positionChangeY = xPos[j] - xPos[j - 1];
                    positionChangeY = yPos[j] - yPos[j - 1];
                    positionChangeSum += (float) Math.Pow(positionChangeX, 2) + (float) Math.Pow(positionChangeY, 2);
                }
                averageSpeeds.Add(positionChangeSum / durations[i]);
            }
            return averageSpeeds;
        }

        #endregion
    }
}
