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
            this.plotViewIterations = new OxyPlot.WindowsForms.PlotView();
            this.txtBox_runMaxTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBox_iterationMaxRunTimeInSec = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGetDevices
            // 
            this.btnGetDevices.Location = new System.Drawing.Point(22, 213);
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
            this.chLstBox.Size = new System.Drawing.Size(442, 158);
            this.chLstBox.TabIndex = 3;
            // 
            // btnRunTest
            // 
            this.btnRunTest.Location = new System.Drawing.Point(1168, 227);
            this.btnRunTest.Name = "btnRunTest";
            this.btnRunTest.Size = new System.Drawing.Size(144, 55);
            this.btnRunTest.TabIndex = 4;
            this.btnRunTest.Text = "Run test on selected items";
            this.btnRunTest.UseVisualStyleBackColor = true;
            this.btnRunTest.Click += new System.EventHandler(this.btnRunTest_Click);
            // 
            // txtBoxMeasurementsInfo
            // 
            this.txtBoxMeasurementsInfo.Location = new System.Drawing.Point(550, 32);
            this.txtBoxMeasurementsInfo.Name = "txtBoxMeasurementsInfo";
            this.txtBoxMeasurementsInfo.ReadOnly = true;
            this.txtBoxMeasurementsInfo.Size = new System.Drawing.Size(762, 174);
            this.txtBoxMeasurementsInfo.TabIndex = 5;
            this.txtBoxMeasurementsInfo.Text = "";
            // 
            // lblDevices
            // 
            this.lblDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDevices.AutoSize = true;
            this.lblDevices.Location = new System.Drawing.Point(22, 9);
            this.lblDevices.Name = "lblDevices";
            this.lblDevices.Size = new System.Drawing.Size(124, 20);
            this.lblDevices.TabIndex = 6;
            this.lblDevices.Text = "Available devices";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(592, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Benchmark measurements";
            // 
            // plotViewIterations
            // 
            this.plotViewIterations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotViewIterations.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.plotViewIterations.Cursor = System.Windows.Forms.Cursors.Cross;
            this.plotViewIterations.Location = new System.Drawing.Point(22, 319);
            this.plotViewIterations.Name = "plotViewIterations";
            this.plotViewIterations.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotViewIterations.Size = new System.Drawing.Size(1335, 431);
            this.plotViewIterations.TabIndex = 10;
            this.plotViewIterations.Text = "plotView";
            this.plotViewIterations.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotViewIterations.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotViewIterations.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // txtBox_runMaxTime
            // 
            this.txtBox_runMaxTime.Location = new System.Drawing.Point(757, 241);
            this.txtBox_runMaxTime.Name = "txtBox_runMaxTime";
            this.txtBox_runMaxTime.PlaceholderText = "Iteration time in secs.";
            this.txtBox_runMaxTime.Size = new System.Drawing.Size(84, 27);
            this.txtBox_runMaxTime.TabIndex = 11;
            this.txtBox_runMaxTime.Text = "60000";
            this.txtBox_runMaxTime.TextChanged += new System.EventHandler(this.txtBox_runMaxTime_TextChanged);
            this.txtBox_runMaxTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnRunTest_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(550, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Run parameters";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(550, 244);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Iteration MAX time:";
            // 
            // txtBox_iterationMaxRunTimeInSec
            // 
            this.txtBox_iterationMaxRunTimeInSec.Location = new System.Drawing.Point(847, 241);
            this.txtBox_iterationMaxRunTimeInSec.Name = "txtBox_iterationMaxRunTimeInSec";
            this.txtBox_iterationMaxRunTimeInSec.ReadOnly = true;
            this.txtBox_iterationMaxRunTimeInSec.Size = new System.Drawing.Size(79, 27);
            this.txtBox_iterationMaxRunTimeInSec.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(772, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "ML-sec";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(867, 209);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Sec";
            // 
            // PCPerformanceTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1369, 762);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBox_iterationMaxRunTimeInSec);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBox_runMaxTime);
            this.Controls.Add(this.plotViewIterations);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.txtBoxMeasurementsInfo);
            this.Controls.Add(this.btnRunTest);
            this.Controls.Add(this.chLstBox);
            this.Controls.Add(this.btnGetDevices);
            this.Name = "PCPerformanceTestForm";
            this.Text = "PCPerformanceTestForm";
            this.Load += new System.EventHandler(this.PCPerformanceTestForm_Load);
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
        private OxyPlot.WindowsForms.PlotView plotViewIterations;
        private System.Windows.Forms.TextBox txtBox_runMaxTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBox_iterationMaxRunTimeInSec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}