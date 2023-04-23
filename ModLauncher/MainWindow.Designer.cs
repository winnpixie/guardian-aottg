
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
            this.PlayBtn = new System.Windows.Forms.Button();
            this.UpdateBtn = new System.Windows.Forms.Button();
            this.OutputLogBox = new System.Windows.Forms.RichTextBox();
            this.MainGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroup
            // 
            this.MainGroup.Controls.Add(this.InformationLbl);
            this.MainGroup.Controls.Add(this.PlayBtn);
            this.MainGroup.Controls.Add(this.UpdateBtn);
            this.MainGroup.Controls.Add(this.OutputLogBox);
            this.MainGroup.Location = new System.Drawing.Point(12, 12);
            this.MainGroup.Name = "MainGroup";
            this.MainGroup.Size = new System.Drawing.Size(680, 417);
            this.MainGroup.TabIndex = 0;
            this.MainGroup.TabStop = false;
            this.MainGroup.Text = "Actions";
            // 
            // InformationLbl
            // 
            this.InformationLbl.AutoSize = true;
            this.InformationLbl.Location = new System.Drawing.Point(7, 366);
            this.InformationLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.InformationLbl.Name = "InformationLbl";
            this.InformationLbl.Size = new System.Drawing.Size(117, 45);
            this.InformationLbl.TabIndex = 4;
            this.InformationLbl.Text = "Platform: {0} {1}-bit\r\n\r\nLauncher Version: {2}";
            // 
            // PlayBtn
            // 
            this.PlayBtn.Location = new System.Drawing.Point(343, 22);
            this.PlayBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size(330, 40);
            this.PlayBtn.TabIndex = 1;
            this.PlayBtn.Text = "Start without Downloading";
            this.PlayBtn.UseVisualStyleBackColor = true;
            this.PlayBtn.Click += new System.EventHandler(this.StartGameBtn_Click);
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Location = new System.Drawing.Point(7, 22);
            this.UpdateBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(330, 40);
            this.UpdateBtn.TabIndex = 0;
            this.UpdateBtn.Text = "Update and Start";
            this.UpdateBtn.UseVisualStyleBackColor = true;
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateAndPlayBtn_Start);
            // 
            // OutputLogBox
            // 
            this.OutputLogBox.AutoWordSelection = true;
            this.OutputLogBox.Location = new System.Drawing.Point(7, 68);
            this.OutputLogBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OutputLogBox.Name = "OutputLogBox";
            this.OutputLogBox.ReadOnly = true;
            this.OutputLogBox.Size = new System.Drawing.Size(666, 295);
            this.OutputLogBox.TabIndex = 3;
            this.OutputLogBox.Text = "";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.MainGroup);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainWindow";
            this.Text = "Guardian Mod Launcher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.MainGroup.ResumeLayout(false);
            this.MainGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox MainGroup;
        private System.Windows.Forms.RichTextBox OutputLogBox;
        private System.Windows.Forms.Button UpdateBtn;
        private System.Windows.Forms.Button PlayBtn;
        private System.Windows.Forms.Label InformationLbl;
    }
}

