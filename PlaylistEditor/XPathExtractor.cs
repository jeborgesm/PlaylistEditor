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

                HtmlElement elem = element;
                string path = elem.TagName;
                while(elem.Parent != null)
                {
                    //from actual node go to parent
                    HtmlElement prnt = element.Parent;
                    
                    int chldcount = 1;
                    //loop through all the children
                    foreach (HtmlElement chld in prnt.Children)
                    {
                        //verify if the child is of the same type of the actual node
                        if (elem.TagName == chld.TagName)
                        {
                            //if the child node is the same type but not equal add 1 to the path array and go to next child
                            if (elem.InnerHtml != chld.InnerHtml)
                            {
                                ++chldcount;
                                path = elem.TagName + "[" + chldcount + "]/" + path;
                            }
                            else//if the child node is equal to the actual node
                            {
                                //if the array is not empty add one to the path array and go to next parent
                                if (chldcount > 1)
                                {
                                    ++chldcount;
                                    path = elem.TagName + "[" + chldcount + "]/" + path;
                                }
                                else
                                {
                                    //if the array is empty add the item to the path and go to next parent
                                    path = elem.TagName + "/" + path;
                                }
                            }
                        }
                        else
                        {
                            path = elem.Parent.TagName + "/" + path; 
                        }
                    }

                    //path = elem.Parent.TagName + "/" + path; 

                    elem = elem.Parent;

                    //if (element.TagName == element.Parent.TagName)
                    //{
                    //    path = element.TagName + "[1]/" + path; 
                    //}
                    //else
                    //{
                    //path = elem.Parent.TagName + "/" + path; 
                    //}
                }
                path = "/" + path.ToLower();

                element.Style = style + "; background-color: #ffc;";
                this.txtXPath.Text = path;
                this.txtExtracted.Text = ScrapeHandler.ScrapeValueXPath(wb.DocumentText, path);
                //this.Text = element.Id ?? "(no id)";
            }
        }

        private void btnXPATH_Click(object sender, EventArgs e)
        {
            wb = this.advancedWebBrowser1.ActiveWebBrowser;
            //MessageBox.Show(wb.Url.ToString());
            MessageBox.Show(ScrapeHandler.ScrapeValueXPath(wb.DocumentText, this.txtXPath.Text));
        }


    }
}
