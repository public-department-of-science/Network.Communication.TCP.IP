namespace TCP.IP.Client
{
    partial class PCPerformanceTestForm
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
            this.btnGetDevices = new System.Windows.Forms.Button();
            this.chLstBox = new System.Windows.Forms.CheckedListBox();
            this.btnRunTest = new System.Windows.Forms.Button();
            this.txtBoxMeasurementsInfo = new System.Windows.Forms.RichTextBox();
            this.lblDevices = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGetDevices
            // 
            this.btnGetDevices.Location = new System.Drawing.Point(22, 245);
            this.btnGetDevices.Name = "btnGetDevices";
            this.btnGetDevices.Size = new System.Drawing.Size(144, 55);
            this.btnGetDevices.TabIndex = 0;
            this.btnGetDevices.Text = "Get devices list";
            this.btnGetDevices.UseVisualStyleBackColor = true;
            this.btnGetDevices.Click += new System.EventHandler(this.btnGetDevices_Click);
            // 
            // chLstBox
            // 
            this.chLstBox.Enabled = false;
            this.chLstBox.FormattingEnabled = true;
            this.chLstBox.Location = new System.Drawing.Point(22, 32);
            this.chLstBox.Name = "chLstBox";
            this.chLstBox.ScrollAlwaysVisible = true;
            this.chLstBox.Size = new System.Drawing.Size(498, 180);
            this.chLstBox.TabIndex = 3;
            // 
            // btnRunTest
            // 
            this.btnRunTest.Location = new System.Drawing.Point(202, 245);
            this.btnRunTest.Name = "btnRunTest";
            this.btnRunTest.Size = new System.Drawing.Size(144, 55);
            this.btnRunTest.TabIndex = 4;
            this.btnRunTest.Text = "Run test on selected items";
            this.btnRunTest.UseVisualStyleBackColor = true;
            this.btnRunTest.Click += new System.EventHandler(this.btnRunTest_Click);
            // 
            // txtBoxMeasurementsInfo
            // 
            this.txtBoxMeasurementsInfo.Location = new System.Drawing.Point(592, 32);
            this.txtBoxMeasurementsInfo.Name = "txtBoxMeasurementsInfo";
            this.txtBoxMeasurementsInfo.ReadOnly = true;
            this.txtBoxMeasurementsInfo.Size = new System.Drawing.Size(561, 215);
            this.txtBoxMeasurementsInfo.TabIndex = 5;
            this.txtBoxMeasurementsInfo.Text = "";
            // 
            // lblDevices
            // 
            this.lblDevices.AutoSize = true;
            this.lblDevices.Location = new System.Drawing.Point(22, 9);
            this.lblDevices.Name = "lblDevices";
            this.lblDevices.Size = new System.Drawing.Size(124, 20);
            this.lblDevices.TabIndex = 6;
            this.lblDevices.Text = "Available devices";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(592, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Benchmark measurements";
            // 
            // PCPerformanceTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.txtBoxMeasurementsInfo);
            this.Controls.Add(this.btnRunTest);
            this.Controls.Add(this.chLstBox);
            this.Controls.Add(this.btnGetDevices);
            this.Name = "PCPerformanceTestForm";
            this.Text = "PCPerformanceTestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetDevices;
        private System.Windows.Forms.CheckedListBox chLstBox;
        private System.Windows.Forms.Button btnRunTest;
        private System.Windows.Forms.RichTextBox txtBoxMeasurementsInfo;
        private System.Windows.Forms.Label lblDevices;
        private System.Windows.Forms.Label label1;
    }
}