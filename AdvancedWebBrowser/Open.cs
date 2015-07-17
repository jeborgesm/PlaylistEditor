// Jaime Borges 2015
// AdvancedWebBrowser - Windows Control
// Based on code by Goga Claudia - WBrowser 2009

using System;
using System.Windows.Forms;

namespace AdvancedWebBrowser
{
	public partial class Open : Form
	{
		WebBrowser wb;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wb"></param>
		public Open(WebBrowser wb)
		{
			this.wb = wb;
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			wb.Navigate(textBox1.Text);
			this.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			OpenFileDialog o = new OpenFileDialog();
			o.Filter = "Web Page|*.htm*; *.asp; *.*html; *.xml; *.hta|Text File|*.txt; *.nfo; *.log|Image|*.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp; *.gif|All Files|*.*";
			o.Title = "Please enter a URL or browse to a local file.";
			if (o.ShowDialog() == DialogResult.OK)
				textBox1.Text = o.FileName;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				wb.Navigate(textBox1.Text);
				this.Close();
			}
		}
	}
}
