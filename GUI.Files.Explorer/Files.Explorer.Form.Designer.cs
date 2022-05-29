namespace GUI.Files.Explorer
{
    partial class FileExplorerForm
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
            this.btn_GoBack = new System.Windows.Forms.Button();
            this.btn_GoForward = new System.Windows.Forms.Button();
            this.btn_OpenExplorer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtb_Path = new System.Windows.Forms.TextBox();
            this.webbrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // btn_GoBack
            // 
            this.btn_GoBack.Location = new System.Drawing.Point(9, 12);
            this.btn_GoBack.Name = "btn_GoBack";
            this.btn_GoBack.Size = new System.Drawing.Size(94, 29);
            this.btn_GoBack.TabIndex = 0;
            this.btn_GoBack.Text = "<<";
            this.btn_GoBack.UseVisualStyleBackColor = true;
            this.btn_GoBack.Click += new System.EventHandler(this.btn_GoBack_Click);
            // 
            // btn_GoForward
            // 
            this.btn_GoForward.Location = new System.Drawing.Point(117, 12);
            this.btn_GoForward.Name = "btn_GoForward";
            this.btn_GoForward.Size = new System.Drawing.Size(94, 29);
            this.btn_GoForward.TabIndex = 1;
            this.btn_GoForward.Text = ">>";
            this.btn_GoForward.UseVisualStyleBackColor = true;
            this.btn_GoForward.Click += new System.EventHandler(this.btn_GoForward_Click);
            // 
            // btn_OpenExplorer
            // 
            this.btn_OpenExplorer.Location = new System.Drawing.Point(940, 13);
            this.btn_OpenExplorer.Name = "btn_OpenExplorer";
            this.btn_OpenExplorer.Size = new System.Drawing.Size(94, 29);
            this.btn_OpenExplorer.TabIndex = 2;
            this.btn_OpenExplorer.Text = "Open";
            this.btn_OpenExplorer.UseVisualStyleBackColor = true;
            this.btn_OpenExplorer.Click += new System.EventHandler(this.btn_OpenExplorer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path:";
            // 
            // txtb_Path
            // 
            this.txtb_Path.Location = new System.Drawing.Point(273, 14);
            this.txtb_Path.Name = "txtb_Path";
            this.txtb_Path.ReadOnly = true;
            this.txtb_Path.Size = new System.Drawing.Size(660, 27);
            this.txtb_Path.TabIndex = 4;
            this.txtb_Path.Text = "Select path..";
            // 
            // webbrowser
            // 
            this.webbrowser.Location = new System.Drawing.Point(12, 61);
            this.webbrowser.Name = "webbrowser";
            this.webbrowser.Size = new System.Drawing.Size(1022, 606);
            this.webbrowser.TabIndex = 5;
            // 
            // FileExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 694);
            this.Controls.Add(this.txtb_Path);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_OpenExplorer);
            this.Controls.Add(this.btn_GoForward);
            this.Controls.Add(this.btn_GoBack);
            this.Controls.Add(this.webbrowser);
            this.Name = "FileExplorerForm";
            this.Text = "FileExplorer";
            this.Load += new System.EventHandler(this.FileExplorerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btn_GoBack;
        private Button btn_GoForward;
        private Button btn_OpenExplorer;
        private Label label1;
        private TextBox txtb_Path;
        private WebBrowser webbrowser;
    }
}