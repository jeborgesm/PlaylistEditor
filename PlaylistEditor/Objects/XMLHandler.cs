using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace PlaylistEditor
{
    class XMLHandler
    {
        public static void writexml(Game game)//String filename, String title)
        {
            String filepath = Directory.GetCurrentDirectory() + "\\gamelist.xml";

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
                XDocument xDocument = XDocument.Load(filepath);
                XElement root = xDocument.Element("gameList");
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
                xDocument.Save(filepath);
            }
        }
    }
}
