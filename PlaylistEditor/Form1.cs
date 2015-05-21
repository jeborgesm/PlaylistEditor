﻿using System;
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
            //"C:\Users\Jaime\.emulationstation\gamelists\mame\gamelist.xml"
            //"gamelists\\mame\\gamelist.xml"
            String filepath = @"C:\Users\Jaime\.emulationstation\gamelists\mame\gamelist.xml";

//                <game>
            //        <path>C:/Users/Jaime/.emulationstation/roms/mame-libretro/sf/sf2049.zip</path>
            //        <name>San Francisco Rush 2049</name>
            //        <desc>San Francisco Rush 2049 is the third game in the Rush series, sequel to San Francisco Rush and Rush 2: Extreme Racing USA.
            //The game features a futuristic representation of San Francisco and an arcade-style physics engine. It also features a multiplayer mode for up to four players and Rumble Pak support on the Nintendo 64 port. A major difference in game play compared to predecessors in the series is the ability to extend wings from the cars in midair and glide. As with previous titles in the franchise, Rush 2049 features a stunt mode in which the player scores points for complex mid-air maneuvers and successful landings. There is also a multiplayer deathmatch battle mode. There are six race tracks, four stunt arenas, eight battle arenas, and one unlockable obstacle course named The Gauntlet. The single player race mode places emphasis on outlandish and death-defying shortcuts in each track. The game has a soundtrack mostly comprising techno music.</desc>
            //        <image>~/.emulationstation/downloaded_images/mame/sf2049-image.jpg</image>
            //        <releasedate>20000906T000000</releasedate>
            //        <developer>Midway</developer>
            //        <publisher>Midway</publisher>
            //        <genre>Racing</genre>
            //        <players>4+</players>
            //        <rating>0.5</rating>
            //    </game>

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
                XDocument xDocument = XDocument.Load(filepath);
                XElement root = xDocument.Element("gameList");
                IEnumerable<XElement> rows = root.Descendants("game");
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                   new XElement("Student",
                   new XElement("FirstName", firstName),
                   new XElement("LastName", lastName)));
                xDocument.Save(filepath);
            }
        }

        static void DirSearch(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        Console.WriteLine(f);
                    }
                    DirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        public static string HttpGet(string URI)
        {
            String ProxyString = "";
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static string HttpPost(string URI, string Parameters)
        {
            String ProxyString = "";
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }
    }
}