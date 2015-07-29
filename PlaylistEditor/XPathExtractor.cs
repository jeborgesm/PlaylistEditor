using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace PlaylistEditor
{
    public partial class XPathExtractor : Form
    {
        private string elementTag;
        private string elementHtml;
        private string selectedbody;
        private string activeURL;
        WebBrowser wb;
        private IDictionary<HtmlElement, string> elementStyles = new Dictionary<HtmlElement, string>();

        public XPathExtractor()
        {
            InitializeComponent();
        }

        private void XPathExtractor_Load(object sender, EventArgs e)
        {
            wb = this.advancedWebBrowser1.ActiveWebBrowser;
            ((this.advancedWebBrowser1.Controls)[1]).Visible = false;
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            wb.Document.Click += new HtmlElementEventHandler(document_Click);
            wb.Document.MouseOver += new HtmlElementEventHandler(document_MouseOver);
            wb.Document.MouseLeave += new HtmlElementEventHandler(document_MouseLeave);
            //WebBrowser wb = (WebBrowser)(((TabControl)this.advancedWebBrowser1.Controls["browserTabControl"])).SelectedTab.Controls[0];
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (wb.Url.ToString() != activeURL)
            {
                activeURL = wb.Url.ToString();

                Text = activeURL;//Header updated with current URL
                XmlDocument htmldoc = XMLHandler.HTMLtoXML(wb.Document.Body.Parent.OuterHtml);

                this.txtExtracted.Text = XMLHandler.Pretty(htmldoc);
                populateTreeview(htmldoc);
            }
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
            selectedbody = ((HtmlDocument)sender).Body.Parent.OuterHtml;
            elementHtml = e.ToElement.OuterHtml;
            elementTag = e.ToElement.TagName;

            //Change the style of the selected HTML element
            HtmlElement selelement = ChangeHTMLTagStyle(e.ToElement);

            //string path = HTMLHandler.XPathHTMLtoXML(e.ToElement);
            //this.txtXPath.Text = path;
            //this.txtExtracted.Text = ScrapeHandler.ScrapeXMLXPath(wb.DocumentText, path);
        }

        private void document_Click(object sender, HtmlElementEventArgs e)
        {
            //string path = HTMLHandler.XPathHTMLtoXML(((HtmlDocument)sender).ActiveElement);
            string path = HTMLHandler.XPathHTMLtoXML(selectedbody, elementHtml, elementTag);
            this.txtXPath.Text = path;

            //string gethtml = HTTPHandler.HttpGet(wb.Url.ToString());
            //this.txtExtracted.Text = ScrapeHandler.ScrapeXMLXPath(gethtml, path);

            //this.txtExtracted.Text = ScrapeHandler.ScrapeXMLXPath(wb.DocumentText, path);
            this.txtExtracted.Text = ScrapeHandler.ScrapeXMLXPath(selectedbody, path);

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
            //wb = this.advancedWebBrowser1.ActiveWebBrowser;
            //MessageBox.Show(wb.Url.ToString());
            //MessageBox.Show(ScrapeHandler.ScrapeValueXPath(wb.DocumentText, this.txtXPath.Text));
            HtmlDocument doc = wb.Document;

            XmlDocument htmldoc = XMLHandler.HTMLtoXML(doc.Body.Parent.OuterHtml);
            this.txtExtracted.Text = ScrapeHandler.ScrapeValueListXPath(htmldoc, this.txtXPath.Text);
            //this.txtExtracted.Text = ScrapeHandler.ScrapeValueListXPath(wb.DocumentText, this.txtXPath.Text);
        }

        //Open the XML file, and start to populate the treeview
        private void populateTreeview(XmlDocument xDoc)
        {
            if (xDoc != null)
            {
                try
                {
                    //Just a good practice -- change the cursor to a 
                    //wait cursor while the nodes populate
                    this.Cursor = Cursors.WaitCursor;

                    tvXML.BeginUpdate();
                    //Now, clear out the treeview, 
                    //and add the first (root) node
                    tvXML.Nodes.Clear();
                    tvXML.Nodes.Add(new
                      TreeNode(xDoc.DocumentElement.Name));
                    TreeNode tNode = new TreeNode();
                    tNode = (TreeNode)tvXML.Nodes[0];
                    //We make a call to addTreeNode, 
                    //where we'll add all of our nodes
                    addTreeNode(xDoc.DocumentElement, tNode);
                    //Expand the treeview to show all nodes
                    tvXML.ExpandAll();
                    tvXML.EndUpdate();
                    tvXML.Nodes[0].EnsureVisible();
                }
                catch (XmlException xExc)
                //Exception is thrown is there is an error in the Xml
                {
                    MessageBox.Show(xExc.Message);
                }
                catch (Exception ex) //General exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Cursor = Cursors.Default; //Change the cursor back
                }
            }
        }
        //This function is called recursively until all nodes are loaded
        private void addTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList xNodeList;
            if (xmlNode.HasChildNodes) //The current node has children
            {
                xNodeList = xmlNode.ChildNodes;
                for (int x = 0; x <= xNodeList.Count - 1; x++)
                //Loop through the child nodes
                {
                    xNode = xmlNode.ChildNodes[x];
                    treeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = treeNode.Nodes[x];
                    addTreeNode(xNode, tNode);
                }
            }
            else //No children, so add the outer xml (trimming off whitespace)
                treeNode.Text = xmlNode.OuterXml.Trim();
        }

    }
}
