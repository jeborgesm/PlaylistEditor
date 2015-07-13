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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.advancedWebBrowser1 = new AdvancedWebBrowser.AdvancedWebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExtracted = new System.Windows.Forms.TextBox();
            this.btnXPATH = new System.Windows.Forms.Button();
            this.txtXPath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.advancedWebBrowser1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.txtExtracted);
            this.splitContainer1.Panel2.Controls.Add(this.btnXPATH);
            this.splitContainer1.Panel2.Controls.Add(this.txtXPath);
            this.splitContainer1.Size = new System.Drawing.Size(879, 546);
            this.splitContainer1.SplitterDistance = 273;
            this.splitContainer1.TabIndex = 3;
            // 
            // advancedWebBrowser1
            // 
            this.advancedWebBrowser1.AutoSize = true;
            this.advancedWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advancedWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.advancedWebBrowser1.Name = "advancedWebBrowser1";
            this.advancedWebBrowser1.Size = new System.Drawing.Size(879, 273);
            this.advancedWebBrowser1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Extracted Text:";
            // 
            // txtExtracted
            // 
            this.txtExtracted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExtracted.Location = new System.Drawing.Point(3, 49);
            this.txtExtracted.Multiline = true;
            this.txtExtracted.Name = "txtExtracted";
            this.txtExtracted.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtExtracted.Size = new System.Drawing.Size(873, 217);
            this.txtExtracted.TabIndex = 8;
            // 
            // btnXPATH
            // 
            this.btnXPATH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXPATH.Location = new System.Drawing.Point(792, 0);
            this.btnXPATH.Name = "btnXPATH";
            this.btnXPATH.Size = new System.Drawing.Size(87, 23);
            this.btnXPATH.TabIndex = 6;
            this.btnXPATH.Text = "XPATH";
            this.btnXPATH.UseVisualStyleBackColor = true;
            this.btnXPATH.Click += new System.EventHandler(this.btnXPATH_Click);
            // 
            // txtXPath
            // 
            this.txtXPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtXPath.Location = new System.Drawing.Point(0, 0);
            this.txtXPath.Name = "txtXPath";
            this.txtXPath.Size = new System.Drawing.Size(879, 22);
            this.txtXPath.TabIndex = 7;
            // 
            // XPathExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 546);
            this.Controls.Add(this.splitContainer1);
            this.Name = "XPathExtractor";
            this.Text = "XPathExtractor";
            this.Load += new System.EventHandler(this.XPathExtractor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private AdvancedWebBrowser.AdvancedWebBrowser advancedWebBrowser1;
        private System.Windows.Forms.Button btnXPATH;
        private System.Windows.Forms.TextBox txtXPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExtracted;

    }
}