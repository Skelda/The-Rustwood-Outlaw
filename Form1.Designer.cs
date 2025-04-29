namespace The_Rustwood_Outlaw
{
    partial class Form1
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
            this.LFrameRate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LFrameRate
            // 
            this.LFrameRate.AutoSize = true;
            this.LFrameRate.ForeColor = System.Drawing.Color.Red;
            this.LFrameRate.Location = new System.Drawing.Point(1228, 9);
            this.LFrameRate.Name = "LFrameRate";
            this.LFrameRate.Size = new System.Drawing.Size(36, 25);
            this.LFrameRate.TabIndex = 0;
            this.LFrameRate.Text = "10";
            this.LFrameRate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 629);
            this.Controls.Add(this.LFrameRate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LFrameRate;
    }
}
