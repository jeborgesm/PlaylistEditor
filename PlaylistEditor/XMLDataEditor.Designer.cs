namespace PlaylistEditor
{
    partial class XMLDataEditor
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
            this.btnOpenXML = new System.Windows.Forms.Button();
            this.pnlDynForm = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnOpenXML
            // 
            this.btnOpenXML.Location = new System.Drawing.Point(12, 12);
            this.btnOpenXML.Name = "btnOpenXML";
            this.btnOpenXML.Size = new System.Drawing.Size(131, 29);
            this.btnOpenXML.TabIndex = 0;
            this.btnOpenXML.Text = "Open XML";
            this.btnOpenXML.UseVisualStyleBackColor = true;
            this.btnOpenXML.Click += new System.EventHandler(this.btnOpenXML_Click);
            // 
            // pnlDynForm
            // 
            this.pnlDynForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDynForm.AutoScroll = true;
            this.pnlDynForm.Location = new System.Drawing.Point(0, 47);
            this.pnlDynForm.Name = "pnlDynForm";
            this.pnlDynForm.Size = new System.Drawing.Size(604, 497);
            this.pnlDynForm.TabIndex = 1;
            // 
            // XMLDataEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 544);
            this.Controls.Add(this.pnlDynForm);
            this.Controls.Add(this.btnOpenXML);
            this.Name = "XMLDataEditor";
            this.Text = "XMLDataEditor";
            this.Load += new System.EventHandler(this.XMLDataEditor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenXML;
        private System.Windows.Forms.Panel pnlDynForm;
    }
}