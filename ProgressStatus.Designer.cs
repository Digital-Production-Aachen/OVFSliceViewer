namespace LayerViewer
{
    partial class ProgressStatus
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
            this.progressBar = new ViewerCustomControls.ColorProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.AccessibleName = "progressBar";
            this.progressBar.Location = new System.Drawing.Point(12, 70);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(629, 43);
            this.progressBar.TabIndex = 0;
            // 
            // progressLabel
            // 
            this.progressLabel.AccessibleName = "progressLabel";
            this.progressLabel.AutoSize = true;
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.Location = new System.Drawing.Point(192, 9);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(241, 42);
            this.progressLabel.TabIndex = 1;
            this.progressLabel.Text = "Loading Data";
            // 
            // ProgressStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 125);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.Name = "ProgressStatus";
            this.Text = "ProgressStatus";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ViewerCustomControls.ColorProgressBar progressBar;
        private System.Windows.Forms.Label progressLabel;
    }
}