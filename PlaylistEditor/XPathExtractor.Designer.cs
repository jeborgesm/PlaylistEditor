namespace PlaylistEditor
{
    partial class XPathExtractor
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
            this.advancedWebBrowser1 = new AdvancedWebBrowser.AdvancedWebBrowser();
            this.btnURL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // advancedWebBrowser1
            // 
            this.advancedWebBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedWebBrowser1.AutoSize = true;
            this.advancedWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.advancedWebBrowser1.Name = "advancedWebBrowser1";
            this.advancedWebBrowser1.Size = new System.Drawing.Size(576, 488);
            this.advancedWebBrowser1.TabIndex = 0;
            // 
            // btnURL
            // 
            this.btnURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnURL.Location = new System.Drawing.Point(12, 511);
            this.btnURL.Name = "btnURL";
            this.btnURL.Size = new System.Drawing.Size(75, 23);
            this.btnURL.TabIndex = 1;
            this.btnURL.Text = "URL";
            this.btnURL.UseVisualStyleBackColor = true;
            this.btnURL.Click += new System.EventHandler(this.btnURL_Click);
            // 
            // XPathExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 546);
            this.Controls.Add(this.btnURL);
            this.Controls.Add(this.advancedWebBrowser1);
            this.Name = "XPathExtractor";
            this.Text = "XPathExtractor";
            this.Load += new System.EventHandler(this.XPathExtractor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AdvancedWebBrowser.AdvancedWebBrowser advancedWebBrowser1;
        private System.Windows.Forms.Button btnURL;

    }
}