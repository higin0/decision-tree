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
using System.Security.Permissions;

namespace DecisionTreeApp
{
    public partial class Form1 : Form
    {
        public List<int> listFeatures = new List<int>();
        public List<int> listLabels = new List<int>();
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
            treeButton.Enabled = false;
            testingGroup.Enabled = false;
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
            listLabels.Clear();
            listFeatures.Clear();
            for (int i = 0; i < featureFlow.Controls.Count; i++)
            {
                if (((CheckBox)featureFlow.Controls[i]).Checked)
                {
                    listFeatures.Add(i);
                }
                else
                {
                    listLabels.Add(i);
                }
            }

            if (listFeatures.Count > 0)
            {
                if (listFeatures.Count >= featureFlow.Controls.Count)
                {
                    featureLabel.Text = "At least 1 feature must be unselected";
                }
                else if (listFeatures.Count != featureFlow.Controls.Count)
                {
                    selectAll.Enabled = false;
                    selectNone.Enabled = false;
                    featureLabel.Text = listFeatures.Count.ToString() + " features selected";
                    checkedFeatures = listFeatures.ToArray();
                    featureFlow.Enabled = false;
                    unselectFeatureButton.Visible = true;
                    for(int j = 0; j < listLabels.Count; j++)
                    {
                        RadioButton rb_label = new System.Windows.Forms.RadioButton();
                        rb_label.Tag = featureFlow.Controls[listLabels[j]].Tag;
                        rb_label.Text = featureFlow.Controls[listLabels[j]].Text;
                        labelFlow.Controls.Add(rb_label);
                    }
                }
            }
            else if (listFeatures.Count == 0)
            {
                selectAll.Enabled = true;
                selectNone.Enabled = true;
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
                treeButton.Enabled = true;
            }
        }

        private void unselectFeatures_Click(object sender, EventArgs e)
        {
            listLabels.Clear();
            listFeatures.Clear();
            selectAll.Enabled = true;
            selectNone.Enabled = true;
            featureLabel.Text = "Features were unselected";
            unselectFeatureButton.Visible = false;
            listFeatures.Clear();
            featureFlow.Enabled = true;
        }

        private void unselectLabel_Click(object sender, EventArgs e)
        {
            labelLabel.Text = "Label was unselected";
            checkedLabel = -1;
            unselectedLabelButton.Visible = false;
            labelFlow.Enabled = true;
            treeButton.Enabled = false;
        }   

        private void treeButton_Click(object sender, EventArgs e)
        {
            if (checkedFeatures.Count() > 0 && checkedLabel > -1)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string path = saveFileDialog1.FileName;
                    string outputDirectory = Path.GetDirectoryName(saveFileDialog1.FileName);
                    FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Write, outputDirectory);
                    f2.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, outputDirectory);

                    bool completed;
                    //manager.TrainingSet(trainingSet, checkedFeatures, checkedLabel, 10);
                    tree = manager.GetTree(trainingSet, checkedFeatures, checkedLabel, path, out completed);
                    if (completed)
                    {
                        label1.Text = "Tree Generated";
                        testingGroup.Enabled = true;
                    }
                    else
                    {
                        label1.Text = "Tree not generated";
                        testingGroup.Enabled = false;
                    }
                }
            }
        }

        private string EmotionTranslator(string code)
        {
            switch (code)
            {
                case "0":
                    return "happy";
                case "1":
                    return "sad";
                case "2":
                    return "angry";
                case "3":
                    return "surprised";
                case "4":
                    return "scared";
                case "5":
                    return "disgusted";
                default:
                    return code;
            }
        }

        private string decode(string label)
        {
            List<string> reverted = new List<string>();
            var codes = manager._codebook;
            var things = codes.Columns[0].Mapping.Keys;
            for(int i = 0; i < things.Count(); i++)
            {
                reverted.Add(codes.Revert("Output", i));
            }
            reverted.ToArray();
            return reverted[Convert.ToInt32(label)];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string output = "";
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            resultsGrid.Rows.Clear();

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
                            string[] results = manager.calculate(tree, testingSet, checkedFeatures, out error);
                            for(int i = 0; i < results.Count(); i++)
                            {
                                DataGridViewRow row = (DataGridViewRow)resultsGrid.Rows[0].Clone();
                                row.Cells[0].Value = i;
                                row.Cells[1].Value = decode(results[i]);
                                output += i + "\t"+ decode(results[i]) + "\n";
                                resultsGrid.Rows.Add(row);
                            }
                            infoLabel.Text = testingPath + " is loaded";
                            errorLabel.Text = "Error: " + error.ToString();
                            
                        }
                    }
                    //File.WriteAllText(@"D:\teste\testing.txt", output);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. " + ex.Message);
                }
            }
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            if(featureFlow.HasChildren)
            {
                foreach(Control c in featureFlow.Controls)
                {
                    if(c is CheckBox)
                    {
                        ((CheckBox)c).Checked = true;
                    }
                }
            }
        }

        private void selectNone_Click(object sender, EventArgs e)
        {
            if (featureFlow.HasChildren)
            {
                foreach (Control c in featureFlow.Controls)
                {
                    if (c is CheckBox)
                    {
                        ((CheckBox)c).Checked = false;
                    }
                }
            }
        }

        private void errorLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
