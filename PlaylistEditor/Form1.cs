using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace PlaylistEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("Test.xml") == false)
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter xmlWriter = XmlWriter.Create("Test.xml", xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("School");

                    xmlWriter.WriteStartElement("Student");
                    xmlWriter.WriteElementString("FirstName", firstName);
                    xmlWriter.WriteElementString("LastName", lastName);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load("Test.xml");
                XElement root = xDocument.Element("School");
                IEnumerable<XElement> rows = root.Descendants("Student");
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                   new XElement("Student",
                   new XElement("FirstName", firstName),
                   new XElement("LastName", lastName)));
                xDocument.Save("Test.xml");
            }
        }
    }
}
