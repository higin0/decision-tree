using System;
using System.Linq;
using Accord.Math;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.MachineLearning.DecisionTrees;
using Accord.Statistics.Filters;
using Accord.Math.Optimization.Losses;
using Accord.MachineLearning.DecisionTrees.Rules;
using System.IO;

namespace decision_tree
{
    public class Manager
    {
        public string[] _labels { get; set; }
        public string[][] _dataSet { get; set; }
        public string[][] stringInputs { get; set; }
        public double[][] _inputs { get; set; }
        public string[] featureNames { get; set; }
        private string[] featureLabels;
        public int[] featureIndexes { get; set; }
        public int labelIndex { get; set; }
        public int[] _outputs { get; set; }
        public Codification _codebook { get; set; }

        //public int[] featureIndexes = new int[27] {3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,32};
        //public int labelIndex = 33;
        string file;

        public string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public string[][] GetDataSet (string file)
        {
            return file.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Apply(x => x.Split(';'));
        }

        public T[][] SeparateLabelsFromDataset<T>(T[][] array2d, out T[] firstRow)
        {
            T[][] temp = new T[array2d.GetLength(0) - 1][];
            for (int i = 1; i < array2d.GetLength(0); i++)
            {
                temp[i - 1] = new T[array2d[i].Length];
                for (int j = 0; j < array2d[i].Length; j++)
                {
                    temp[i - 1][j] = array2d[i][j];
                }
            }
            firstRow = array2d[0];
            return temp;
        }

        public double[][] GetInputs(string[][] dataset, int[] inputIndexes)
        {
            return dataset.GetColumns(inputIndexes).To<double[][]>();
        }

        public string[] GetLabels(string[][] dataset, int labelIndex)
        {
            return dataset.GetColumn(labelIndex);
        }

        public DecisionVariable[] GetDecisionVariables(int[] variableIndexes, string[] variables)
        {
            DecisionVariable[] features = new DecisionVariable[variableIndexes.Length];

            for (int i = 0; i < variables.Length; i++)
            {
                for (int j = 0; j < variableIndexes.Length; j++)
                {
                    if (i == variableIndexes[j])
                    {
                        features[j] = new DecisionVariable(variables[i], DecisionVariableKind.Continuous);
                    }
                }
            }
            return features;
        }

        public Manager(string filePath)
        {
            file = ReadFile(filePath);

            _dataSet = GetDataSet(file);
            _dataSet = SeparateLabelsFromDataset(_dataSet, out featureLabels);
            featureNames = featureLabels;
        }

        public DecisionTree GetTree(int[] featureIndexes, int labelIndex)
        {
            _inputs = GetInputs(_dataSet, featureIndexes);
            _labels = GetLabels(_dataSet, labelIndex);

            //creating the codebook with labels and converting them to integer representations
            _codebook = new Codification("Output", _labels);
            _outputs = _codebook.Translate("Output", _labels);

            DecisionVariable[] features = GetDecisionVariables(featureIndexes, featureLabels);

            var tree = new DecisionTree(inputs: features, classes: 6);

            var teacher = new C45Learning(tree);

            teacher.Learn(_inputs, _outputs);

            return tree;
        }

        public string calculate(DecisionTree tree)
        {
            int[] predicted = tree.Decide(_inputs);
            string[] result = predicted.Select(z => z.ToString()).ToArray();
            File.WriteAllLines(@"C:\Users\higin\Desktop\predictions.txt", result);

            double error = new ZeroOneLoss(_outputs).Loss(tree.Decide(_inputs));
            Console.WriteLine("Error of: " + error);

            // Moreover, we may decide to convert our tree to a set of rules:
            DecisionSet rules = tree.ToRules();

            // And using the codebook, we can inspect the tree reasoning:
            string ruleText = rules.ToString(_codebook, "Output", System.Globalization.CultureInfo.InvariantCulture);
            File.WriteAllText(@"C:\Users\higin\Desktop\log.txt", ruleText);

            return "completed";
        }
    }


}
