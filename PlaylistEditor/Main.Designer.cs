namespace PlaylistEditor
{
    partial class Main
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnGetGameDetails = new System.Windows.Forms.Button();
            this.GameNameBox = new System.Windows.Forms.TextBox();
            this.ResultsBox = new System.Windows.Forms.TextBox();
            this.ProcessProgress = new PlaylistEditor.ProgressBarWithCaption();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(156, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(405, 136);
            this.button1.TabIndex = 0;
            this.button1.Text = "Read all Files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGetGameDetails
            // 
            this.btnGetGameDetails.Location = new System.Drawing.Point(428, 173);
            this.btnGetGameDetails.Name = "btnGetGameDetails";
            this.btnGetGameDetails.Size = new System.Drawing.Size(133, 31);
            this.btnGetGameDetails.TabIndex = 1;
            this.btnGetGameDetails.Text = "Get Game Details";
            this.btnGetGameDetails.UseVisualStyleBackColor = true;
            this.btnGetGameDetails.Click += new System.EventHandler(this.btnGetGameDetails_Click);
            // 
            // GameNameBox
            // 
            this.GameNameBox.Location = new System.Drawing.Point(156, 177);
            this.GameNameBox.Name = "GameNameBox";
            this.GameNameBox.Size = new System.Drawing.Size(273, 22);
            this.GameNameBox.TabIndex = 3;
            this.GameNameBox.Text = "Makyou Senshi";
            // 
            // ResultsBox
            // 
            this.ResultsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsBox.Location = new System.Drawing.Point(12, 210);
            this.ResultsBox.MaxLength = 500000000;
            this.ResultsBox.Multiline = true;
            this.ResultsBox.Name = "ResultsBox";
            this.ResultsBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ResultsBox.Size = new System.Drawing.Size(697, 451);
            this.ResultsBox.TabIndex = 4;
            // 
            // ProcessProgress
            // 
            this.ProcessProgress.CustomText = null;
            this.ProcessProgress.DisplayStyle = PlaylistEditor.ProgressBarDisplayText.CustomText;
            this.ProcessProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ProcessProgress.Location = new System.Drawing.Point(0, 679);
            this.ProcessProgress.Name = "ProcessProgress";
            this.ProcessProgress.Size = new System.Drawing.Size(721, 23);
            this.ProcessProgress.TabIndex = 5;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 702);
            this.Controls.Add(this.ProcessProgress);
            this.Controls.Add(this.ResultsBox);
            this.Controls.Add(this.GameNameBox);
            this.Controls.Add(this.btnGetGameDetails);
            this.Controls.Add(this.button1);
            this.Name = "Main";
            this.Text = "MAME Playlist Editor - Scraper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnGetGameDetails;
        private System.Windows.Forms.TextBox GameNameBox;
        private System.Windows.Forms.TextBox ResultsBox;
        private ProgressBarWithCaption ProcessProgress;
    }
}

