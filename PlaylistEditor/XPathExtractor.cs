using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

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
            //using (TextReader tr = new StringReader(wb.DocumentText))
            //{
            //    XmlDocument htmldoc = XMLHandler.HTMLtoXML(tr);
            //    this.txtExtracted.Text = htmldoc.OuterXml;
            //}
            this.txtExtracted.Text = wb.DocumentText;
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

        private string getXPath(HtmlElement elem)
        {
            string elemXPath = "";
            string children = "";
            List<string> ParentTagsList = new List<string>();
            List<string> TagsList = new List<string>();

            while (elem.Parent != null)
            {
                //from actual node go to parent
                HtmlElement prnt = elem.Parent;
                ParentTagsList.Add(prnt.TagName);

                children += "|" + prnt.TagName;

                //loop through all the children
                foreach (HtmlElement chld in prnt.Children)
                {
                    //=> this code is retrieving all the paths to the root. 
                    //=>I will create an array with it instead of looping trough all the childern of the parent
                    TagsList.Add(chld.TagName);
                }
                elem = elem.Parent;
                TagsList.Add(elem.TagName);
            }


            string prevtag = ""; //holds the previous tag to create the duplicate tag index 
            int tagcount = 1; //holds the duplicate tag index
            foreach (string tag in TagsList)
            {
                if (tag == prevtag)
                {
                    //if (tagcount == 1)
                    //{
                    //    tagcount++;
                    //    int prvtaglength = ("/" + tag + "/").Length;
                    //    if (prvtaglength > elemXPath.Length - prvtaglength)
                    //    {
                    //        elemXPath = "/" + tag + "[" + tagcount + "]";
                    //    }
                    //    else
                    //    {
                    //        elemXPath = elemXPath.Substring(prvtaglength, elemXPath.Length - prvtaglength);
                    //        elemXPath = "/" + tag + "[" + tagcount + "]/" + elemXPath;
                    //    }
                    //}
                    //else
                    //{
                        int prvtaglength = ("/" + tag + "[" + tagcount + "]/").Length;
                        if (prvtaglength > elemXPath.Length - prvtaglength)
                        {
                            elemXPath = "/" + tag + "[" + tagcount + "]";
                        }
                        else
                        {
                            elemXPath = elemXPath.Substring(prvtaglength, elemXPath.Length - prvtaglength);

                            tagcount++;
                            elemXPath = "/" + tag + "[" + tagcount + "]/" + elemXPath;
                        }
                    //}
                }
                else
                {
                    tagcount = 1;
                    elemXPath = "/" + tag + "[" + tagcount + "]" + elemXPath;
                    
                }
                prevtag = tag;

            }

            this.txtExtracted.Text = children + Environment.NewLine + elemXPath.ToLower();
            return elemXPath.ToLower(); 
        }

        private string getShortXPath(HtmlElement elem)
        {
            HtmlElement selelem = elem;
            string elemXPath = "";
            string prntXPath = "";
            string children = "";
            List<string> ParentTagsList = new List<string>();
            List<string> TagsList = new List<string>();

            //loop through the parents until reaching root
            while (elem.Parent != null)
            {
                //from actual node go to parent
                HtmlElement prnt = elem.Parent;
                ParentTagsList.Add(prnt.TagName);
                prntXPath = getParentLocation(prnt) + prntXPath;
                elem = elem.Parent;
            }
            
            //Add selected element to path;
            elemXPath = getParentLocation(selelem);


            //Join the selected path with the route to root
            elemXPath = prntXPath + elemXPath; 

            ////br[1]/div[1]/ul[8]/li[1]/a
            //TODO: The XPath should be shorten to the nearest unique value and use // (from root XPath indicator)
            this.txtExtracted.Text = children + Environment.NewLine + elemXPath.ToLower();
            return elemXPath.ToLower();
        }

        private string getXpathNavXPath(HtmlElement elem)
        {
            string prntXPath = "";
            List<string> ParentTagsList = new List<string>();

            string XMLelem = "";
            XPathNavigator xpathNav = null;

            //Convert selected element to XML
            XMLelem = XMLHandler.HTMLtoXMLString(elem.OuterHtml);

            //loop through the parents until reaching root
            while (elem.Parent != null)
            {   //(Using HTMLElement)
                //from actual node go to parent 
                //HtmlElement prnt = elem.Parent;
                //ParentTagsList.Add(prnt.TagName);
                //prntXPath = getParentLocation(prnt) + prntXPath;

                //(Using HTMLDocument)
                XmlDocument prntXDoc = XMLHandler.HTMLtoXML(elem.Parent.OuterHtml);
                string Xelem = XMLHandler.HTMLtoXMLString(elem.OuterHtml);
                ParentTagsList.Add(prntXDoc.DocumentElement.Name);
                prntXPath = getParentXPath(prntXDoc, Xelem, elem.TagName.ToLower()) + prntXPath;
                elem = elem.Parent;
            }

            ////br[1]/div[1]/ul[8]/li[1]/a
            //TODO: The XPath should be shorten to the nearest unique value and use // (from root XPath indicator)
            this.txtExtracted.Text = prntXPath;
            return prntXPath.ToLower();
        }

        private string getParentXPath(XmlDocument XParent, string XElement, string XElementName)
        {
            string elemXPath = "";
            string prevtag = ""; //holds the previous tag to create the duplicate tag index 
            int tagcount = 0; //holds the duplicate tag index

            foreach (XmlNode chld in XParent.FirstChild.ChildNodes)
            {
                string tag = chld.Name;

                //prntXDoc.FirstChild.ChildNodes[2].OuterXml.Contains(Xelem);
                //TODO: each loop in parent should look for the child that contains the selected element.
                //from that node search again for the child that contains the selected item.
                //This loop could be created from root instead of from element backwards... Needs Testing 7-14-2015

                if (tag == XElementName)//Only write to XPath if the tag is the same not if it is a sibling. 7-13-2015
                {
                    tagcount++;
                    int prvtaglength = ("/" + tag + "[" + tagcount + "]/").Length;
                    if (prvtaglength > elemXPath.Length - prvtaglength)
                    {
                        elemXPath = "/" + tag + "[" + tagcount + "]";
                    }
                    else
                    {
                        elemXPath = elemXPath.Substring(prvtaglength, elemXPath.Length - prvtaglength);

                        tagcount++;
                        elemXPath = elemXPath + "/" + tag + "[" + tagcount + "]";
                    }
                }
                prevtag = tag;
                if (chld.OuterXml == XElement) { break; }
            }

            return elemXPath;
        }

        private string getParentLocation(HtmlElement selelem)
        {
            string elemXPath = "";
            string prevtag = ""; //holds the previous tag to create the duplicate tag index 
            int tagcount = 0; //holds the duplicate tag index
            if (selelem.Parent != null && selelem.Parent.Children != null)
            {
                foreach (HtmlElement chld in selelem.Parent.Children)
                {
                    string tag = chld.TagName;
                    if (tag == selelem.TagName)//Only write to XPath if the tag is the same not if it is a sibling. 7-13-2015
                    {
                        //if (tag == prevtag)
                        //{
                            tagcount++;
                            int prvtaglength = ("/" + tag + "[" + tagcount + "]/").Length;
                            if (prvtaglength > elemXPath.Length - prvtaglength)
                            {
                                elemXPath = "/" + tag + "[" + tagcount + "]";
                            }
                            else
                            {
                                elemXPath = elemXPath.Substring(prvtaglength, elemXPath.Length - prvtaglength);

                                tagcount++;
                                elemXPath = elemXPath + "/" + tag + "[" + tagcount + "]";
                            }
                        //}
                        //else
                        //{
                        //    tagcount = 1;
                        //    elemXPath = elemXPath + "/" + tag + "[" + tagcount + "]";
                        //}
                    }
                    prevtag = tag;
                    if (chld.InnerHtml == selelem.InnerHtml) { break; }
                }
            }
            return elemXPath;
        }

        private void document_MouseOver(object sender, HtmlElementEventArgs e)
        {
            string path = getXpathNavXPath(e.ToElement);

            HtmlElement selelement = ChangeHTMLTagStyle(e.ToElement);
            //string path = getShortXPath(selelement);



            //string path = getXPath((HtmlDocument)sender, selelement);

            

            //XmlNode foundnode = null;

            //using (TextReader tr = new StringReader(wb.DocumentText))
            //{
            //    XmlDocument htmldoc = XMLHandler.HTMLtoXML(tr);

            //    XmlNodeList xmlNL = XMLHandler.FullNodeList(htmldoc);


            //    //XmlNodeList htmlnodes = htmldoc.SelectNodes("/");

            //    //var list = htmldoc.SelectNodes("/")
            //    //                .Cast<XmlNode>()
            //    //                .ToList();

            //    XmlElement elm = htmldoc.DocumentElement;
            //    XmlNodeList htmlnodes = elm.ChildNodes;

            //    //XmlReader rdr = XmlReader.Create(new System.IO.StringReader(htmldoc.InnerXml));
            //    //while (rdr.Read())
            //    //{
            //    //    if (rdr.NodeType == XmlNodeType.Element)
            //    //    {
            //    //        Console.WriteLine(rdr.LocalName);
            //    //    }
            //    //}

            //    foreach (XmlNode htmlnode in htmlnodes)
            //    {
            //        if (htmlnode.InnerXml == selelement.InnerHtml && selelement.InnerHtml.Length>0)
            //        {
            //            foundnode = htmlnode;
            //            this.txtExtracted.Text = foundnode.InnerText;
            //            break;
            //        }
            //    }
                
            //    // this.txtExtracted.Text = htmldoc.OuterXml;
            //}



            //using (TextReader tr = new StringReader(e.ToElement.OuterHtml))
            //{
            //    XmlDocument htmldoc = XMLHandler.HTMLtoXML(tr);

            //    XmlDocument doc = new XmlDocument();
            //    XmlNode docNode = doc.CreateNode(XmlNodeType.Element, e.ToElement.TagName, "");
            //}

            //string elemspath = "";
            //foreach (HtmlElement el in elems)
            //{
            //    elemspath = elemspath + "/" + el.TagName;
            //    if (selelement == el)
            //        break;
            //}




 

                //element.Style = style + "; background-color: #ffc;";
                this.txtXPath.Text = path;
                //this.txtExtracted.Text = ScrapeHandler.ScrapeValueXPath(wb.DocumentText, path);
                //this.Text = element.Id ?? "(no id)";
            //}
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
