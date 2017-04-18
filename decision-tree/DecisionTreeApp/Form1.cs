using decision_tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.MachineLearning.DecisionTrees;


namespace DecisionTreeApp
{
    public partial class Form1 : Form
    {
        public List<int> listFeatures = new List<int>();
        private string[][] trainingSet;
        private string[][] testingSet;
        public int[] checkedFeatures;
        public int checkedLabel = -1;
        string filePath;
        string testingPath;
        public DecisionTree tree;
        double error;
        Manager manager = new Manager();

        public Form1()
        {
            InitializeComponent();
            infoLabel.Text = "CSV not loaded";
            featureLabel.Text = "";
            labelLabel.Text = "";
            label1.Text = "";
            testingInfoLabel.Text = "";
        }

        private void trainingSet_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            listFeatures.Clear();
            checkedLabel = -1;

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            filePath = openFileDialog1.FileName;
                            infoLabel.Text = filePath + " is loaded";
                            trainingSet = manager.CreateDataSet(filePath);

                            for(int i = 0; i < manager.featureNames.Count(); i++)
                            {
                                CheckBox cb_feature = new System.Windows.Forms.CheckBox();
                                cb_feature.Tag = i.ToString();
                                cb_feature.Text = manager.featureNames[i];
                                featureFlow.Controls.Add(cb_feature);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void selectFeatures_Click(object sender, EventArgs e)
        {
            labelFlow.Controls.Clear();
            for (int j = 0; j < featureFlow.Controls.Count; j++)
            {
                if (!((CheckBox) featureFlow.Controls[j]).Checked)
                {
                    RadioButton rb_label = new System.Windows.Forms.RadioButton();
                    rb_label.Tag = featureFlow.Controls[j].Tag;
                    rb_label.Text = featureFlow.Controls[j].Text;
                    labelFlow.Controls.Add(rb_label);
                }
                else
                {
                    listFeatures.Add(j);
                }
            }
            if (listFeatures.Count > 0)
            {
                featureLabel.Text = listFeatures.Count.ToString() + " features selected";
                checkedFeatures = listFeatures.ToArray();
                featureFlow.Enabled = false;
                unselectFeatureButton.Visible = true;
            }
            else if (listFeatures.Count == 0)
            {
                featureLabel.Text = "No features were selected";
            }

        }

        private void selectLabel_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < labelFlow.Controls.Count; i++)
            {
                if (((RadioButton)labelFlow.Controls[i]).Checked)
                {
                    checkedLabel = Convert.ToInt32(labelFlow.Controls[i].Tag);
                }
            }
            if (checkedLabel > 0)
            {
                labelLabel.Text = featureFlow.Controls[checkedLabel].Text + " selected as label";
                labelFlow.Enabled = false;
                unselectedLabelButton.Visible = true;
            }
        }

        private void unselectFeatures_Click(object sender, EventArgs e)
        {
            featureLabel.Text = "Features were unselected";
            listFeatures.Clear();
            featureFlow.Enabled = true;
        }

        private void unselectLabel_Click(object sender, EventArgs e)
        {
            labelLabel.Text = "Label was unselected";
            checkedLabel = -1;
            labelFlow.Enabled = true;
        }

        private void treeButton_Click(object sender, EventArgs e)
        {
            if (checkedFeatures.Count() > 0 && checkedLabel > -1)
            {
                //bool completed;
                manager.TrainingSet(trainingSet, checkedFeatures, checkedLabel, 25);
                //tree = manager.GetTree(trainingSet, checkedFeatures, checkedLabel, out completed);
                //label1.Text = completed ? "Tree Generated" : "Tree not generated";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        { 
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            testingPath = openFileDialog1.FileName;
                            infoLabel.Text = testingPath + " is loaded";
                            testingSet = manager.CreateDataSet(testingPath);
                            string[] results = manager.calculate(tree, testingSet, checkedFeatures, checkedLabel, out error);
                            for(int i = 0; i < results.Count(); i++)
                            {
                                DataGridViewRow row = (DataGridViewRow)resultsGrid.Rows[0].Clone();
                                row.Cells[0].Value = i;
                                row.Cells[1].Value = results[i];
                                resultsGrid.Rows.Add(row);
                            }
                            infoLabel.Text = testingPath + " is loaded";
                            errorLabel.Text = "Error: " + error.ToString();
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
