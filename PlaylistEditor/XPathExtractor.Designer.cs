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
            this.btnXPATH = new System.Windows.Forms.Button();
            this.txtXPath = new System.Windows.Forms.TextBox();
            this.lblXHTML = new System.Windows.Forms.Label();
            this.lblExtracted = new System.Windows.Forms.Label();
            this.txtExtracted = new System.Windows.Forms.TextBox();
            this.pnlXHTML = new System.Windows.Forms.Panel();
            this.tvXML = new System.Windows.Forms.TreeView();
            this.tblXHTML = new System.Windows.Forms.TableLayoutPanel();
            this.vSplitter = new System.Windows.Forms.Splitter();
            this.pnlExtract = new System.Windows.Forms.Panel();
            this.tblExtract = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlXHTML.SuspendLayout();
            this.tblXHTML.SuspendLayout();
            this.pnlExtract.SuspendLayout();
            this.tblExtract.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.pnlExtract);
            this.splitContainer1.Panel2.Controls.Add(this.vSplitter);
            this.splitContainer1.Panel2.Controls.Add(this.pnlXHTML);
            this.splitContainer1.Panel2.Controls.Add(this.btnXPATH);
            this.splitContainer1.Panel2.Controls.Add(this.txtXPath);
            this.splitContainer1.Size = new System.Drawing.Size(879, 671);
            this.splitContainer1.SplitterDistance = 296;
            this.splitContainer1.TabIndex = 3;
            // 
            // advancedWebBrowser1
            // 
            this.advancedWebBrowser1.AutoSize = true;
            this.advancedWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advancedWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.advancedWebBrowser1.Name = "advancedWebBrowser1";
            this.advancedWebBrowser1.Size = new System.Drawing.Size(879, 296);
            this.advancedWebBrowser1.TabIndex = 1;
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
            // lblXHTML
            // 
            this.lblXHTML.AutoSize = true;
            this.lblXHTML.Location = new System.Drawing.Point(5, 5);
            this.lblXHTML.Margin = new System.Windows.Forms.Padding(5);
            this.lblXHTML.Name = "lblXHTML";
            this.lblXHTML.Size = new System.Drawing.Size(59, 11);
            this.lblXHTML.TabIndex = 14;
            this.lblXHTML.Text = "XHTML:";
            // 
            // lblExtracted
            // 
            this.lblExtracted.AutoSize = true;
            this.lblExtracted.Location = new System.Drawing.Point(5, 5);
            this.lblExtracted.Margin = new System.Windows.Forms.Padding(5);
            this.lblExtracted.Name = "lblExtracted";
            this.lblExtracted.Size = new System.Drawing.Size(102, 11);
            this.lblExtracted.TabIndex = 9;
            this.lblExtracted.Text = "Extracted Text:";
            // 
            // txtExtracted
            // 
            this.txtExtracted.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExtracted.Location = new System.Drawing.Point(3, 24);
            this.txtExtracted.Multiline = true;
            this.txtExtracted.Name = "txtExtracted";
            this.txtExtracted.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtExtracted.Size = new System.Drawing.Size(495, 322);
            this.txtExtracted.TabIndex = 13;
            // 
            // pnlXHTML
            // 
            this.pnlXHTML.Controls.Add(this.tblXHTML);
            this.pnlXHTML.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlXHTML.Location = new System.Drawing.Point(0, 22);
            this.pnlXHTML.Name = "pnlXHTML";
            this.pnlXHTML.Size = new System.Drawing.Size(375, 349);
            this.pnlXHTML.TabIndex = 15;
            // 
            // tvXML
            // 
            this.tvXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvXML.Location = new System.Drawing.Point(3, 24);
            this.tvXML.Name = "tvXML";
            this.tvXML.Size = new System.Drawing.Size(369, 322);
            this.tvXML.TabIndex = 12;
            // 
            // tblXHTML
            // 
            this.tblXHTML.ColumnCount = 1;
            this.tblXHTML.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblXHTML.Controls.Add(this.lblXHTML, 0, 0);
            this.tblXHTML.Controls.Add(this.tvXML, 0, 1);
            this.tblXHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblXHTML.Location = new System.Drawing.Point(0, 0);
            this.tblXHTML.Name = "tblXHTML";
            this.tblXHTML.RowCount = 2;
            this.tblXHTML.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.303725F));
            this.tblXHTML.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.69627F));
            this.tblXHTML.Size = new System.Drawing.Size(375, 349);
            this.tblXHTML.TabIndex = 0;
            // 
            // vSplitter
            // 
            this.vSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.vSplitter.Location = new System.Drawing.Point(375, 22);
            this.vSplitter.Name = "vSplitter";
            this.vSplitter.Size = new System.Drawing.Size(3, 349);
            this.vSplitter.TabIndex = 16;
            this.vSplitter.TabStop = false;
            // 
            // pnlExtract
            // 
            this.pnlExtract.Controls.Add(this.tblExtract);
            this.pnlExtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExtract.Location = new System.Drawing.Point(378, 22);
            this.pnlExtract.Name = "pnlExtract";
            this.pnlExtract.Size = new System.Drawing.Size(501, 349);
            this.pnlExtract.TabIndex = 17;
            // 
            // tblExtract
            // 
            this.tblExtract.ColumnCount = 1;
            this.tblExtract.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblExtract.Controls.Add(this.lblExtracted, 0, 0);
            this.tblExtract.Controls.Add(this.txtExtracted, 0, 1);
            this.tblExtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblExtract.Location = new System.Drawing.Point(0, 0);
            this.tblExtract.Name = "tblExtract";
            this.tblExtract.RowCount = 2;
            this.tblExtract.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.303725F));
            this.tblExtract.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.69627F));
            this.tblExtract.Size = new System.Drawing.Size(501, 349);
            this.tblExtract.TabIndex = 0;
            // 
            // XPathExtractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 671);
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
            this.pnlXHTML.ResumeLayout(false);
            this.tblXHTML.ResumeLayout(false);
            this.tblXHTML.PerformLayout();
            this.pnlExtract.ResumeLayout(false);
            this.tblExtract.ResumeLayout(false);
            this.tblExtract.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private AdvancedWebBrowser.AdvancedWebBrowser advancedWebBrowser1;
        private System.Windows.Forms.Button btnXPATH;
        private System.Windows.Forms.TextBox txtXPath;
        private System.Windows.Forms.Label lblXHTML;
        private System.Windows.Forms.Label lblExtracted;
        private System.Windows.Forms.TextBox txtExtracted;
        private System.Windows.Forms.Panel pnlExtract;
        private System.Windows.Forms.TableLayoutPanel tblExtract;
        private System.Windows.Forms.Splitter vSplitter;
        private System.Windows.Forms.Panel pnlXHTML;
        private System.Windows.Forms.TableLayoutPanel tblXHTML;
        private System.Windows.Forms.TreeView tvXML;
    }
}