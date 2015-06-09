using PlaylistEditor.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace PlaylistEditor
{
    public partial class Main : Form
    {
        static bool stopProcess = false;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            SetStatus("Status: MAME Scrape Ready");
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetStatus("Status");
            stopProcess = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try {
                stopProcess = false;
                SetStatus("Process START!");

                //Create a new Thread start object passing the method to be started
                ThreadStart oThreadStart = new ThreadStart(this.DoWork);

                //Create a new threat passing the start details
                Thread oThread = new Thread(oThreadStart);

                //Optionally give the Thread a name
                oThread.Name = "Processing Thread";

                //Start the thread
                oThread.Start();
            }
            catch (System.Exception excpt)
            {
                ResultsBox.Text += excpt.Message + Environment.NewLine;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopProcess = true;
        }

        private void btnGetGameDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ResultsBox.Text += ScrapeHandler.getDetailsArcadeMuseum(this.GameNameBox.Text, new Game()).desc + Environment.NewLine;
            }
            catch (System.Exception excpt)
            {
                ResultsBox.Text += excpt.Message + Environment.NewLine;
            }
        }

        private void SetStatus(string status)
        {
            ToolStripStatusLabel statusStrip = ((FormsContainer)(this.MdiParent)).toolStripStatusLabel;
            statusStrip.Text = status;
        }

        private void DoWork()
        {
            try
            {
                //foreach (string d in Directory.GetDirectories(@"C:\Users\Jaime\.emulationstation\roms\mame"))
                //{
                string d = Settings.Default.MAME_Roms_Directory; // @"C:\Users\Jaime\.emulationstation\roms\mame-libretro";
                //string d = @"C:\Users\Jaime\.emulationstation\roms\mame-mame4all";

                int fileCount = Directory.GetFiles(d).Count();
                int filesProcessed = 0;

                ProcessProgress.Invoke((MethodInvoker)delegate
                { ProcessProgress.Value = 0; ProcessProgress.Maximum = fileCount; ProcessProgress.CustomText = "Process START! - Ready Player 1"; });

                ResultsBox.Invoke((MethodInvoker)delegate
                     {ResultsBox.Text = ""; });
                
                foreach (string f in Directory.GetFiles(d))
                {
                    if (stopProcess) { //If process is ordered to Stop loop is finished
                        SetStatus("Process Stopped! - GAME OVER");

                        ProcessProgress.Invoke((MethodInvoker)delegate
                        { ProcessProgress.Value = 100; ProcessProgress.Maximum = 100; ProcessProgress.CustomText = "Process Stopped! - GAME OVER"; });

                        break; 
                    } 

                    Debug.WriteLine(f);
                    String ScrapeResponse = HTTPHandler.HttpGet("http://www.mamedb.com/game/" + Path.GetFileNameWithoutExtension(f));

                    if (ScrapeResponse != "")
                    {
                        Game game = ScrapeHandler.getDetails(ScrapeResponse);
                        game.path = "./" + Path.GetFileName(f);

                        String TitleMarkerStart = "<table border='0' cellspacing='25'><tr><td><h1>";
                        String TitleMarkerEnd = "</h1>";

                        game.name  = ScrapeHandler.ScrapeValue(TitleMarkerStart, TitleMarkerEnd, ScrapeResponse).Replace("(MAME version 0.147)", "").Trim();

                        if (game.name != "")
                        { 
                            int gameParenthesis = game.name.IndexOf("(");
                            string cleanName = "";
                            if (gameParenthesis > 0)
                            {
                                cleanName = game.name.Substring(0, game.name.IndexOf("(")).Trim().Replace(":", "");
                            }
                            else
                            {
                                cleanName = game.name.Replace(":", "");
                            }

                            string imagepath = ScrapeHandler.getimage("", game.path);
                            if (imagepath != "")
                            {
                                game.image = "~/.emulationstation/downloaded_images/" + imagepath;
                            }

                            game = ScrapeHandler.getDetailsArcadeMuseum(cleanName, game);
                        }

                        //Int32 respLength = ScrapeResponse.Length;
                        //Int32 titleStPos = ScrapeResponse.IndexOf(TitleMarkerStart) + TitleMarkerStart.Length;
                        //Int32 titleEndPos = ScrapeResponse.IndexOf(TitleMarkerEnd);
                        //if (titleStPos < titleEndPos)
                        //{
                        //    Int32 titleLength = titleEndPos - titleStPos;
                        //    game.name = ScrapeResponse.Substring(titleStPos, titleLength).Replace("(MAME version 0.147)", "").Trim();

                        //    int gameParenthesis = game.name.IndexOf("(");
                        //    string cleanName = "";
                        //    if (gameParenthesis > 0)
                        //    {
                        //        cleanName = game.name.Substring(0, game.name.IndexOf("(")).Trim().Replace(":", "");
                        //    }
                        //    else
                        //    {
                        //        cleanName = game.name.Replace(":", "");
                        //    }

                        //    string imagepath = ScrapeHandler.getimage("", game.path);
                        //    if (imagepath != "")
                        //    {
                        //        game.image = "~/.emulationstation/downloaded_images/" + imagepath;
                        //    }

                        //    game = ScrapeHandler.getDetailsArcadeMuseum(cleanName, game);
                        //}

                        XMLHandler.UpdateGameXML(game, Directory.GetCurrentDirectory() + "\\gamelist.xml", true);// writexml(game);

                        //Refresh the Results Box by invoking it on the main window thread
                        ResultsBox.Invoke((MethodInvoker)delegate
                        {  
                            ResultsBox.Text += game.name + " - " + game.desc + Environment.NewLine;
                            ResultsBox.SelectionStart = ResultsBox.Text.Length;
                            ResultsBox.ScrollToCaret();
                        });

                        filesProcessed++;
                        ProcessProgress.Invoke((MethodInvoker)delegate
                        {
                            //ProcessProgress.Text += "File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " in " + d + Environment.NewLine;
                            ProcessProgress.CustomText = "File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " - " +  game.name;
                            ProcessProgress.Value++;
                        });

                        SetStatus("File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " - " + game.name);
                    }
                }

                //DirSearch(d);

                ProcessProgress.Invoke((MethodInvoker)delegate
                { ProcessProgress.Value = 100; ProcessProgress.Maximum = 100; ProcessProgress.CustomText = "Process Complete! - YOU WIN"; });

                //Join the thread
                Thread.CurrentThread.Join(0);
            }
            catch (System.Exception excpt)
            {
                if (ResultsBox.IsHandleCreated == true)
                {
                    //Refresh the Results Box by invoking it on the main window thread
                    ResultsBox.Invoke(
                        (MethodInvoker)
                        delegate
                        {
                            ResultsBox.Text += excpt.Source + " - " + excpt.StackTrace + " - " + excpt.Message + Environment.NewLine;
                        }
                    );
                }
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

        private void btnXMLSearch_Click(object sender, EventArgs e)
        {
            Game game = new Game();
            game.path="./10yard85.zip";
            game.name = "Test";
            game.desc = "test";
            game.developer = "test";
            XMLHandler.UpdateGameXML(game,Directory.GetCurrentDirectory() + "\\gamelist.xml",true);

            //String filepath = Directory.GetCurrentDirectory() + "\\gamelist.xml";

            ////<gameList>
            ////  <game>
            ////    <path>./005.zip</path>

            //if (File.Exists(filepath) == true)
            //{
            //    XmlDocument doc = new XmlDocument();
            //    doc.Load(filepath);
            //    //doc.LoadXml(@"<ArrayOfRecentFiles> <RecentFile>C:\asd\1\Examples\8389.atc</RecentFile> <RecentFile>C:\asd\1\Examples\8385.atc</RecentFile>   </ArrayOfRecentFiles>");
            //    //string mFilePath = @"C:\asd\1\Examples\8385.atc";
            //    //var el = doc.SelectSingleNode("/ArrayOfRecentFiles/RecentFile[text()='" + mFilePath + "']");
            //    XmlNode node = doc.SelectSingleNode("/gameList/game/path[text()='" + this.GameNameBox.Text + "']").ParentNode;

            //        //xmlWriter.WriteElementString("path", game.path);
            //        //xmlWriter.WriteElementString("name", game.name);
            //        //xmlWriter.WriteElementString("desc", game.desc);
            //        //xmlWriter.WriteElementString("image", game.image);
            //        //xmlWriter.WriteElementString("releasedate", game.releasedate);
            //        //xmlWriter.WriteElementString("developer", game.developer);
            //        //xmlWriter.WriteElementString("publisher", game.publisher);
            //        //xmlWriter.WriteElementString("genre", game.genre);
            //        //xmlWriter.WriteElementString("players", game.players);
            //        //xmlWriter.WriteElementString("rating", game.rating);

            //    this.ResultsBox.Text += node.SelectSingleNode("desc").InnerText;
            //    node.SelectSingleNode("desc").InnerText = "TEST";
            //    doc.Save(filepath);
            //    //this.ResultsBox.Text = node.ParentNode.ToString();

            //    //node.ParentNode.InnerXml
            //}

            ////var hrefs = doc.Root.Descendants("a")
            ////    .Where(a => a.Attrib("href").Value.ToUpper().EndsWith(".PDF"))
            ////    .Select(a => a.Attrib("href"));
        }





    }

}
