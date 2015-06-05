using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlaylistEditor.Properties;

namespace PlaylistEditor
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            // Read settings
            this.txtMAMERoms.Text = Settings.Default.MAME_Roms_Directory;
            this.txtUserAgent.Text = Settings.Default.UserAgent;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Write settings
            Properties.Settings.Default.MAME_Roms_Directory = this.txtMAMERoms.Text;
            Properties.Settings.Default.UserAgent = this.txtUserAgent.Text;

            // Save settings
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMAMEPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtMAMERoms.Text;
            DialogResult result = folderBrowserDialog.ShowDialog(this);
            if (result == DialogResult.OK) // The user selected a folder and pressed the OK button.
            {
                 this.txtMAMERoms.Text = folderBrowserDialog.SelectedPath;
            }
        }

    }
}
