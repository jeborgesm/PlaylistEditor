using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class XPathExtractor : Form
    {
        private HtmlDocument document;
        private IDictionary<HtmlElement, string> elementStyles = new Dictionary<HtmlElement, string>();

        public XPathExtractor()
        {
            InitializeComponent();
        }

        private void XPathExtractor_Load(object sender, EventArgs e)
        {
            //WebBrowser wb = (WebBrowser)(((TabControl)this.advancedWebBrowser1.Controls["browserTabControl"])).SelectedTab.Controls[0];
            //wb.DocumentCompleted
        }

        private void btnURL_Click(object sender, EventArgs e)
        {
            WebBrowser wb = this.advancedWebBrowser1.ActiveWebBrowser;
            MessageBox.Show(wb.Url.ToString());
        }

        //private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    this.Text = e.Url.ToString();
        //    this.document = this.advancedWebBrowser1.Document;
        //    this.document.MouseOver += new HtmlElementEventHandler(document_MouseOver);
        //    this.document.MouseLeave += new HtmlElementEventHandler(document_MouseLeave);
        //}

        //private void document_MouseLeave(object sender, HtmlElementEventArgs e)
        //{
        //    HtmlElement element = e.FromElement;
        //    if (this.elementStyles.ContainsKey(element))
        //    {
        //        string style = this.elementStyles[element];
        //        this.elementStyles.Remove(element);
        //        element.Style = style;
        //    }
        //}

        //private void document_MouseOver(object sender, HtmlElementEventArgs e)
        //{
        //    HtmlElement element = e.ToElement;
        //    if (!this.elementStyles.ContainsKey(element))
        //    {
        //        string style = element.Style;
        //        this.elementStyles.Add(element, style);
        //        element.Style = style + "; background-color: #ffc;";
        //        this.Text = element.Id ?? "(no id)";
        //    }
        //}


    }
}
