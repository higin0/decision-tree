namespace DecisionTreeApp
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.featureFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.featureButton = new System.Windows.Forms.Button();
            this.labelButton = new System.Windows.Forms.Button();
            this.unselectFeatureButton = new System.Windows.Forms.Button();
            this.labelLabel = new System.Windows.Forms.Label();
            this.featureLabel = new System.Windows.Forms.Label();
            this.unselectedLabelButton = new System.Windows.Forms.Button();
            this.treeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.resultsGrid = new System.Windows.Forms.DataGridView();
            this.key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.output = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testingInfoLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.selectAll = new System.Windows.Forms.Button();
            this.selectNone = new System.Windows.Forms.Button();
            this.testingGroup = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).BeginInit();
            this.testingGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(5, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 62);
            this.button1.TabIndex = 0;
            this.button1.Text = "Training CSV";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.trainingSet_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(66, 12);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(49, 13);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = "info label";
            // 
            // featureFlow
            // 
            this.featureFlow.AutoScroll = true;
            this.featureFlow.AutoScrollMinSize = new System.Drawing.Size(1, 0);
            this.featureFlow.BackColor = System.Drawing.SystemColors.Window;
            this.featureFlow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.featureFlow.Location = new System.Drawing.Point(10, 101);
            this.featureFlow.Margin = new System.Windows.Forms.Padding(2);
            this.featureFlow.Name = "featureFlow";
            this.featureFlow.Size = new System.Drawing.Size(137, 378);
            this.featureFlow.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Features";
            // 
            // labelFlow
            // 
            this.labelFlow.AutoScroll = true;
            this.labelFlow.BackColor = System.Drawing.SystemColors.Window;
            this.labelFlow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFlow.Location = new System.Drawing.Point(150, 101);
            this.labelFlow.Margin = new System.Windows.Forms.Padding(2);
            this.labelFlow.Name = "labelFlow";
            this.labelFlow.Size = new System.Drawing.Size(137, 378);
            this.labelFlow.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(148, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Label";
            // 
            // featureButton
            // 
            this.featureButton.Location = new System.Drawing.Point(4, 542);
            this.featureButton.Margin = new System.Windows.Forms.Padding(2);
            this.featureButton.Name = "featureButton";
            this.featureButton.Size = new System.Drawing.Size(136, 19);
            this.featureButton.TabIndex = 4;
            this.featureButton.Text = "Lock Features";
            this.featureButton.UseVisualStyleBackColor = true;
            this.featureButton.Click += new System.EventHandler(this.selectFeatures_Click);
            // 
            // labelButton
            // 
            this.labelButton.Location = new System.Drawing.Point(144, 542);
            this.labelButton.Margin = new System.Windows.Forms.Padding(2);
            this.labelButton.Name = "labelButton";
            this.labelButton.Size = new System.Drawing.Size(136, 19);
            this.labelButton.TabIndex = 5;
            this.labelButton.Text = "Lock Label";
            this.labelButton.UseVisualStyleBackColor = true;
            this.labelButton.Click += new System.EventHandler(this.selectLabel_Click);
            // 
            // unselectFeatureButton
            // 
            this.unselectFeatureButton.Location = new System.Drawing.Point(4, 566);
            this.unselectFeatureButton.Margin = new System.Windows.Forms.Padding(2);
            this.unselectFeatureButton.Name = "unselectFeatureButton";
            this.unselectFeatureButton.Size = new System.Drawing.Size(136, 19);
            this.unselectFeatureButton.TabIndex = 6;
            this.unselectFeatureButton.Text = "Unlock Features";
            this.unselectFeatureButton.UseVisualStyleBackColor = true;
            this.unselectFeatureButton.Visible = false;
            this.unselectFeatureButton.Click += new System.EventHandler(this.unselectFeatures_Click);
            // 
            // labelLabel
            // 
            this.labelLabel.AutoSize = true;
            this.labelLabel.Location = new System.Drawing.Point(66, 60);
            this.labelLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new System.Drawing.Size(74, 13);
            this.labelLabel.TabIndex = 7;
            this.labelLabel.Text = "label info label";
            // 
            // featureLabel
            // 
            this.featureLabel.AutoSize = true;
            this.featureLabel.Location = new System.Drawing.Point(66, 36);
            this.featureLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.featureLabel.Name = "featureLabel";
            this.featureLabel.Size = new System.Drawing.Size(85, 13);
            this.featureLabel.TabIndex = 8;
            this.featureLabel.Text = "feature info label";
            // 
            // unselectedLabelButton
            // 
            this.unselectedLabelButton.Location = new System.Drawing.Point(144, 566);
            this.unselectedLabelButton.Margin = new System.Windows.Forms.Padding(2);
            this.unselectedLabelButton.Name = "unselectedLabelButton";
            this.unselectedLabelButton.Size = new System.Drawing.Size(136, 19);
            this.unselectedLabelButton.TabIndex = 9;
            this.unselectedLabelButton.Text = "Unlock Label";
            this.unselectedLabelButton.UseVisualStyleBackColor = true;
            this.unselectedLabelButton.Visible = false;
            this.unselectedLabelButton.Click += new System.EventHandler(this.unselectLabel_Click);
            // 
            // treeButton
            // 
            this.treeButton.Location = new System.Drawing.Point(4, 589);
            this.treeButton.Margin = new System.Windows.Forms.Padding(2);
            this.treeButton.Name = "treeButton";
            this.treeButton.Size = new System.Drawing.Size(276, 19);
            this.treeButton.TabIndex = 10;
            this.treeButton.Text = "Generate Tree";
            this.treeButton.UseVisualStyleBackColor = true;
            this.treeButton.Click += new System.EventHandler(this.treeButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 610);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "tree info";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(5, 11);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 62);
            this.button2.TabIndex = 12;
            this.button2.Text = "Testing CSV";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // resultsGrid
            // 
            this.resultsGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.resultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.key,
            this.output});
            this.resultsGrid.Location = new System.Drawing.Point(6, 101);
            this.resultsGrid.Margin = new System.Windows.Forms.Padding(2);
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.RowTemplate.Height = 24;
            this.resultsGrid.Size = new System.Drawing.Size(324, 497);
            this.resultsGrid.TabIndex = 13;
            // 
            // key
            // 
            this.key.HeaderText = "ID";
            this.key.Name = "key";
            this.key.ReadOnly = true;
            // 
            // output
            // 
            this.output.HeaderText = "Output";
            this.output.Name = "output";
            this.output.ReadOnly = true;
            // 
            // testingInfoLabel
            // 
            this.testingInfoLabel.AutoSize = true;
            this.testingInfoLabel.Location = new System.Drawing.Point(66, 12);
            this.testingInfoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.testingInfoLabel.Name = "testingInfoLabel";
            this.testingInfoLabel.Size = new System.Drawing.Size(87, 13);
            this.testingInfoLabel.TabIndex = 14;
            this.testingInfoLabel.Text = "Testing info label";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.BackColor = System.Drawing.SystemColors.Window;
            this.errorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLabel.Location = new System.Drawing.Point(6, 608);
            this.errorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(45, 18);
            this.errorLabel.TabIndex = 15;
            this.errorLabel.Text = "Error: ";
            this.errorLabel.Click += new System.EventHandler(this.errorLabel_Click);
            // 
            // selectAll
            // 
            this.selectAll.Location = new System.Drawing.Point(9, 484);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(58, 37);
            this.selectAll.TabIndex = 16;
            this.selectAll.Text = "Select All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // selectNone
            // 
            this.selectNone.Location = new System.Drawing.Point(83, 484);
            this.selectNone.Name = "selectNone";
            this.selectNone.Size = new System.Drawing.Size(57, 37);
            this.selectNone.TabIndex = 17;
            this.selectNone.Text = "Select None";
            this.selectNone.UseVisualStyleBackColor = true;
            this.selectNone.Click += new System.EventHandler(this.selectNone_Click);
            // 
            // testingGroup
            // 
            this.testingGroup.Controls.Add(this.button2);
            this.testingGroup.Controls.Add(this.testingInfoLabel);
            this.testingGroup.Controls.Add(this.resultsGrid);
            this.testingGroup.Controls.Add(this.errorLabel);
            this.testingGroup.Location = new System.Drawing.Point(304, 0);
            this.testingGroup.Name = "testingGroup";
            this.testingGroup.Size = new System.Drawing.Size(329, 632);
            this.testingGroup.TabIndex = 18;
            this.testingGroup.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelLabel);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.selectNone);
            this.groupBox1.Controls.Add(this.infoLabel);
            this.groupBox1.Controls.Add(this.selectAll);
            this.groupBox1.Controls.Add(this.labelFlow);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.featureFlow);
            this.groupBox1.Controls.Add(this.treeButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.unselectedLabelButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.unselectFeatureButton);
            this.groupBox1.Controls.Add(this.featureLabel);
            this.groupBox1.Controls.Add(this.labelButton);
            this.groupBox1.Controls.Add(this.featureButton);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 632);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 644);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.testingGroup);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).EndInit();
            this.testingGroup.ResumeLayout(false);
            this.testingGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.FlowLayoutPanel featureFlow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel labelFlow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button featureButton;
        private System.Windows.Forms.Button labelButton;
        private System.Windows.Forms.Button unselectFeatureButton;
        private System.Windows.Forms.Label labelLabel;
        private System.Windows.Forms.Label featureLabel;
        private System.Windows.Forms.Button unselectedLabelButton;
        private System.Windows.Forms.Button treeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView resultsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn key;
        private System.Windows.Forms.DataGridViewTextBoxColumn output;
        private System.Windows.Forms.Label testingInfoLabel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Button selectAll;
        private System.Windows.Forms.Button selectNone;
        private System.Windows.Forms.GroupBox testingGroup;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}