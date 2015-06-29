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
            this.SuspendLayout();
            // 
            // advancedWebBrowser1
            // 
            this.advancedWebBrowser1.AutoSize = true;
            this.advancedWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advancedWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.advancedWebBrowser1.Name = "advancedWebBrowser1";
            this.advancedWebBrowser1.Size = new System.Drawing.Size(508, 471);
            this.advancedWebBrowser1.TabIndex = 0;
            // 
            // XPathExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 471);
            this.Controls.Add(this.advancedWebBrowser1);
            this.Name = "XPathExtractor";
            this.Text = "XPathExtractor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AdvancedWebBrowser.AdvancedWebBrowser advancedWebBrowser1;

    }
}