using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace PlaylistEditor
{
    class HTMLHandler
    {
        public static string XPathHTML(HtmlElement elem)
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

            //this.txtExtracted.Text = children + Environment.NewLine + elemXPath.ToLower();
            return elemXPath.ToLower();
        }

        public static string XPathHTMLSimple(HtmlElement elem)
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
            //this.txtExtracted.Text = children + Environment.NewLine + elemXPath.ToLower();
            return elemXPath.ToLower();
        }

        public static string XPathHTMLtoXMLBackwards(HtmlElement elem)
        {
            string prntXPath = "";
            List<string> ParentTagsList = new List<string>();

            string XMLelem = "";
            XPathNavigator xpathNav = null;

            //Convert selected element to XML
            XMLelem = XMLHandler.HTMLtoXMLString(elem.OuterHtml);

            //TODO!! ==V
            //The search has to be done after the complete document is in xml not using parent and intermediate conversions
            //the result is not the same

            XmlDocument prntXDoc = null;
            //loop through the parents until reaching root
            while (elem.Parent != null)
            {   //(Using HTMLElement)
                //from actual node go to parent 
                //HtmlElement prnt = elem.Parent;
                //ParentTagsList.Add(prnt.TagName);
                //prntXPath = getParentLocation(prnt) + prntXPath;

                //(Using HTMLDocument)
                prntXDoc = XMLHandler.HTMLtoXML(elem.Parent.OuterHtml);
                string Xelem = XMLHandler.HTMLtoXMLString(elem.OuterHtml);
                ParentTagsList.Add(prntXDoc.DocumentElement.Name);
                prntXPath = XMLHandler.getParentXPath(prntXDoc.FirstChild.ChildNodes, Xelem, elem.TagName.ToLower()) + prntXPath;
                elem = elem.Parent;
            }

            if (prntXDoc != null)
            {
                prntXPath = "/" + prntXDoc.FirstChild.Name + "[1]" + prntXPath;
            }

            ////br[1]/div[1]/ul[8]/li[1]/a
            ///html[1]/body[1]/br[1]/p[1]/hr[1]/center[1]/hr[1]/p[1]/p[1]
            //TODO: The XPath should be shorten to the nearest unique value and use // (from root XPath indicator)
            //this.txtExtracted.Text = prntXPath;
            return prntXPath.ToLower();
        }

        public static string XPathHTMLtoXML(string html, string elem, string tagname)
        {
            string prntXPath = "";
            List<string> ParentTagsList = new List<string>();

            string XMLelem = "";

            //Convert selected element to XML
            XMLelem = XMLHandler.HTMLtoXMLString(elem);

            //TODO!! ==V
            //The search has to be done after the complete document is in xml not using parent and intermediate conversions
            //the result is not the same

            XmlDocument prntXDoc = null;

            prntXDoc = XMLHandler.HTMLtoXML(html);
            prntXPath = XMLHandler.getChildrenXPath(prntXDoc.FirstChild.ChildNodes, XMLelem) + prntXPath;
            

            if (prntXDoc != null)
            {
                prntXPath = "//" + prntXDoc.FirstChild.Name + "[1]" + prntXPath;
            }

            ////br[1]/div[1]/ul[8]/li[1]/a
            ///html[1]/body[1]/br[1]/p[1]/hr[1]/center[1]/hr[1]/p[1]/p[1]
            //TODO: The XPath should be shorten to the nearest unique value and use // (from root XPath indicator)
            //this.txtExtracted.Text = prntXPath;
            return prntXPath.ToLower();
        }


        public static string getParentLocation(HtmlElement selelem)
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
    }
}
