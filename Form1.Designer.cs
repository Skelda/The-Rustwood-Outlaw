using static The_Rustwood_Outlaw.GameSettings;
using System;

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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.MainMenu = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.menuLabel = new System.Windows.Forms.Label();
            this.Pause = new System.Windows.Forms.Panel();
            this.lDifficulty1 = new System.Windows.Forms.Label();
            this.difficulty = new System.Windows.Forms.ComboBox();
            this.bContinue = new System.Windows.Forms.Button();
            this.PauseLabel = new System.Windows.Forms.Label();
            this.lLevel = new System.Windows.Forms.Label();
            this.lScore = new System.Windows.Forms.Label();
            this.Background = new System.Windows.Forms.PictureBox();
            this.lHealth = new System.Windows.Forms.Label();
            this.panelHearts = new System.Windows.Forms.Panel();
            this.pYouLost = new System.Windows.Forms.Panel();
            this.lLostScreenLevel = new System.Windows.Forms.Label();
            this.playAgain = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lLostScreenScore = new System.Windows.Forms.Label();
            this.MainMenu.SuspendLayout();
            this.Pause.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Background)).BeginInit();
            this.pYouLost.SuspendLayout();
            this.SuspendLayout();
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
            this.MainMenu.Controls.Add(this.menuLabel);
            this.MainMenu.Location = new System.Drawing.Point(302, 157);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(369, 241);
            this.MainMenu.TabIndex = 2;
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
            this.button1.Click += new System.EventHandler(this.bStartGame_Click);
            // 
            // menuLabel
            // 
            this.menuLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.menuLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.menuLabel.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.menuLabel.Location = new System.Drawing.Point(3, 46);
            this.menuLabel.Name = "menuLabel";
            this.menuLabel.Size = new System.Drawing.Size(363, 40);
            this.menuLabel.TabIndex = 0;
            this.menuLabel.Text = "MENU";
            this.menuLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Pause
            // 
            this.Pause.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Pause.Controls.Add(this.lDifficulty1);
            this.Pause.Controls.Add(this.difficulty);
            this.Pause.Controls.Add(this.bContinue);
            this.Pause.Controls.Add(this.PauseLabel);
            this.Pause.Location = new System.Drawing.Point(302, 154);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(369, 241);
            this.Pause.TabIndex = 3;
            this.Pause.Visible = false;
            // 
            // lDifficulty1
            // 
            this.lDifficulty1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.lDifficulty1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lDifficulty1.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.lDifficulty1.Location = new System.Drawing.Point(67, 135);
            this.lDifficulty1.Name = "lDifficulty1";
            this.lDifficulty1.Size = new System.Drawing.Size(104, 40);
            this.lDifficulty1.TabIndex = 3;
            this.lDifficulty1.Text = "Difficulty";
            this.lDifficulty1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // difficulty
            // 
            this.difficulty.FormattingEnabled = true;
            this.difficulty.Location = new System.Drawing.Point(177, 149);
            this.difficulty.Name = "difficulty";
            this.difficulty.Size = new System.Drawing.Size(121, 21);
            this.difficulty.TabIndex = 4;
            // 
            // bContinue
            // 
            this.bContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.bContinue.Location = new System.Drawing.Point(129, 92);
            this.bContinue.Name = "bContinue";
            this.bContinue.Size = new System.Drawing.Size(112, 35);
            this.bContinue.TabIndex = 1;
            this.bContinue.Text = "Continue";
            this.bContinue.UseVisualStyleBackColor = true;
            this.bContinue.Click += new System.EventHandler(this.bContinue_Click);
            // 
            // PauseLabel
            // 
            this.PauseLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.PauseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PauseLabel.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.PauseLabel.Location = new System.Drawing.Point(3, 46);
            this.PauseLabel.Name = "PauseLabel";
            this.PauseLabel.Size = new System.Drawing.Size(363, 40);
            this.PauseLabel.TabIndex = 0;
            this.PauseLabel.Text = "Pause";
            this.PauseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lLevel
            // 
            this.lLevel.AutoSize = true;
            this.lLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lLevel.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.lLevel.Location = new System.Drawing.Point(13, 43);
            this.lLevel.Name = "lLevel";
            this.lLevel.Size = new System.Drawing.Size(63, 20);
            this.lLevel.TabIndex = 4;
            this.lLevel.Text = "Level: 0";
            // 
            // lScore
            // 
            this.lScore.AutoSize = true;
            this.lScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lScore.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.lScore.Location = new System.Drawing.Point(13, 63);
            this.lScore.Name = "lScore";
            this.lScore.Size = new System.Drawing.Size(68, 20);
            this.lScore.TabIndex = 5;
            this.lScore.Text = "Score: 0";
            // 
            // Background
            // 
            this.Background.BackgroundImage = global::The_Rustwood_Outlaw.Properties.Resources.background;
            this.Background.Enabled = false;
            this.Background.Location = new System.Drawing.Point(-1, 0);
            this.Background.Margin = new System.Windows.Forms.Padding(34, 39, 34, 39);
            this.Background.Name = "Background";
            this.Background.Size = new System.Drawing.Size(5760, 6500);
            this.Background.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Background.TabIndex = 0;
            this.Background.TabStop = false;
            // 
            // lHealth
            // 
            this.lHealth.AutoSize = true;
            this.lHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lHealth.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.lHealth.Location = new System.Drawing.Point(13, 83);
            this.lHealth.Name = "lHealth";
            this.lHealth.Size = new System.Drawing.Size(60, 20);
            this.lHealth.TabIndex = 6;
            this.lHealth.Text = "Health:";
            // 
            // panelHearts
            // 
            this.panelHearts.Location = new System.Drawing.Point(70, 83);
            this.panelHearts.Name = "panelHearts";
            this.panelHearts.Size = new System.Drawing.Size(10, 20);
            this.panelHearts.TabIndex = 7;
            // 
            // pYouLost
            // 
            this.pYouLost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pYouLost.Controls.Add(this.lLostScreenScore);
            this.pYouLost.Controls.Add(this.lLostScreenLevel);
            this.pYouLost.Controls.Add(this.playAgain);
            this.pYouLost.Controls.Add(this.label2);
            this.pYouLost.Location = new System.Drawing.Point(302, 151);
            this.pYouLost.Name = "pYouLost";
            this.pYouLost.Size = new System.Drawing.Size(369, 241);
            this.pYouLost.TabIndex = 5;
            this.pYouLost.Visible = false;
            // 
            // lLostScreenLevel
            // 
            this.lLostScreenLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.lLostScreenLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lLostScreenLevel.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.lLostScreenLevel.Location = new System.Drawing.Point(124, 130);
            this.lLostScreenLevel.Name = "lLostScreenLevel";
            this.lLostScreenLevel.Size = new System.Drawing.Size(117, 40);
            this.lLostScreenLevel.TabIndex = 3;
            this.lLostScreenLevel.Text = "Level:";
            this.lLostScreenLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playAgain
            // 
            this.playAgain.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.playAgain.Location = new System.Drawing.Point(129, 92);
            this.playAgain.Name = "playAgain";
            this.playAgain.Size = new System.Drawing.Size(112, 35);
            this.playAgain.TabIndex = 1;
            this.playAgain.Text = "Play again";
            this.playAgain.UseVisualStyleBackColor = true;
            this.playAgain.Click += new System.EventHandler(this.bStartGame_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(363, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "You lost";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lLostScreenScore
            // 
            this.lLostScreenScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.lLostScreenScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lLostScreenScore.ForeColor = System.Drawing.Color.MediumSpringGreen;
            this.lLostScreenScore.Location = new System.Drawing.Point(124, 170);
            this.lLostScreenScore.Name = "lLostScreenScore";
            this.lLostScreenScore.Size = new System.Drawing.Size(117, 40);
            this.lLostScreenScore.TabIndex = 4;
            this.lLostScreenScore.Text = "Score:";
            this.lLostScreenScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Board
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(960, 780);
            this.Controls.Add(this.pYouLost);
            this.Controls.Add(this.panelHearts);
            this.Controls.Add(this.lHealth);
            this.Controls.Add(this.Pause);
            this.Controls.Add(this.lScore);
            this.Controls.Add(this.lLevel);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Background);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(773, 975, 773, 975);
            this.Name = "Board";
            this.Text = "The Rustwood Outlaw";
            this.MainMenu.ResumeLayout(false);
            this.Pause.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Background)).EndInit();
            this.pYouLost.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel MainMenu;
        private System.Windows.Forms.Label menuLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel Pause;
        private System.Windows.Forms.Button bContinue;
        private System.Windows.Forms.Label PauseLabel;
        private System.Windows.Forms.Label lLevel;
        private System.Windows.Forms.Label lScore;
        private System.Windows.Forms.ComboBox difficulty;
        private System.Windows.Forms.Label lDifficulty1;
        private System.Windows.Forms.PictureBox Background;
        private System.Windows.Forms.Label lHealth;
        private System.Windows.Forms.Panel panelHearts;
        private System.Windows.Forms.Panel pYouLost;
        private System.Windows.Forms.Label lLostScreenLevel;
        private System.Windows.Forms.Button playAgain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lLostScreenScore;
    }
}
