using PlaylistEditor.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;

namespace PlaylistEditor
{
    class ScrapeHandler
    {
        public static string getimage(string inHtml, string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            try
            {
                string foundpath = "";

                if (Directory.Exists("mame") == false)
                {
                    Directory.CreateDirectory("mame");
                }

                ServicePointManager.MaxServicePoints = 5;
                ServicePointManager.MaxServicePointIdleTime = 5000;
                ServicePointManager.UseNagleAlgorithm = true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;
                ServicePoint servicePoint = ServicePointManager.FindServicePoint(new Uri("http://www.mamedb.com"));

                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData("http://www.mamedb.com/titles/" + fileName + ".png");

                    if (data.Length > 0)
                    {
                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            var yourImage = Image.FromStream(mem);

                            ///mame/sf2049-image.jpg/
                            //if (yourImage.RawFormat == ImageFormat.Jpeg)
                            //{
                            // If you want it as Png
                            foundpath = "mame/" + fileName + "-image.png";
                            yourImage.Save(foundpath, ImageFormat.Png);
                            //}
                            //else if (yourImage.RawFormat == ImageFormat.Png)
                            //{
                            //    // If you want it as Jpeg
                            //    foundpath = "mame/" + fileName + "-image.jpg";
                            //    yourImage.Save(foundpath, ImageFormat.Jpeg);
                            //}
                        }
                    }
                }
                return foundpath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public void getRating(string inHtml)
        {
            //http://www.mamedb.com/game/64streetj
            //        <td>	
            //<h1>Rate This Game</h1>
            //<b>Score:&nbsp;</b>7.04347826085 (115 votes)<br>	

        }

        public static Game getDetails(string inHtml)
        {
            Game game = new Game();
            try
            {

                String Detail = "";
                String DetailMarkerStart = "<h1>Game Details</h1><br/>";
                String DetailMarkerEnd = "</td>";
                Int32 respLength = inHtml.Length;
                Int32 detailStPos = inHtml.IndexOf(DetailMarkerStart) + DetailMarkerStart.Length;
                if (inHtml.IndexOf(DetailMarkerStart) > 0)
                {
                    Int32 detailEndPos = detailStPos;
                    while (inHtml.Substring(detailEndPos, 5) != DetailMarkerEnd)
                    {
                        detailEndPos++;
                    }

                    //Int32 detailEndPos = inHtml.IndexOf(DetailMarkerEnd);
                    if (detailStPos < detailEndPos)
                    {
                        Int32 titleLength = detailEndPos - detailStPos;
                        Detail = inHtml.Substring(detailStPos, titleLength);

                        //Int32 detailEndPos = detailStPos;
                        //while (inHtml.Substring(detailEndPos, 5) != DetailMarkerEnd)
                        //{
                        //    detailEndPos++;
                        //}
                        string[] Detaildata = Detail.Replace("<br/>", "|").Replace("<b>", "").Replace("&nbsp;</b>", "").Replace("&nbsp</b>", "").Split('|');
                        string ManufMarkerStart = "<b>Manufacturer:&nbsp</b> <a href='/manufacturer/";
                        string ManufMarkerEnd = "</td>";
                        string manufacturer = Detail.Substring(Detail.IndexOf(ManufMarkerStart) + ManufMarkerStart.Length, 5);
                        //game.developer = manufacturer;
                    }

                    //http://www.mamedb.com/game/64streetj
                    //<h1>Game Details</h1><br>
                    //<b>Name:&nbsp;</b>64th. Street - A Detective Story (Japan) &nbsp;(clone of: <a href="/game/64street">64street</a>)&nbsp;<br>
                    //<b>Year:&nbsp;</b> <a href="/year/1991">1991</a><br>
                    //<b>Manufacturer:&nbsp;</b> <a href="/manufacturer/Jaleco">Jaleco</a><br>
                    //<b>Filename:&nbsp;</b>64streetj<br>
                    //<b>Status:&nbsp;</b>good<br>
                    //<b>Emulation:&nbsp;</b>good<br>
                    //<b>Color:&nbsp;</b>good<br>
                    //<b>Sound:&nbsp;</b>good<br>
                    //<b>Graphic:&nbsp;</b>good<br>
                    //<b>Palette Size:&nbsp;</b>1024</td>

                    //http://www.arcade-museum.com/results.php
                    //q=Alex+Kidd+the+lost+stars&boolean=AND&search_desc=%3D%220%22&type=
                    //<a href="game_detail.php?game_id=6845">Alex Kidd: The Lost Stars</a>
                    //http://www.arcade-museum.com/game_detail.php?game_id=6845

                    //http://thegamesdb.net/api/GetGame.php?name=alex%20kidd%20the%20lost%20stars&platform=arcade
                    //<Game>
                    //<id>16790</id>
                    //<GameTitle>Alex Kidd: The Lost Stars</GameTitle>
                    //<PlatformId>23</PlatformId>
                    //<Platform>Arcade</Platform>
                    //<Overview>
                    //Alex Kidd: The Lost Stars features Alex Kidd and Stella searching for the twelve Zodiac signs.
                    //</Overview>
                    //<Genres>
                    //<genre>Platform</genre>
                    //</Genres>
                    //<Players>1</Players>
                    //<Co-op>No</Co-op>
                    //<Publisher>Sega</Publisher>
                    //<Developer>Sega</Developer>
                    //<Similar>
                    //<SimilarCount>1</SimilarCount>
                    //<Game>
                    //<id>5498</id>
                    //<PlatformId>35</PlatformId>
                    //</Game>
                    //</Similar>
                    //<Images>
                    //<boxart side="front" width="501" height="700" thumb="boxart/thumb/original/front/16790-1.jpg">boxart/original/front/16790-1.jpg</boxart>
                    //<screenshot>
                    //<original width="320" height="224">screenshots/16790-1.jpg</original>
                    //<thumb>screenshots/thumb/16790-1.jpg</thumb>
                    //</screenshot>
                    //</Images>
                    //</Game>
                }

            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return game;
        }

        public static Game getDetailsArcadeMuseum(string gameName, Game game)
        {
            try
            {
                //Start: Description</H2>
                //End:<p>

                //Start: 
                //<H2>Game Play</H2>
                //End:<H2>Miscellaneous</H2>

                //title image
                //<img alt="Makyou Senshi - Title screen image" src="/images/118/11812421378.png" width="240" height="256">

                //Start: <h2>Cheats, Tricks, Bugs, and Easter Eggs</h2>
                //End: <h2>


                String ScrapeResponse = HTTPHandler.HttpPostwithThreads("http://www.arcade-museum.com/results.php", "q=" + HttpUtility.UrlEncode(gameName) + "&boolean=AND&search_desc=%3D%220%22&type=");

                if (ScrapeResponse.Length > 0)
                {
                    string Description = "";
                    String GameID = "";
                    String GameDetailResponse = "";
                    string gameidMarkerEnd = "\">" + gameName.ToUpper() + "</A>";
                    Int32 gameidEndPos = ScrapeResponse.ToUpper().IndexOf(gameidMarkerEnd) + gameidMarkerEnd.Length;
                    Int32 gameidStPos = gameidEndPos;
                    if (gameidEndPos > gameidMarkerEnd.Length)
                    {
                        while (ScrapeResponse.Substring(gameidStPos, 1) != "=")
                        {
                            gameidStPos--;
                        }

                        if (gameidStPos < gameidEndPos)
                        {
                            Int32 gameidLength = (gameidEndPos - 1) - gameidStPos;
                            GameID = ScrapeResponse.Substring(gameidStPos + 1, gameidLength - gameidMarkerEnd.Length);

                            GameDetailResponse = HTTPHandler.HttpGetwithThreads("http://www.arcade-museum.com/game_detail.php?game_id=" + GameID);

                            Description = ScrapeValue("Description</H2>", "<p>", GameDetailResponse);

                            if (game.image == null)
                            {
                                string img = ScrapeValue("Title screen image", ".png", GameDetailResponse);
                                img = img.Substring(img.IndexOf("images/"));
                                string imagepath = SaveImage("http://www.arcade-museum.com/" + img + ".png", Path.GetFileNameWithoutExtension(game.path));
                                if (imagepath != "")
                                {
                                    game.image = "~/.emulationstation/downloaded_images/" + imagepath;
                                }
                                //title image
                                //<img alt="Makyou Senshi - Title screen image" src="/images/118/11812421378.png" width="240" height="256">
                            }

                            //String DescMarkerStart = "Description</H2>";
                            //String DescMarkerEnd = "<p>";
                            //Int32 respLength = GameDetailResponse.Length;
                            //Int32 descStPos = GameDetailResponse.IndexOf(DescMarkerStart) + DescMarkerStart.Length;

                            //Int32 descEndPos = descStPos;
                            //while (GameDetailResponse.Substring(descEndPos, 3) != DescMarkerEnd)
                            //{
                            //    descEndPos++;
                            //}

                            //if (descStPos < descEndPos)
                            //{
                            //    Int32 DescLength = descEndPos - descStPos;
                            //    Description = GameDetailResponse.Substring(descStPos, DescLength);
                            //}
                        }
                    }

                    game.desc = Description;
                }

            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return game;
        }

        public static string ScrapeValue(string MarkerStart, string MarkerEnd, string inHtml)
        {
            Int32 respLength = inHtml.Length;
            Int32 descStPos = inHtml.IndexOf(MarkerStart) + MarkerStart.Length;

            Int32 descEndPos = descStPos;

            if (descEndPos < respLength)
            {
                while (inHtml.Substring(descEndPos, MarkerEnd.Length) != MarkerEnd)
                {
                    descEndPos++;
                }

                if (descStPos < descEndPos)
                {
                    Int32 DescLength = descEndPos - descStPos;
                    return inHtml.Substring(descStPos, DescLength);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public static string SaveImage(string sourceFilePath, string savefileName)
        {
            try
            {
                string foundpath = "";

                if (Directory.Exists("mame") == false)
                {
                    Directory.CreateDirectory("mame");
                }

                ServicePointManager.MaxServicePoints = 5;
                ServicePointManager.MaxServicePointIdleTime = 5000;
                ServicePointManager.UseNagleAlgorithm = true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;
                ServicePoint servicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + (new Uri(sourceFilePath).Host)));

                HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(sourceFilePath);
                lxRequest.UserAgent = Settings.Default.UserAgent;

                using (HttpWebResponse lxResponse = (HttpWebResponse)lxRequest.GetResponse())
                {
                    using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                    {
                        Byte[] data = reader.ReadBytes(1 * 1024 * 1024 * 10);
                        if (data.Length > 0)
                        {
                            using (MemoryStream mem = new MemoryStream(data))
                            {
                                var yourImage = Image.FromStream(mem);

                                foundpath = "mame/" + savefileName + "-image.png";
                                yourImage.Save(foundpath, ImageFormat.Png);
                            }
                        }
                        //using (FileStream lxFS = new FileStream("34891.jpg", FileMode.Create)) {
                        //    lxFS.Write(lnByte, 0, lnByte.Length);
                        //}
                    }
                }

                //using (WebClient webClient = new WebClient())
                //{

                //    byte[] data = webClient.DownloadData(sourceFilePath);

                //    if (data.Length > 0)
                //    {
                //        using (MemoryStream mem = new MemoryStream(data))
                //        {
                //            var yourImage = Image.FromStream(mem);

                //            foundpath = "mame/" + savefileName + "-image.png";
                //            yourImage.Save(foundpath, ImageFormat.Png);
                //        }
                //    }
                //}
                return foundpath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
