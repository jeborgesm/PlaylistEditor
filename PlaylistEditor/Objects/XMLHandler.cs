using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

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

        public static XmlDocument HTMLtoXML(TextReader reader)
        {

            // setup SgmlReader
            Sgml.SgmlReader sgmlReader = new Sgml.SgmlReader();
            sgmlReader.DocType = "HTML";
            sgmlReader.WhitespaceHandling = WhitespaceHandling.All;
            sgmlReader.CaseFolding = Sgml.CaseFolding.ToLower;
            sgmlReader.InputStream = reader;

            // create document
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.XmlResolver = null;
            doc.Load(sgmlReader);
            return doc;
        }
    }
}
