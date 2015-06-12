using PlaylistEditor.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Threading.Tasks;

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
            try
            {
                ToolStripStatusLabel statusStrip = ((FormsContainer)(this.MdiParent)).toolStripStatusLabel;
                statusStrip.Text = status;
            }
            catch (System.Exception excpt)
            {
            }
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
                    {ProcessProgress.Value = 0; ProcessProgress.Maximum = fileCount; ProcessProgress.CustomText = "Process START! - Ready Player 1"; });

                ResultsBox.Invoke((MethodInvoker)delegate
                     {ResultsBox.Text = ""; });

                CancellationTokenSource cts = new CancellationTokenSource();

                // Use ParallelOptions instance to store the CancellationToken
                ParallelOptions po = new ParallelOptions();
                po.CancellationToken = cts.Token;
                po.MaxDegreeOfParallelism = 15;// System.Environment.ProcessorCount; //4;

                try 
                { 
                    //foreach (string f in Directory.GetFiles(d))
                    Parallel.ForEach(Directory.GetFiles(d), po, f =>
                    {
                        if (stopProcess)
                        { //If process is ordered to Stop loop is finished
                            SetStatus("Process Stopped! - GAME OVER");

                            ProcessProgress.Invoke((MethodInvoker)delegate
                            { ProcessProgress.Value = 100; ProcessProgress.Maximum = 100; ProcessProgress.CustomText = "Process Stopped! - GAME OVER"; });

                            cts.Cancel();
                            //break;
                        }

                        Debug.WriteLine(f);
                        String ScrapeResponse = HTTPHandler.HttpGetwithThreads("http://www.mamedb.com/game/" + Path.GetFileNameWithoutExtension(f));

                        if (ScrapeResponse != "")
                        {
                            Game game = ScrapeHandler.getDetails(ScrapeResponse);
                            game.path = "./" + Path.GetFileName(f);

                            String TitleMarkerStart = "<table border='0' cellspacing='25'><tr><td><h1>";
                            String TitleMarkerEnd = "</h1>";

                            game.name = ScrapeHandler.ScrapeValue(TitleMarkerStart, TitleMarkerEnd, ScrapeResponse).Replace("(MAME version 0.147)", "").Trim();

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
                                ProcessProgress.CustomText = "File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " - " + game.name;
                                if (ProcessProgress.Value < ProcessProgress.Maximum)
                                    ProcessProgress.Value++;
                            });

                            SetStatus("File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " - " + game.name);
                        }
                    });

                    //DirSearch(d);
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    cts.Dispose();
                }

                ProcessProgress.Invoke((MethodInvoker)delegate
                { ProcessProgress.Value = 100; ProcessProgress.Maximum = 100; ProcessProgress.CustomText = "Process Complete! - YOU WIN"; });
                SetStatus("Process Complete! - YOU WIN");

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
            game.path = "./10yard85.zip";
            game.name = "Test";
            game.desc = "test";
            game.developer = "test";
            XMLHandler.UpdateGameXML(game, Directory.GetCurrentDirectory() + "\\gamelist.xml", true);


        }





    }

}
