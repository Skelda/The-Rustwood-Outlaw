namespace The_Rustwood_Outlaw
{
    partial class Board
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.Background = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.MainMenu = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Pause = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.PauseText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Background)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.Pause.SuspendLayout();
            this.SuspendLayout();
            // 
            // Background
            // 
            this.Background.BackgroundImage = global::The_Rustwood_Outlaw.Properties.Resources.background;
            this.Background.Location = new System.Drawing.Point(0, 0);
            this.Background.Margin = new System.Windows.Forms.Padding(34, 39, 34, 39);
            this.Background.Name = "Background";
            this.Background.Size = new System.Drawing.Size(5760, 6500);
            this.Background.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Background.TabIndex = 0;
            this.Background.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 13);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(935, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // MainMenu
            // 
            this.MainMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainMenu.Controls.Add(this.button1);
            this.MainMenu.Controls.Add(this.textBox1);
            this.MainMenu.Location = new System.Drawing.Point(302, 157);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(369, 241);
            this.MainMenu.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox1.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.textBox1.Location = new System.Drawing.Point(3, 46);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(363, 40);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "MENU";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(146, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Play";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Pause
            // 
            this.Pause.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Pause.Controls.Add(this.button2);
            this.Pause.Controls.Add(this.PauseText);
            this.Pause.Location = new System.Drawing.Point(332, 31);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(369, 241);
            this.Pause.TabIndex = 3;
            this.Pause.Visible = false;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button2.Location = new System.Drawing.Point(126, 146);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 1;
            this.button2.Text = "Continue";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // PauseText
            // 
            this.PauseText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.PauseText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PauseText.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PauseText.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.PauseText.Location = new System.Drawing.Point(3, 46);
            this.PauseText.Name = "PauseText";
            this.PauseText.Size = new System.Drawing.Size(363, 40);
            this.PauseText.TabIndex = 0;
            this.PauseText.Text = "Pause";
            this.PauseText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Board
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(960, 780);
            this.Controls.Add(this.Pause);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Background);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(773, 975, 773, 975);
            this.Name = "Board";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Background)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.Pause.ResumeLayout(false);
            this.Pause.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Background;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel MainMenu;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel Pause;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox PauseText;
    }
}
