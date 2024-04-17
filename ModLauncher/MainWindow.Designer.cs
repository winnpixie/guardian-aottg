
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
            this.InformationLbl = new System.Windows.Forms.Label();
            this.PlayBtn = new System.Windows.Forms.Button();
            this.UpdateBtn = new System.Windows.Forms.Button();
            this.OutputLogBox = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // InformationLbl
            // 
            this.InformationLbl.AutoSize = true;
            this.InformationLbl.Location = new System.Drawing.Point(13, 402);
            this.InformationLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.InformationLbl.Name = "InformationLbl";
            this.InformationLbl.Size = new System.Drawing.Size(117, 30);
            this.InformationLbl.TabIndex = 8;
            this.InformationLbl.Text = "Platform: {0} {1}-bit\r\nLauncher Version: {2}";
            // 
            // PlayBtn
            // 
            this.PlayBtn.Location = new System.Drawing.Point(356, 12);
            this.PlayBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size(335, 40);
            this.PlayBtn.TabIndex = 6;
            this.PlayBtn.Text = "Play (No Update)";
            this.PlayBtn.UseVisualStyleBackColor = true;
            this.PlayBtn.Click += new System.EventHandler(this.PlayBtn_Click);
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Location = new System.Drawing.Point(13, 12);
            this.UpdateBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(335, 40);
            this.UpdateBtn.TabIndex = 5;
            this.UpdateBtn.Text = "Play";
            this.UpdateBtn.UseVisualStyleBackColor = true;
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // OutputLogBox
            // 
            this.OutputLogBox.AutoWordSelection = true;
            this.OutputLogBox.Location = new System.Drawing.Point(13, 58);
            this.OutputLogBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OutputLogBox.Name = "OutputLogBox";
            this.OutputLogBox.ReadOnly = true;
            this.OutputLogBox.Size = new System.Drawing.Size(678, 341);
            this.OutputLogBox.TabIndex = 7;
            this.OutputLogBox.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.InformationLbl);
            this.Controls.Add(this.PlayBtn);
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.OutputLogBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainWindow";
            this.Text = "WinnPixie\'s Mod Launcher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InformationLbl;
        private System.Windows.Forms.Button PlayBtn;
        private System.Windows.Forms.Button UpdateBtn;
        private System.Windows.Forms.RichTextBox OutputLogBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

