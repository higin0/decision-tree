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
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 76);
            this.button1.TabIndex = 0;
            this.button1.Text = "Training CSV";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.trainingSet_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(94, 12);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(65, 17);
            this.infoLabel.TabIndex = 1;
            this.infoLabel.Text = "info label";
            // 
            // featureFlow
            // 
            this.featureFlow.AutoScroll = true;
            this.featureFlow.AutoScrollMinSize = new System.Drawing.Size(1, 0);
            this.featureFlow.BackColor = System.Drawing.SystemColors.Window;
            this.featureFlow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.featureFlow.Location = new System.Drawing.Point(12, 111);
            this.featureFlow.Name = "featureFlow";
            this.featureFlow.Size = new System.Drawing.Size(181, 286);
            this.featureFlow.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Features";
            // 
            // labelFlow
            // 
            this.labelFlow.AutoScroll = true;
            this.labelFlow.BackColor = System.Drawing.SystemColors.Window;
            this.labelFlow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFlow.Location = new System.Drawing.Point(199, 111);
            this.labelFlow.Name = "labelFlow";
            this.labelFlow.Size = new System.Drawing.Size(181, 286);
            this.labelFlow.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Label";
            // 
            // featureButton
            // 
            this.featureButton.Location = new System.Drawing.Point(12, 403);
            this.featureButton.Name = "featureButton";
            this.featureButton.Size = new System.Drawing.Size(181, 23);
            this.featureButton.TabIndex = 4;
            this.featureButton.Text = "Select Features";
            this.featureButton.UseVisualStyleBackColor = true;
            this.featureButton.Click += new System.EventHandler(this.selectFeatures_Click);
            // 
            // labelButton
            // 
            this.labelButton.Location = new System.Drawing.Point(199, 403);
            this.labelButton.Name = "labelButton";
            this.labelButton.Size = new System.Drawing.Size(181, 23);
            this.labelButton.TabIndex = 5;
            this.labelButton.Text = "Select Label";
            this.labelButton.UseVisualStyleBackColor = true;
            this.labelButton.Click += new System.EventHandler(this.selectLabel_Click);
            // 
            // unselectFeatureButton
            // 
            this.unselectFeatureButton.Location = new System.Drawing.Point(12, 432);
            this.unselectFeatureButton.Name = "unselectFeatureButton";
            this.unselectFeatureButton.Size = new System.Drawing.Size(181, 23);
            this.unselectFeatureButton.TabIndex = 6;
            this.unselectFeatureButton.Text = "Unselect Features";
            this.unselectFeatureButton.UseVisualStyleBackColor = true;
            this.unselectFeatureButton.Visible = false;
            this.unselectFeatureButton.Click += new System.EventHandler(this.unselectFeatures_Click);
            // 
            // labelLabel
            // 
            this.labelLabel.AutoSize = true;
            this.labelLabel.Location = new System.Drawing.Point(94, 71);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new System.Drawing.Size(99, 17);
            this.labelLabel.TabIndex = 7;
            this.labelLabel.Text = "label info label";
            // 
            // featureLabel
            // 
            this.featureLabel.AutoSize = true;
            this.featureLabel.Location = new System.Drawing.Point(94, 42);
            this.featureLabel.Name = "featureLabel";
            this.featureLabel.Size = new System.Drawing.Size(114, 17);
            this.featureLabel.TabIndex = 8;
            this.featureLabel.Text = "feature info label";
            // 
            // unselectedLabelButton
            // 
            this.unselectedLabelButton.Location = new System.Drawing.Point(199, 432);
            this.unselectedLabelButton.Name = "unselectedLabelButton";
            this.unselectedLabelButton.Size = new System.Drawing.Size(181, 23);
            this.unselectedLabelButton.TabIndex = 9;
            this.unselectedLabelButton.Text = "Unselect Label";
            this.unselectedLabelButton.UseVisualStyleBackColor = true;
            this.unselectedLabelButton.Visible = false;
            this.unselectedLabelButton.Click += new System.EventHandler(this.unselectLabel_Click);
            // 
            // treeButton
            // 
            this.treeButton.Location = new System.Drawing.Point(12, 461);
            this.treeButton.Name = "treeButton";
            this.treeButton.Size = new System.Drawing.Size(368, 23);
            this.treeButton.TabIndex = 10;
            this.treeButton.Text = "Generate Tree";
            this.treeButton.UseVisualStyleBackColor = true;
            this.treeButton.Click += new System.EventHandler(this.treeButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(173, 487);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "tree info";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(413, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 76);
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
            this.resultsGrid.Location = new System.Drawing.Point(413, 111);
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.RowTemplate.Height = 24;
            this.resultsGrid.Size = new System.Drawing.Size(324, 286);
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
            this.testingInfoLabel.Location = new System.Drawing.Point(496, 13);
            this.testingInfoLabel.Name = "testingInfoLabel";
            this.testingInfoLabel.Size = new System.Drawing.Size(116, 17);
            this.testingInfoLabel.TabIndex = 14;
            this.testingInfoLabel.Text = "Testing info label";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.BackColor = System.Drawing.SystemColors.Window;
            this.errorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.errorLabel.Location = new System.Drawing.Point(413, 403);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(50, 19);
            this.errorLabel.TabIndex = 15;
            this.errorLabel.Text = "Error: ";
            this.errorLabel.Click += new System.EventHandler(this.label4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 559);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.testingInfoLabel);
            this.Controls.Add(this.resultsGrid);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeButton);
            this.Controls.Add(this.unselectedLabelButton);
            this.Controls.Add(this.featureLabel);
            this.Controls.Add(this.labelLabel);
            this.Controls.Add(this.unselectFeatureButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelButton);
            this.Controls.Add(this.featureButton);
            this.Controls.Add(this.featureFlow);
            this.Controls.Add(this.labelFlow);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.resultsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}