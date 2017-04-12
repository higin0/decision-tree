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
        public int[] checkedFeatures;
        public int checkedLabel;
        string filePath;
        public DecisionTree tree;
        Manager manager;

        public Form1()
        {
            InitializeComponent();
            label1.Text = "CSV not loaded";
        }

        private void button1_Click(object sender, EventArgs e)
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
                            filePath = openFileDialog1.FileName;
                            label1.Text = filePath + " is loaded";
                            manager = new Manager(filePath);

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

        private void button2_Click(object sender, EventArgs e)
        {
            labelFlow.Controls.Clear();
            label1.Text = "Features selected";
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
            checkedFeatures = listFeatures.ToArray();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < labelFlow.Controls.Count; i++)
            {
                if (((RadioButton)labelFlow.Controls[i]).Checked)
                {
                    checkedLabel = Convert.ToInt32(labelFlow.Controls[i].Tag);
                }
            }
            tree = manager.GetTree(checkedFeatures, checkedLabel);
        }
    }
}
