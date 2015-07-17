using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

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

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Text = e.Url.ToString();
            this.document = wb.Document;
            this.document.Click += new HtmlElementEventHandler(document_Click);
            this.document.MouseOver += new HtmlElementEventHandler(document_MouseOver);
            this.document.MouseLeave += new HtmlElementEventHandler(document_MouseLeave);

            XmlDocument htmldoc = XMLHandler.HTMLtoXML(wb.DocumentText);
            this.txtExtracted.Text = htmldoc.OuterXml;
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
            //Change the style of the selected HTML element
            HtmlElement selelement = ChangeHTMLTagStyle(e.ToElement);

            //string path = HTMLHandler.XPathHTMLtoXML(e.ToElement);
            //this.txtXPath.Text = path;
            //this.txtExtracted.Text = ScrapeHandler.ScrapeXMLXPath(wb.DocumentText, path);
        }

        private void document_Click(object sender, HtmlElementEventArgs e)
        {
            string path = HTMLHandler.XPathHTMLtoXML(((HtmlDocument)sender).ActiveElement);
            this.txtXPath.Text = path;
            this.txtExtracted.Text = ScrapeHandler.ScrapeXMLXPath(wb.DocumentText, path);
             //stop mouse events moving on to the HTML doc

            e.ReturnValue = false;

        }


        private HtmlElement ChangeHTMLTagStyle(HtmlElement element )
        {
            if (!this.elementStyles.ContainsKey(element))
            {
                string style = element.Style;
                this.elementStyles.Add(element, style);
                element.Style = style + "; border-color: red; border-style: solid; border-width: medium;";
            }

            return element;
        }

        private void btnXPATH_Click(object sender, EventArgs e)
        {
            wb = this.advancedWebBrowser1.ActiveWebBrowser;
            //MessageBox.Show(wb.Url.ToString());
            //MessageBox.Show(ScrapeHandler.ScrapeValueXPath(wb.DocumentText, this.txtXPath.Text));
            this.txtExtracted.Text = ScrapeHandler.ScrapeValueListXPath(wb.DocumentText, this.txtXPath.Text);
        }


    }
}
