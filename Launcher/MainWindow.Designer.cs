
namespace Launcher
{
    partial class MainWindow
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
            this.MainGroup = new System.Windows.Forms.GroupBox();
            this.InformationLbl = new System.Windows.Forms.Label();
            this.UploadLogBtn = new System.Windows.Forms.Button();
            this.StartGameBtn = new System.Windows.Forms.Button();
            this.UpdateAndPlayBtn = new System.Windows.Forms.Button();
            this.LogOutputArea = new System.Windows.Forms.RichTextBox();
            this.MainGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroup
            // 
            this.MainGroup.Controls.Add(this.InformationLbl);
            this.MainGroup.Controls.Add(this.UploadLogBtn);
            this.MainGroup.Controls.Add(this.StartGameBtn);
            this.MainGroup.Controls.Add(this.UpdateAndPlayBtn);
            this.MainGroup.Controls.Add(this.LogOutputArea);
            this.MainGroup.Location = new System.Drawing.Point(12, 11);
            this.MainGroup.Name = "MainGroup";
            this.MainGroup.Size = new System.Drawing.Size(690, 438);
            this.MainGroup.TabIndex = 0;
            this.MainGroup.TabStop = false;
            this.MainGroup.Text = "Actions";
            // 
            // InformationLbl
            // 
            this.InformationLbl.AutoSize = true;
            this.InformationLbl.Location = new System.Drawing.Point(6, 384);
            this.InformationLbl.Name = "InformationLbl";
            this.InformationLbl.Size = new System.Drawing.Size(110, 39);
            this.InformationLbl.TabIndex = 4;
            this.InformationLbl.Text = "Platform: {0} {1}-bit\r\n\r\nLauncher Version: {2}";
            // 
            // UploadLogBtn
            // 
            this.UploadLogBtn.Location = new System.Drawing.Point(347, 384);
            this.UploadLogBtn.Name = "UploadLogBtn";
            this.UploadLogBtn.Size = new System.Drawing.Size(335, 48);
            this.UploadLogBtn.TabIndex = 2;
            this.UploadLogBtn.Text = "Upload output_log.txt to Hastebin.com\r\n\r\n(Game must be ran at least once!)";
            this.UploadLogBtn.UseVisualStyleBackColor = true;
            this.UploadLogBtn.Click += new System.EventHandler(this.UploadLogBtn_Click);
            // 
            // StartGameBtn
            // 
            this.StartGameBtn.Location = new System.Drawing.Point(347, 19);
            this.StartGameBtn.Name = "StartGameBtn";
            this.StartGameBtn.Size = new System.Drawing.Size(335, 32);
            this.StartGameBtn.TabIndex = 1;
            this.StartGameBtn.Text = "Play (Skip Update)";
            this.StartGameBtn.UseVisualStyleBackColor = true;
            this.StartGameBtn.Click += new System.EventHandler(this.StartGameBtn_Click);
            // 
            // UpdateAndPlayBtn
            // 
            this.UpdateAndPlayBtn.Location = new System.Drawing.Point(6, 19);
            this.UpdateAndPlayBtn.Name = "UpdateAndPlayBtn";
            this.UpdateAndPlayBtn.Size = new System.Drawing.Size(335, 32);
            this.UpdateAndPlayBtn.TabIndex = 0;
            this.UpdateAndPlayBtn.Text = "Update and Play";
            this.UpdateAndPlayBtn.UseVisualStyleBackColor = true;
            this.UpdateAndPlayBtn.Click += new System.EventHandler(this.UpdateAndPlayBtn_Start);
            // 
            // LogOutputArea
            // 
            this.LogOutputArea.Location = new System.Drawing.Point(6, 57);
            this.LogOutputArea.Name = "LogOutputArea";
            this.LogOutputArea.ReadOnly = true;
            this.LogOutputArea.Size = new System.Drawing.Size(677, 321);
            this.LogOutputArea.TabIndex = 3;
            this.LogOutputArea.Text = "";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 461);
            this.Controls.Add(this.MainGroup);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.Text = "Guardian Launcher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.MainGroup.ResumeLayout(false);
            this.MainGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox MainGroup;
        private System.Windows.Forms.RichTextBox LogOutputArea;
        private System.Windows.Forms.Button UpdateAndPlayBtn;
        private System.Windows.Forms.Button UploadLogBtn;
        private System.Windows.Forms.Button StartGameBtn;
        private System.Windows.Forms.Label InformationLbl;
    }
}

