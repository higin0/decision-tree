using System;
using System.Linq;
using Accord.Math;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.MachineLearning.DecisionTrees;
using Accord.Statistics.Filters;
using Accord.Math.Optimization.Losses;
using Accord.MachineLearning.DecisionTrees.Rules;
using System.IO;
using System.Collections.Generic;

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
        public DecisionTree tree { get; set; }
        public DecisionSet rules { get; set; }

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

        public Manager()
        {
        }

        public string[][] CreateDataSet(string filePath)
        {
            var file = File.ReadAllText(filePath);
            var dataSet = file.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Apply(x => x.Split(';'));
            dataSet = SeparateLabelsFromDataset(dataSet, out featureLabels);
            featureNames = featureLabels;
            return dataSet;
        }

        public void TrainingSet(string[][] dataSet, int[] featureIndexes, int labelIndex, float percentage)
        {
            List<double> errors = new List<double>();
            double error;
            int entries = dataSet.GetLength(0);
            int testingSetLenght = (int) (entries * (percentage/100));
            for(int i = 0; i < (int) (100/percentage); i++)
            {
                var testingSet = dataSet.Skip(testingSetLenght * i).Take(testingSetLenght).ToArray();
                var trainingSet = dataSet.Except(testingSet).ToArray();

                var tree = GetTree(trainingSet, featureIndexes, labelIndex);

                var test = calculate(tree, testingSet, featureIndexes, labelIndex, out error);
                errors.Add(error);
            }
        }

        public DecisionTree GetTree(string[][] dataSet, int[] featureIndexes, int labelIndex)
        {
            var _inputs = GetInputs(dataSet, featureIndexes);
            var _labels = GetLabels(dataSet, labelIndex);

            //creating the codebook with labels and converting them to integer representations
            _codebook = new Codification("Output", _labels);
            _outputs = _codebook.Translate("Output", _labels);

            DecisionVariable[] features = GetDecisionVariables(featureIndexes, featureLabels);
            //how many distinc values are there in the label column
            int classNumbers = _labels.Distinct().Length;

            tree = new DecisionTree(inputs: features, classes: classNumbers);

            var teacher = new C45Learning(tree);

            teacher.Learn(_inputs, _outputs);

            return tree;
        }

        public DecisionTree GetTree(string[][] dataSet, int[] featureIndexes, int labelIndex, string path, out bool completed)
        {
            var _inputs = GetInputs(dataSet, featureIndexes);
            _labels = GetLabels(dataSet, labelIndex);

            //creating the codebook with labels and converting them to integer representations
            _codebook = new Codification("Output", _labels);
            _outputs = _codebook.Translate("Output", _labels);

            DecisionVariable[] features = GetDecisionVariables(featureIndexes, featureLabels);
            //how many distinc values are there in the label column
            int classNumbers = _labels.Distinct().Length;

            tree = new DecisionTree(inputs: features, classes: classNumbers);

            var teacher = new C45Learning(tree);

            teacher.Learn(_inputs, _outputs);

            completed = true;

            // Moreover, we may decide to convert our tree to a set of rules:
            rules = tree.ToRules();
            // And using the codebook, we can inspect the tree reasoning:
            string ruleText = rules.ToString(_codebook, "Output", System.Globalization.CultureInfo.InvariantCulture);
            File.WriteAllText(path, ruleText);
            return tree;
        }

        public string[] calculate(DecisionTree tree, string[][] testingSet, int[] checkedFeatures, out double error)
        {
            var inputs = GetInputs(testingSet, checkedFeatures);
            Console.WriteLine();
            int[] predicted = tree.Decide(inputs);
            string[] result = predicted.Select(z => z.ToString()).ToArray();
            //File.WriteAllLines(@"C:\Users\higin\Desktop\predictions.txt", result);


            //creating the codebook with labels and converting them to integer representations
            var codebook = new Codification("Output", _labels);
            var outputs = codebook.Translate("Output", _labels);

            error = new ZeroOneLoss(tree.Decide(inputs)).Loss(outputs);
            //Console.WriteLine("Error of: " + error);

            // Moreover, we may decide to convert our tree to a set of rules:
            rules = tree.ToRules();
            // And using the codebook, we can inspect the tree reasoning:
            //string ruleText = rules.ToString(codebook, "Output", System.Globalization.CultureInfo.InvariantCulture);
            //File.WriteAllText(@"F:\teste\rules.txt", ruleText);
            return result;
        }

        public string[] calculate(DecisionTree tree, string[][] testingSet, int[] checkedFeatures, int labelIndex, out double error)
        {
            var inputs = GetInputs(testingSet, checkedFeatures);

            int[] predicted = tree.Decide(inputs);
            string[] result = predicted.Select(z => z.ToString()).ToArray();
            //File.WriteAllLines(@"C:\Users\higino\Desktop\predictions.txt", result);


            //creating the codebook with labels and converting them to integer representations
            var codebook = new Codification("Output", _labels);
            var outputs = codebook.Translate("Output", _labels);

            error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));
            //Console.WriteLine("Error of: " + error);

            // Moreover, we may decide to convert our tree to a set of rules:
            //DecisionSet rules = tree.ToRules();

            // And using the codebook, we can inspect the tree reasoning:
            //string ruleText = rules.ToString(codebook, "Output", System.Globalization.CultureInfo.InvariantCulture);
            //File.WriteAllText(@"C:\Users\higin\Desktop\log.txt", ruleText);
            return result;
        }
    }


}
