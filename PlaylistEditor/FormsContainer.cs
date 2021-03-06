﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class FormsContainer : Form
    {
        private int childFormNumber = 0;

        public FormsContainer()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void FormsContainer_Load(object sender, EventArgs e)
        {
            var mainForm = new Main();
            mainForm.MdiParent = this;
            mainForm.WindowState = FormWindowState.Maximized;
            mainForm.Show();

            if (ActiveMdiChild.Icon != null)
            {
                Bitmap bmp = new Bitmap(16, 16);
                bmp.MakeTransparent();
                using (Graphics gp = Graphics.FromImage(bmp))
                    gp.DrawIcon(ActiveMdiChild.Icon, new Rectangle(0, 0, 16, 16));
                this.MainMenuStrip.Items[0].Image = bmp;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog(this);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            options.ShowDialog(this);

        }

        private void editXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mainForm = new XMLDataEditor();
            mainForm.MdiParent = this;
            mainForm.WindowState = FormWindowState.Maximized;
            mainForm.Show();
            LayoutMdi(MdiLayout.TileVertical);

            if (ActiveMdiChild.Icon != null)
            {
                Bitmap bmp = new Bitmap(16, 16);
                bmp.MakeTransparent();
                using (Graphics gp = Graphics.FromImage(bmp))
                    gp.DrawIcon(ActiveMdiChild.Icon, new Rectangle(0, 0, 16, 16));
                this.MainMenuStrip.Items[0].Image = bmp;
            }
        }

        private void xPathExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mainForm = new XPathExtractor();
            mainForm.MdiParent = this;
            mainForm.WindowState = FormWindowState.Maximized;
            mainForm.Show();
            LayoutMdi(MdiLayout.TileVertical);

            if (ActiveMdiChild.Icon != null)
            {
                Bitmap bmp = new Bitmap(16, 16);
                bmp.MakeTransparent();
                using (Graphics gp = Graphics.FromImage(bmp))
                    gp.DrawIcon(ActiveMdiChild.Icon, new Rectangle(0, 0, 16, 16));
                this.MainMenuStrip.Items[0].Image = bmp;
            }
        }
    }
}
