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
        WebBrowser wb;
        private IDictionary<HtmlElement, string> elementStyles = new Dictionary<HtmlElement, string>();

        public XPathExtractor()
        {
            InitializeComponent();
        }

        private void XPathExtractor_Load(object sender, EventArgs e)
        {
            wb = this.advancedWebBrowser1.ActiveWebBrowser;
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            //WebBrowser wb = (WebBrowser)(((TabControl)this.advancedWebBrowser1.Controls["browserTabControl"])).SelectedTab.Controls[0];
            //wb.DocumentCompleted
        }

        private void btnURL_Click(object sender, EventArgs e)
        {
            wb = this.advancedWebBrowser1.ActiveWebBrowser;
            //MessageBox.Show(wb.Url.ToString());
            MessageBox.Show(ScrapeHandler.ScrapeValueXPath(wb.DocumentText, this.txtXPath.Text));
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Text = e.Url.ToString();
            this.document = wb.Document;
            this.document.MouseOver += new HtmlElementEventHandler(document_MouseOver);
            this.document.MouseLeave += new HtmlElementEventHandler(document_MouseLeave);
        }

        private void document_MouseLeave(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = e.FromElement;
            if (this.elementStyles.ContainsKey(element))
            {
                string style = this.elementStyles[element];
                this.elementStyles.Remove(element);
                element.Style = style;
            }
        }

        private void document_MouseOver(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = e.ToElement;
            if (!this.elementStyles.ContainsKey(element))
            {
                string style = element.Style;
                this.elementStyles.Add(element, style);

                string path = element.TagName;
                HtmlElement elem = element;
                while(elem.Parent != null)
                {
                    if (element.TagName == element.Parent.TagName)
                    {
                        path = element.TagName + "[1]/" + path; 
                    }
                    else
                    {
                        path = element.Parent.TagName + "/" + path; 
                    }
                    
                    elem = elem.Parent;
                }
                path = path.ToLower();

                element.Style = style + "; background-color: #ffc;";
                this.txtXPath.Text = path;
                //this.Text = element.Id ?? "(no id)";
            }
        }


    }
}
