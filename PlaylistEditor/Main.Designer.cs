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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnGetGameDetails = new System.Windows.Forms.Button();
            this.GameNameBox = new System.Windows.Forms.TextBox();
            this.ResultsBox = new System.Windows.Forms.TextBox();
            this.ProcessProgress = new ProgressBarWithCaption();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnXMLSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.BackgroundImage")));
            this.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnUpdate.Location = new System.Drawing.Point(12, 12);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(179, 159);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "Update Games Details";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
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
            this.ProcessProgress.DisplayStyle = ProgressBarDisplayText.CustomText;
            this.ProcessProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ProcessProgress.Location = new System.Drawing.Point(0, 679);
            this.ProcessProgress.Name = "ProcessProgress";
            this.ProcessProgress.Size = new System.Drawing.Size(721, 23);
            this.ProcessProgress.TabIndex = 5;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Red;
            this.btnStop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStop.BackgroundImage")));
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnStop.Location = new System.Drawing.Point(212, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(179, 159);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "STOP Update";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnXMLSearch
            // 
            this.btnXMLSearch.Location = new System.Drawing.Point(567, 173);
            this.btnXMLSearch.Name = "btnXMLSearch";
            this.btnXMLSearch.Size = new System.Drawing.Size(113, 31);
            this.btnXMLSearch.TabIndex = 7;
            this.btnXMLSearch.Text = "Find in xml";
            this.btnXMLSearch.UseVisualStyleBackColor = true;
            this.btnXMLSearch.Click += new System.EventHandler(this.btnXMLSearch_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 702);
            this.Controls.Add(this.btnXMLSearch);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.ProcessProgress);
            this.Controls.Add(this.ResultsBox);
            this.Controls.Add(this.GameNameBox);
            this.Controls.Add(this.btnGetGameDetails);
            this.Controls.Add(this.btnUpdate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "MAME Playlist Scraper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnGetGameDetails;
        private System.Windows.Forms.TextBox GameNameBox;
        private System.Windows.Forms.TextBox ResultsBox;
        private ProgressBarWithCaption ProcessProgress;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnXMLSearch;
    }
}

