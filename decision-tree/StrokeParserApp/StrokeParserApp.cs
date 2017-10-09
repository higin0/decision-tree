using StrokeParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrokeParserApp
{
    public partial class StrokeParserApp : Form
    {
        public List<int> featureList = new List<int>();
        public List<string> strokeFilesList = new List<string>();
        public List<string> expressionFilesList = new List<string>();
        private string output = "";
        public string dataPath;
        public string[] folders;
        private string[] features = new string[] { "Duration", "Dist2prev", "timeElappsed", "xMin", "xMax", "xMean", "xMedian", "xSTD", "yMin", "yMax", "yMean", "yMedian", "ySTD", "pMin", "pMax", "pMean", "pMedian", "pSTD", "lengthT", "spanX", "spanY", "distanceX", "distanceY", "displacement", "1stDVPCx", "1stDVPCy", "1stDVPP", "2ndDVPCx", "2ndDVPCy", "2ndDVPP", "velocity", "acceleration", "wj_Mean", "wj_Min", "wj_Max", "curlX", "curlY", "1stDVcurlx", "1stDVcurly", "angleM", "angle1", "angle2", "numOfStrk1", "numOfStrk3", "numOfStrk5", "numOfStrk10", "Area", "Valence", "Arousal", "Emotion" };
        public StrokeParserApp()
        {
            folders = null;
            InitializeComponent();
            createData.Enabled = false;
            featurePanel.Enabled = false;
            selectAll.Enabled = false;
            selectNone.Enabled = false;
            pBar1.Enabled = false;
            generateCSV.Enabled = false;
            for (int i = 0; i < features.Count(); i++)
            {
                CheckBox cb_feature = new System.Windows.Forms.CheckBox();
                cb_feature.Tag = i.ToString();
                cb_feature.Text = features[i];
                featurePanel.Controls.Add(cb_feature);
            }

        }

        private void loadData_Click(object sender, EventArgs e)
        {
            featurePanel.Enabled = true;
            selectNone.Enabled = true;
            selectAll.Enabled = true;
            pBar1.Enabled = true;
            generateCSV.Enabled = true;
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dataPath = folderBrowserDialog1.SelectedPath;
                    dataFolderPath.Text = folderBrowserDialog1.SelectedPath;
                    folders = System.IO.Directory.GetDirectories(dataPath, "*", System.IO.SearchOption.AllDirectories);
                    numSessions.Text = folders.Length.ToString();
                    for (int i = 0; i < folders.Count(); i++)
                    {
                        string[] strokeFiles = System.IO.Directory.GetFiles(folders[i], "*.txt").Concat(new string[] { "no stroke data" }).ToArray();
                        string[] expressionFiles = System.IO.Directory.GetFiles(folders[i], "*.csv").Concat(System.IO.Directory.GetFiles(folders[i], "*.txt")).Concat(new string[] { "no emotion data" }).ToArray();
                        DataGridViewRow row = (DataGridViewRow) fileDataGrid.Rows[0].Clone();

                        ((DataGridViewTextBoxCell) row.Cells[0]).Value = i+1;
                        ((DataGridViewComboBoxCell) row.Cells[1]).DataSource = (strokeFiles.Count() > 0) ? strokeFiles : null;
                        ((DataGridViewComboBoxCell)row.Cells[1]).Value = (strokeFiles.Count() > 0) ? strokeFiles[0] : null;
                        ((DataGridViewComboBoxCell) row.Cells[2]).DataSource = (expressionFiles.Count() > 0) ? expressionFiles : null;
                        ((DataGridViewComboBoxCell)row.Cells[2]).Value = (expressionFiles.Count() > 0) ? expressionFiles[0] : null;
                        fileDataGrid.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                   MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            if (featurePanel.HasChildren)
            {
                foreach (Control c in featurePanel.Controls)
                {
                    if (c is CheckBox)
                    {
                        ((CheckBox)c).Checked = true;
                    }
                }
            }
        }

        private void selectNone_Click(object sender, EventArgs e)
        {
            if (featurePanel.HasChildren)
            {
                foreach (Control c in featurePanel.Controls)
                {
                    if (c is CheckBox)
                    {
                        ((CheckBox)c).Checked = false;
                    }
                }
            }
        }

        private void generateCSV_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < featurePanel.Controls.Count; i++)
            {
                if (((CheckBox)featurePanel.Controls[i]).Checked)
                {
                    featureList.Add(i);
                    output += features[i] +";";
                }
            }
            output += "\n";
            foreach (DataGridViewRow row in fileDataGrid.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[2].Value != null)
                {
                    if (row.Cells[1].Value.ToString()[1] == ':' && row.Cells[2].Value.ToString()[1] == ':')
                    {
                        strokeFilesList.Add(row.Cells[1].Value.ToString());
                        expressionFilesList.Add(row.Cells[2].Value.ToString());
                    }
                }
            }

            // Display the ProgressBar control.
            pBar1.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            pBar1.Minimum = 1;
            // Set Maximum to the total number of files to copy.
            pBar1.Maximum = strokeFilesList.Count;
            // Set the initial value of the ProgressBar.
            pBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            pBar1.Step = 1;

            for (int i = 0; i < strokeFilesList.Count(); i++)
            {
                Parser strokes = new Parser(strokeFilesList[i], expressionFilesList[i]);
                var tempOutput = strokes.GetResults(featureList, i);
                output += strokes.ConvertToCSV(tempOutput, '\t');
                pBar1.PerformStep();
            }

            WriteToFile();
        }

        private void WriteToFile()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string outputPath = saveFileDialog1.FileName;
                string outputDirectory = Path.GetDirectoryName(saveFileDialog1.FileName);
                FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Write, outputDirectory);
                f2.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, outputDirectory);
                File.WriteAllText(outputPath, output);
            }
        }

    }
}
