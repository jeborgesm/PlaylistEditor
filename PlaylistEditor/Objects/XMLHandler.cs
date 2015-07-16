using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PlaylistEditor
{
    class XMLHandler
    {
        private static Mutex mut = new Mutex();

        public static void OpenCreateXMLFile(string filepath)
        {
            if (File.Exists(filepath) == false)
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter xmlWriter = XmlWriter.Create(filepath, xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("gameList");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
        }

        public static void writexml(Game game)//String filename, String title)
        {
            String filepath = Directory.GetCurrentDirectory() + "\\gamelist.xml";

            OpenCreateXMLFile(filepath);
            AddGameNode(game, filepath);

        }

        public static void AddGameNode(Game game, string filepath)
        {
            if (File.Exists(filepath) == false)
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter xmlWriter = XmlWriter.Create(filepath, xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("gameList");

                    xmlWriter.WriteStartElement("game");
                    xmlWriter.WriteElementString("path", game.path);
                    xmlWriter.WriteElementString("name", game.name);
                    xmlWriter.WriteElementString("desc", game.desc);
                    xmlWriter.WriteElementString("image", game.image);
                    xmlWriter.WriteElementString("releasedate", game.releasedate);
                    xmlWriter.WriteElementString("developer", game.developer);
                    xmlWriter.WriteElementString("publisher", game.publisher);
                    xmlWriter.WriteElementString("genre", game.genre);
                    xmlWriter.WriteElementString("players", game.players);
                    xmlWriter.WriteElementString("rating", game.rating);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
            else
            {
                mut.WaitOne();
                //if (HTTPHandler.WaitForFile(filepath))
                //{
                    XDocument xDocument = XDocument.Load(filepath);
                    XElement root = xDocument.Element("gameList");
                    if (root.HasElements)
                    {
                        IEnumerable<XElement> rows = root.Descendants("game");
                        XElement lastRow = rows.Last();

                        lastRow.AddAfterSelf(
                            new XElement("game",
                            new XElement("path", game.path),
                            new XElement("desc", game.desc),
                            new XElement("name", game.name),
                            new XElement("image", game.image),
                            new XElement("releasedate", game.releasedate),
                            new XElement("developer", game.developer),
                            new XElement("publisher", game.publisher),
                            new XElement("genre", game.genre),
                            new XElement("players", game.players),
                            new XElement("rating", game.rating)));

                        StreamWriter outStream = System.IO.File.CreateText(filepath);
                        xDocument.Save(outStream);
                        outStream.Close();
                    }
                    else
                    {
                        root.Add(
                            new XElement("game",
                            new XElement("path", game.path),
                            new XElement("desc", game.desc),
                            new XElement("name", game.name),
                            new XElement("image", game.image),
                            new XElement("releasedate", game.releasedate),
                            new XElement("developer", game.developer),
                            new XElement("publisher", game.publisher),
                            new XElement("genre", game.genre),
                            new XElement("players", game.players),
                            new XElement("rating", game.rating)));

                        StreamWriter outStream = System.IO.File.CreateText(filepath);
                        xDocument.Save(outStream);
                        outStream.Close();
                    }
                //}
                mut.ReleaseMutex();
            }

        }

        public static void UpdateGameXML(Game game, string filepath, bool insertnotfound)
        {
            OpenCreateXMLFile(filepath);

            XmlDocument doc = new XmlDocument();
            mut.WaitOne();
            //if (HTTPHandler.WaitForFile(filepath))
            //{
                using (Stream s = File.OpenRead(filepath))
                {
                    doc.Load(s);
                }

                //if (HTTPHandler.WaitForFile(filepath))
                //{
                    XmlNode node = doc.SelectSingleNode("/gameList/game/path[text()='" + game.path + "']");

                    if (node != null)
                    {

                        node = node.ParentNode;

                        node.SelectSingleNode("name").InnerText = game.name;
                        node.SelectSingleNode("desc").InnerText = game.desc;
                        node.SelectSingleNode("image").InnerText = game.image;
                        node.SelectSingleNode("releasedate").InnerText = game.releasedate;
                        node.SelectSingleNode("developer").InnerText = game.developer;
                        node.SelectSingleNode("publisher").InnerText = game.publisher;
                        node.SelectSingleNode("genre").InnerText = game.genre;
                        node.SelectSingleNode("players").InnerText = game.players;
                        node.SelectSingleNode("rating").InnerText = game.rating;

                        //using (var writer = new StreamWriter(filepath))
                        //{
                        //    doc.Save(writer);
                        //}

                        //doc.Save(filepath);

                        StreamWriter outStream = System.IO.File.CreateText(filepath);
                        doc.Save(outStream);
                        outStream.Close();
                    }
                //}
                else if (insertnotfound == true)
                {
                    AddGameNode(game, filepath);
                }
            //}
            mut.ReleaseMutex();
        }

        public static XmlDocument HTMLtoXML(string html)
        {
            string htmlCleaned = HTMLtoXMLString(html);

            // create document
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.XmlResolver = null;
            doc.LoadXml(htmlCleaned);
            return doc;
        }

        
        public static XPathNavigator HTMLtoXPathNav(string html)
        {
            string htmlCleaned = HTMLtoXMLString(html);

            XPathDocument doc = new XPathDocument(new StringReader(htmlCleaned));
            XPathNavigator nav = doc.CreateNavigator();

            return nav;
        }

        public static String HTMLtoXMLString(string html)
        {
            using (TextReader htmlReader = new StringReader(html))
            {
                // setup SgmlReader
                Sgml.SgmlReader sgmlReader = new Sgml.SgmlReader();
                sgmlReader.DocType = "HTML";
                sgmlReader.WhitespaceHandling = WhitespaceHandling.None;
                sgmlReader.CaseFolding = Sgml.CaseFolding.ToLower;
                sgmlReader.InputStream = htmlReader;
                sgmlReader.IgnoreDtd = false;

                return RemoveAllNamespaces(sgmlReader.ReadOuterXml());
            }
        }

        public static string FindXPath(XmlNode node)
        {
            StringBuilder builder = new StringBuilder();
            while (node != null)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + node.Name);
                        node = ((XmlAttribute)node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        int index = FindElementIndex((XmlElement)node);
                        builder.Insert(0, "/" + node.Name + "[" + index + "]");
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Document:
                        return builder.ToString();
                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            throw new ArgumentException("Node was not in a document");
        }

        private static int FindElementIndex(XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }
            XmlElement parent = (XmlElement)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlElement && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }

        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            //return xmlDocumentWithoutNs.ToString();
            return xmlDocumentWithoutNs.ToString(SaveOptions.DisableFormatting);
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        public static string getParentXPath(XmlNodeList XParentNodes, string XElement, string XElementName)
        {
            string elemXPath = "";
            string prevtag = ""; //holds the previous tag to create the duplicate tag index 
            int tagcount = 0; //holds the duplicate tag index

            foreach (XmlNode chld in XParentNodes)
            {
                string tag = chld.Name;

                if (tag == XElementName)//Only write to XPath if the tag is the same not if it is a sibling. 
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
                else if (chld.OuterXml.Contains(XElement)) //Only run if the current node is not the child with content but one further down in the hierarchy
                {
                    //Recursive Search of children nodes when the current node has significative children
                    //->Each loop in parent should look for the child that contains the selected element.
                    //from that node search again for the child that contains the selected item.
                    elemXPath = "/" + tag + "[1]" + getParentXPath(chld.ChildNodes, XElement, tag);
                }
                prevtag = tag;
                if (chld.OuterXml == XElement) { break; }
            }

            return elemXPath;
        }

        public static XmlNodeList FullNodeList(XmlDocument xmldoc)
        {
            foreach (XmlNode xmlnode in xmldoc.ChildNodes)
            {                    
                if (xmlnode.NodeType == XmlNodeType.Element)
                {
                    XmlNode getchildren = getXMLChildren(xmlnode);
                    if (getchildren != null)
                    {
                        xmldoc.AppendChild(getchildren);
                    }
                }
            }
            return xmldoc.ChildNodes;
        }

        private static XmlNode getXMLChildren(XmlNode inNode)
        {
            foreach (XmlNode xmlnode in inNode.ChildNodes)
            {
                if (xmlnode.NodeType == XmlNodeType.Element)
                {
                    inNode.AppendChild(xmlnode);
                    Console.WriteLine(xmlnode.InnerXml);

                    if (xmlnode.HasChildNodes)
                    {
                        inNode.AppendChild(getXMLChildren(xmlnode));
                    }
                }
            }
            return inNode;
        }

        public static void RecursiveWalk(XPathNavigator navigator, string selected)
        {

            switch (navigator.NodeType)
            {
                case XPathNodeType.Element:
                    if (navigator.Prefix == String.Empty)
                        Console.WriteLine("<{0}>", navigator.LocalName);
                    else
                        Console.Write("<{0}:{1}>", navigator.Prefix, navigator.LocalName);
                    Console.WriteLine("\t" + navigator.NamespaceURI);
                    break;
                //case XPathNodeType.Text:
                //    Console.WriteLine("\t" + navigator.Value);
                //    break;
            }

            if (navigator.MoveToFirstChild())
            {
                do
                {
                    if (navigator.OuterXml == selected)
                    { break; }
                    RecursiveWalk(navigator, selected);
                } while (navigator.MoveToNext());

                navigator.MoveToParent();
                if (navigator.NodeType == XPathNodeType.Element)
                    Console.WriteLine("</{0}>", navigator.Name);
            }
            else
            {
                if (navigator.NodeType == XPathNodeType.Element)
                {
                    Console.WriteLine("</{0}>", navigator.Name);
                }
            }
        }
        
    }
}
