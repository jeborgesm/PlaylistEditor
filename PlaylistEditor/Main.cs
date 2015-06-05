using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace PlaylistEditor
{
    public partial class Main : Form
    {
        static bool stopProcess = false;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        { }

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
                string d = @"C:\Users\Jaime\.emulationstation\roms\mame-libretro";
                //string d = @"C:\Users\Jaime\.emulationstation\roms\mame-mame4all";

                int fileCount = Directory.GetFiles(d).Count();
                int filesProcessed = 0;

                ProcessProgress.Invoke((MethodInvoker)
                    delegate {ProcessProgress.Value = 0; ProcessProgress.Maximum = fileCount;});

                ResultsBox.Invoke((MethodInvoker)
                    delegate {ResultsBox.Text = ""; });
                
                foreach (string f in Directory.GetFiles(d))
                {
                    if (stopProcess) { //If process is ordered to Stop loop is finished
                        SetStatus("Process END! - GAME OVER");
                        ////Refresh the Results Box by invoking it on the main window thread
                        //ResultsBox.Invoke(
                        //    (MethodInvoker)
                        //    delegate
                        //    {
                                
                        //        ResultsBox.SelectionStart = ResultsBox.Text.Length;
                        //        ResultsBox.ScrollToCaret();
                        //    }
                        //);
                        break; 
                    } 

                    Debug.WriteLine(f);
                    String ScrapeResponse = HTTPHandler.HttpGet("http://www.mamedb.com/game/" + Path.GetFileNameWithoutExtension(f));

                    String Title = "";
                    if (ScrapeResponse != "")
                    {
                        Game game = ScrapeHandler.getDetails(ScrapeResponse);
                        game.path = "./" + Path.GetFileName(f);

                        String TitleMarkerStart = "<table border='0' cellspacing='25'><tr><td><h1>";
                        String TitleMarkerEnd = "</h1>";
                        Int32 respLength = ScrapeResponse.Length;
                        Int32 titleStPos = ScrapeResponse.IndexOf(TitleMarkerStart) + TitleMarkerStart.Length;
                        Int32 titleEndPos = ScrapeResponse.IndexOf(TitleMarkerEnd);
                        if (titleStPos < titleEndPos)
                        {
                            Int32 titleLength = titleEndPos - titleStPos;
                            game.name = ScrapeResponse.Substring(titleStPos, titleLength).Replace("(MAME version 0.147)", "").Trim();

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
                       
                        XMLHandler.writexml(game);

                        //Refresh the Results Box by invoking it on the main window thread
                        ResultsBox.Invoke(
                            (MethodInvoker)
                            delegate
                            {  
                                ResultsBox.Text += game.name + " - " + game.desc + Environment.NewLine;
                                ResultsBox.SelectionStart = ResultsBox.Text.Length;
                                ResultsBox.ScrollToCaret();
                            }
                        );

                        filesProcessed++;
                        ProcessProgress.Invoke((MethodInvoker)
                        delegate
                        {
                            //ProcessProgress.Text += "File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " in " + d + Environment.NewLine;
                            ProcessProgress.CustomText = "File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " - " +  game.name;
                            ProcessProgress.Value++;
                        });

                        SetStatus("File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " - " + game.name);
                    }
                }

                DirSearch(d);

                //Join the thread
                Thread.CurrentThread.Join(0);
            }
            catch (System.Exception excpt)
            {
                //Refresh the Results Box by invoking it on the main window thread
                ResultsBox.Invoke(
                    (MethodInvoker)
                    delegate
                    {
                        ResultsBox.Text += excpt.Source + " - " + excpt.StackTrace + " - " +  excpt.Message + Environment.NewLine;
                    }
                );
            }
        }

        private void btnGetGameDetails_Click(object sender, EventArgs e)
        {
            try {
                ResultsBox.Text += ScrapeHandler.getDetailsArcadeMuseum(this.GameNameBox.Text, new Game()).desc + Environment.NewLine;
            }
            catch (System.Exception excpt)
            {
                ResultsBox.Text += excpt.Message + Environment.NewLine;
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

        //private void ThreadDelegate(Control obj)
        //{
        //    obj.Invoke((MethodInvoker)
        //        delegate
        //        {
        //            //ProcessProgress.Text += "File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " in " + d + Environment.NewLine;
        //            ProcessProgress.CustomText = "File " + filesProcessed.ToString() + " of " + fileCount.ToString() + " - " + game.name;
        //            ProcessProgress.Value++;
        //        });
        //}




    }

}
