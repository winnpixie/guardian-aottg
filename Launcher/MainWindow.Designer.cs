
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
            this.actionGroup = new System.Windows.Forms.GroupBox();
            this.uploadLogBtn = new System.Windows.Forms.Button();
            this.startGameBtn = new System.Windows.Forms.Button();
            this.updateAndPlayBtn = new System.Windows.Forms.Button();
            this.outputLogArea = new System.Windows.Forms.RichTextBox();
            this.actionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionGroup
            // 
            this.actionGroup.Controls.Add(this.uploadLogBtn);
            this.actionGroup.Controls.Add(this.startGameBtn);
            this.actionGroup.Controls.Add(this.updateAndPlayBtn);
            this.actionGroup.Controls.Add(this.outputLogArea);
            this.actionGroup.Location = new System.Drawing.Point(12, 11);
            this.actionGroup.Name = "actionGroup";
            this.actionGroup.Size = new System.Drawing.Size(690, 438);
            this.actionGroup.TabIndex = 0;
            this.actionGroup.TabStop = false;
            this.actionGroup.Text = "Actions";
            // 
            // uploadLogBtn
            // 
            this.uploadLogBtn.Location = new System.Drawing.Point(6, 384);
            this.uploadLogBtn.Name = "uploadLogBtn";
            this.uploadLogBtn.Size = new System.Drawing.Size(335, 48);
            this.uploadLogBtn.TabIndex = 2;
            this.uploadLogBtn.Text = "Upload output_log.txt to Hastebin\r\n\r\n(Game must be ran at least once!)";
            this.uploadLogBtn.UseVisualStyleBackColor = true;
            this.uploadLogBtn.Click += new System.EventHandler(this.uploadLogBtn_Click);
            // 
            // startGameBtn
            // 
            this.startGameBtn.Location = new System.Drawing.Point(347, 19);
            this.startGameBtn.Name = "startGameBtn";
            this.startGameBtn.Size = new System.Drawing.Size(335, 32);
            this.startGameBtn.TabIndex = 1;
            this.startGameBtn.Text = "Play (No Update)";
            this.startGameBtn.UseVisualStyleBackColor = true;
            this.startGameBtn.Click += new System.EventHandler(this.startGameBtn_Click);
            // 
            // updateAndPlayBtn
            // 
            this.updateAndPlayBtn.Location = new System.Drawing.Point(6, 19);
            this.updateAndPlayBtn.Name = "updateAndPlayBtn";
            this.updateAndPlayBtn.Size = new System.Drawing.Size(335, 32);
            this.updateAndPlayBtn.TabIndex = 0;
            this.updateAndPlayBtn.Text = "Update && Play";
            this.updateAndPlayBtn.UseVisualStyleBackColor = true;
            this.updateAndPlayBtn.Click += new System.EventHandler(this.updateAndPlayBtn_Start);
            // 
            // outputLogArea
            // 
            this.outputLogArea.Location = new System.Drawing.Point(6, 57);
            this.outputLogArea.Name = "outputLogArea";
            this.outputLogArea.ReadOnly = true;
            this.outputLogArea.Size = new System.Drawing.Size(677, 321);
            this.outputLogArea.TabIndex = 3;
            this.outputLogArea.Text = "";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 461);
            this.Controls.Add(this.actionGroup);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.Text = "Guardian Mod Launcher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.actionGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox actionGroup;
        private System.Windows.Forms.RichTextBox outputLogArea;
        private System.Windows.Forms.Button updateAndPlayBtn;
        private System.Windows.Forms.Button uploadLogBtn;
        private System.Windows.Forms.Button startGameBtn;
    }
}

