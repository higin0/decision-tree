using Accord.MachineLearning.DecisionTrees;
using decision_tree;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DecisionTreeTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<int> listFeatures = new List<int>();
        private string[][] trainingSet;
        //private string[][] testingSet;
        public int[] checkedFeatures;
        public int checkedLabel = -1;
        string filePath;
        //string testingPath;
        public DecisionTree tree;
        //double error;
        Manager manager = new Manager();
        StackPanel innerStack;

        public MainWindow()
        {
            InitializeComponent();
            var buttonAll = FindName("SelectAll") as Button;
            var buttonNone = FindName("SelectNone") as Button;
            buttonAll.Visibility = Visibility.Hidden;
            buttonNone.Visibility = Visibility.Hidden;
        }

        private void CSV_Load(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            listFeatures.Clear();
            checkedLabel = -1;

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            filePath = openFileDialog1.FileName;
                            trainingSet = manager.CreateDataSet(filePath);
                            innerStack = FindName("InnerPanel") as StackPanel;
                            if (innerStack != null)
                            {
                                for (int i = 0; i < manager.featureNames.Count(); i++)
                                {
                                    CheckBox cb = new CheckBox();
                                    cb.Name = "_" + manager.featureNames[i].ToString();
                                    cb.Content = manager.featureNames[i].ToString();
                                    innerStack.Children.Add(cb);
                                }
                                var buttonAll = FindName("SelectAll") as Button;
                                var buttonNone = FindName("SelectNone") as Button;
                                buttonAll.Visibility = Visibility.Visible;
                                buttonNone.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void FeatureSelectAll(object sender, RoutedEventArgs e)
        {
            var FeatureStack = FindName("InnerPanel");
        }

        private void FeatureSelectNone(object sender, RoutedEventArgs e)
        {

        }
    }
}
