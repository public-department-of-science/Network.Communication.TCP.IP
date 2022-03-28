
namespace MultiThreadingDrawSample
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnThread1 = new System.Windows.Forms.Button();
            this.btnThread2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnThread1
            // 
            this.btnThread1.Location = new System.Drawing.Point(44, 27);
            this.btnThread1.Name = "btnThread1";
            this.btnThread1.Size = new System.Drawing.Size(94, 29);
            this.btnThread1.TabIndex = 0;
            this.btnThread1.Text = "Thread 1";
            this.btnThread1.UseVisualStyleBackColor = true;
            this.btnThread1.Click += new System.EventHandler(this.btnThread1_Click);
            // 
            // btnThread2
            // 
            this.btnThread2.Location = new System.Drawing.Point(168, 27);
            this.btnThread2.Name = "btnThread2";
            this.btnThread2.Size = new System.Drawing.Size(94, 29);
            this.btnThread2.TabIndex = 1;
            this.btnThread2.Text = "Thread 2";
            this.btnThread2.UseVisualStyleBackColor = true;
            this.btnThread2.Click += new System.EventHandler(this.btnThread2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnThread2);
            this.Controls.Add(this.btnThread1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnThread1;
        private System.Windows.Forms.Button btnThread2;
    }
}

