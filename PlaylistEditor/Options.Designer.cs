namespace PlaylistEditor
{
    partial class Options
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMAMERoms = new System.Windows.Forms.TextBox();
            this.lblMAMERoms = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnMAMEPath = new System.Windows.Forms.Button();
            this.lblUserAgent = new System.Windows.Forms.Label();
            this.txtUserAgent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(399, 188);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 53);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(526, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(121, 53);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtMAMERoms
            // 
            this.txtMAMERoms.Location = new System.Drawing.Point(150, 37);
            this.txtMAMERoms.Name = "txtMAMERoms";
            this.txtMAMERoms.Size = new System.Drawing.Size(497, 22);
            this.txtMAMERoms.TabIndex = 2;
            // 
            // lblMAMERoms
            // 
            this.lblMAMERoms.AutoSize = true;
            this.lblMAMERoms.Location = new System.Drawing.Point(12, 40);
            this.lblMAMERoms.Name = "lblMAMERoms";
            this.lblMAMERoms.Size = new System.Drawing.Size(125, 17);
            this.lblMAMERoms.TabIndex = 3;
            this.lblMAMERoms.Text = "MAME Roms Path:";
            // 
            // btnMAMEPath
            // 
            this.btnMAMEPath.Location = new System.Drawing.Point(648, 36);
            this.btnMAMEPath.Name = "btnMAMEPath";
            this.btnMAMEPath.Size = new System.Drawing.Size(32, 24);
            this.btnMAMEPath.TabIndex = 4;
            this.btnMAMEPath.Text = "...";
            this.btnMAMEPath.UseVisualStyleBackColor = true;
            this.btnMAMEPath.Click += new System.EventHandler(this.btnMAMEPath_Click);
            // 
            // lblUserAgent
            // 
            this.lblUserAgent.AutoSize = true;
            this.lblUserAgent.Location = new System.Drawing.Point(12, 78);
            this.lblUserAgent.Name = "lblUserAgent";
            this.lblUserAgent.Size = new System.Drawing.Size(132, 17);
            this.lblUserAgent.TabIndex = 6;
            this.lblUserAgent.Text = "Scrape User Agent:";
            // 
            // txtUserAgent
            // 
            this.txtUserAgent.Location = new System.Drawing.Point(150, 75);
            this.txtUserAgent.Name = "txtUserAgent";
            this.txtUserAgent.Size = new System.Drawing.Size(497, 22);
            this.txtUserAgent.TabIndex = 5;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 253);
            this.Controls.Add(this.lblUserAgent);
            this.Controls.Add(this.txtUserAgent);
            this.Controls.Add(this.btnMAMEPath);
            this.Controls.Add(this.lblMAMERoms);
            this.Controls.Add(this.txtMAMERoms);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtMAMERoms;
        private System.Windows.Forms.Label lblMAMERoms;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnMAMEPath;
        private System.Windows.Forms.Label lblUserAgent;
        private System.Windows.Forms.TextBox txtUserAgent;
    }
}