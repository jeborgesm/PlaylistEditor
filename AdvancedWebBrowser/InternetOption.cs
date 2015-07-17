// Jaime Borges 2015
// AdvancedWebBrowser - Windows Control
// Based on code by Goga Claudia - WBrowser 2009

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AdvancedWebBrowser
{
	public partial class InternetOption : Form
	{
		String adresa;

		/// <summary>
		/// 
		/// </summary>
		public Font font;

		/// <summary>
		/// 
		/// </summary>
		public Color forecolor, backcolor;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="adresa"></param>
		public InternetOption(String adresa)
		{
			this.adresa = adresa;
			InitializeComponent();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			homepage.Text = "about:blank";
		}

		private void btnUseCurrent_Click(object sender, EventArgs e)
		{
			homepage.Text = adresa;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			FontDialog dlg = new FontDialog();
			if (dlg.ShowDialog() == DialogResult.OK)
				font = dlg.Font;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			if (c.ShowDialog() == DialogResult.OK)
				forecolor = c.Color;
		}

		private void button7_Click(object sender, EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			if (c.ShowDialog() == DialogResult.OK)
				backcolor = c.Color;
		}


	}
}
