namespace StrokeParserApp
{
    partial class StrokeParserApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadData = new System.Windows.Forms.Button();
            this.createData = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fileDataGrid = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataFolderPath = new System.Windows.Forms.TextBox();
            this.numSessions = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.selectNone = new System.Windows.Forms.Button();
            this.selectAll = new System.Windows.Forms.Button();
            this.generateCSV = new System.Windows.Forms.Button();
            this.featurePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            this.SessionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strokeFiles = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.emotionFiles = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileDataGrid)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadData
            // 
            this.loadData.Location = new System.Drawing.Point(14, 13);
            this.loadData.Name = "loadData";
            this.loadData.Size = new System.Drawing.Size(101, 32);
            this.loadData.TabIndex = 0;
            this.loadData.Text = "Load Data Folder";
            this.loadData.UseVisualStyleBackColor = true;
            this.loadData.Click += new System.EventHandler(this.loadData_Click);
            // 
            // createData
            // 
            this.createData.Location = new System.Drawing.Point(14, 51);
            this.createData.Name = "createData";
            this.createData.Size = new System.Drawing.Size(101, 32);
            this.createData.TabIndex = 1;
            this.createData.Text = "Created Data";
            this.createData.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileDataGrid);
            this.groupBox1.Location = new System.Drawing.Point(14, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(834, 451);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Session Data";
            // 
            // fileDataGrid
            // 
            this.fileDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fileDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SessionID,
            this.strokeFiles,
            this.emotionFiles});
            this.fileDataGrid.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.fileDataGrid.Location = new System.Drawing.Point(7, 20);
            this.fileDataGrid.Name = "fileDataGrid";
            this.fileDataGrid.Size = new System.Drawing.Size(821, 425);
            this.fileDataGrid.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataFolderPath);
            this.groupBox2.Controls.Add(this.numSessions);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(126, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(502, 69);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Session Info";
            // 
            // dataFolderPath
            // 
            this.dataFolderPath.Location = new System.Drawing.Point(117, 17);
            this.dataFolderPath.Name = "dataFolderPath";
            this.dataFolderPath.Size = new System.Drawing.Size(379, 20);
            this.dataFolderPath.TabIndex = 3;
            // 
            // numSessions
            // 
            this.numSessions.Location = new System.Drawing.Point(117, 43);
            this.numSessions.Name = "numSessions";
            this.numSessions.Size = new System.Drawing.Size(379, 20);
            this.numSessions.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Sessions";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Folder Path";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pBar1);
            this.groupBox3.Controls.Add(this.selectNone);
            this.groupBox3.Controls.Add(this.selectAll);
            this.groupBox3.Controls.Add(this.generateCSV);
            this.groupBox3.Controls.Add(this.featurePanel);
            this.groupBox3.Location = new System.Drawing.Point(854, 90);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(176, 451);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Feature Info";
            // 
            // selectNone
            // 
            this.selectNone.Location = new System.Drawing.Point(89, 352);
            this.selectNone.Name = "selectNone";
            this.selectNone.Size = new System.Drawing.Size(54, 40);
            this.selectNone.TabIndex = 19;
            this.selectNone.Text = "Select None";
            this.selectNone.UseVisualStyleBackColor = true;
            this.selectNone.Click += new System.EventHandler(this.selectNone_Click);
            // 
            // selectAll
            // 
            this.selectAll.Location = new System.Drawing.Point(29, 352);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(54, 40);
            this.selectAll.TabIndex = 18;
            this.selectAll.Text = "Select All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // generateCSV
            // 
            this.generateCSV.Location = new System.Drawing.Point(7, 398);
            this.generateCSV.Name = "generateCSV";
            this.generateCSV.Size = new System.Drawing.Size(158, 23);
            this.generateCSV.TabIndex = 1;
            this.generateCSV.Text = "Generate Feature Vector";
            this.generateCSV.UseVisualStyleBackColor = true;
            this.generateCSV.Click += new System.EventHandler(this.generateCSV_Click);
            // 
            // featurePanel
            // 
            this.featurePanel.AutoScroll = true;
            this.featurePanel.BackColor = System.Drawing.SystemColors.Window;
            this.featurePanel.Location = new System.Drawing.Point(7, 20);
            this.featurePanel.Name = "featurePanel";
            this.featurePanel.Size = new System.Drawing.Size(158, 326);
            this.featurePanel.TabIndex = 0;
            // 
            // pBar1
            // 
            this.pBar1.Location = new System.Drawing.Point(7, 422);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(158, 23);
            this.pBar1.TabIndex = 20;
            // 
            // SessionID
            // 
            this.SessionID.HeaderText = "ID";
            this.SessionID.Name = "SessionID";
            this.SessionID.Width = 50;
            // 
            // strokeFiles
            // 
            this.strokeFiles.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.strokeFiles.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.strokeFiles.HeaderText = "Stroke Files";
            this.strokeFiles.Name = "strokeFiles";
            this.strokeFiles.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.strokeFiles.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // emotionFiles
            // 
            this.emotionFiles.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.emotionFiles.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.emotionFiles.HeaderText = "Emotion Files";
            this.emotionFiles.Name = "emotionFiles";
            this.emotionFiles.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.emotionFiles.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // StrokeParserApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 553);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.createData);
            this.Controls.Add(this.loadData);
            this.Name = "StrokeParserApp";
            this.Text = "Stroke Parser App";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileDataGrid)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadData;
        private System.Windows.Forms.Button createData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox dataFolderPath;
        private System.Windows.Forms.TextBox numSessions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView fileDataGrid;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.FlowLayoutPanel featurePanel;
        private System.Windows.Forms.Button generateCSV;
        private System.Windows.Forms.Button selectNone;
        private System.Windows.Forms.Button selectAll;
        private System.Windows.Forms.ProgressBar pBar1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SessionID;
        private System.Windows.Forms.DataGridViewComboBoxColumn strokeFiles;
        private System.Windows.Forms.DataGridViewComboBoxColumn emotionFiles;
    }
}

