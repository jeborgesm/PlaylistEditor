using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.Text;


namespace PlaylistEditor
{
    public partial class Main : Form
    {


        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        { }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //foreach (string d in Directory.GetDirectories(@"C:\Users\Jaime\.emulationstation\roms\mame"))
                //{
                //string d = @"C:\Users\Jaime\.emulationstation\roms\mame-libretro";
                string d = @"C:\Users\Jaime\.emulationstation\roms\mame-mame4all";
                foreach (string f in Directory.GetFiles(d))
                {
                    Debug.WriteLine(f);
                    String ScrapeResponse = HttpGet("http://www.mamedb.com/game/" + Path.GetFileNameWithoutExtension(f));
                    
                    String Title = "";
                    if (ScrapeResponse != "")
                    {
                        Game game = getDetails(ScrapeResponse);
                        String TitleMarkerStart = "<table border='0' cellspacing='25'><tr><td><h1>";
                        String TitleMarkerEnd = "</h1>";
                        Int32 respLength = ScrapeResponse.Length;
                        Int32 titleStPos = ScrapeResponse.IndexOf(TitleMarkerStart) + TitleMarkerStart.Length;
                        Int32 titleEndPos = ScrapeResponse.IndexOf(TitleMarkerEnd);
                        if (titleStPos < titleEndPos)
                        {
                            Int32 titleLength = titleEndPos - titleStPos;
                            game.name = ScrapeResponse.Substring(titleStPos, titleLength);
                        }
                        Debug.Write(Title);
                        ResultsBox.Text += Title + Environment.NewLine;
                        game.path = "./" + Path.GetFileName(f);
                        writexml(game);//"./" + Path.GetFileName(f), Title);
                    }
                }
                DirSearch(d);
                //}
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        private void btnGetGameDetails_Click(object sender, EventArgs e)
        {
            ResultsBox.Text += getDetailsArcadeMuseum(this.GameNameBox.Text).desc + Environment.NewLine; ;
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

        static void writexml(Game game)//String filename, String title)
        {
            //"C:\Users\Jaime\.emulationstation\gamelists\mame\gamelist.xml"
            //"gamelists\\mame\\gamelist.xml"
            String filepath = Directory.GetCurrentDirectory() + "\\gamelist.xml"; // @"C:\Users\Jaime\.emulationstation\gamelists\mame\gamelist.xml";

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

            string imagepath = getimage("", game.path);
            if (imagepath != "")
            {
                game.image  = "~/.emulationstation/downloaded_images/" + imagepath;
            }

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
                    xmlWriter.WriteElementString("name", game.name.Replace("(MAME version 0.147)",""));
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
                    new XElement("name", game.name.Replace("(MAME version 0.147)", "")),
                    new XElement("image", game.image),
                    new XElement("releasedate", game.releasedate),
                    new XElement("developer", game.developer),
                    new XElement("publisher", game.publisher),
                    new XElement("genre", game.genre),
                    new XElement("players", game.players),
                    new XElement("rating", game.rating)));
                //firstRow.AddBeforeSelf(
                //new XElement("Student",
                //new XElement("FirstName", firstName),
                //new XElement("LastName", lastName)));
                xDocument.Save(filepath);
            }
        }

        public static string HttpGet(string URI)
        {
            try
            {
                //String ProxyString = "";
                //System.Net.WebRequest req = System.Net.WebRequest.Create(URI);

                // Create a new 'HttpWebRequest' object to the mentioned URL.
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URI);
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

                //req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public static string HttpPost(string URI, string Parameters)
        {
            //String ProxyString = "";
            //System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URI);
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

            //req.Proxy = new System.Net.WebProxy(ProxyString, true);
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

                using (WebClient webClient = new WebClient()) 
                {
                    //http://www.mamedb.com/image/snap/64streetj
                    //http://www.mamedb.com/image/title/64streetj
                    //<tbody><tr><td><img src="/titles/64street.png" alt="Title Screen:  64th. Street - A Detective Story (Japan)"></td></tr><tr><td align="center">Title Screen</td></tr></tbody>

                    //byte [] data = webClient.DownloadData("https://fbcdn-sphotos-h-a.akamaihd.net/hphotos-ak-xpf1/v/t34.0-12/10555140_10201501435212873_1318258071_n.jpg?oh=97ebc03895b7acee9aebbde7d6b002bf&oe=53C9ABB0&__gda__=1405685729_110e04e71d9");
                    //byte[] data = webClient.DownloadData("http://www.mamedb.com/snap/" + fileName + ".png");
                    byte[] data = webClient.DownloadData("http://www.mamedb.com/titles/" + fileName + ".png");
                    //byte[] data = Convert.ToByte(HttpGet("http://www.mamedb.com/snap/" + Path.GetFileNameWithoutExtension(fileName) + ".png"));

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

        static Game getDetails(string inHtml)
        {
            Game game = new Game();
            String Detail = "";
            String DetailMarkerStart = "<h1>Game Details</h1><br/>";
            String DetailMarkerEnd = "</td>";
            Int32 respLength = inHtml.Length;
            Int32 detailStPos = inHtml.IndexOf(DetailMarkerStart) + DetailMarkerStart.Length;

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

            return game;
        }

        static Game getDetailsArcadeMuseum(string gameName)
        {
            String ScrapeResponse = HttpPost("http://www.arcade-museum.com/results.php","q="+ HttpUtility.UrlEncode(gameName)+"&boolean=AND&search_desc=%3D%220%22&type=");
            //"Search results for: "

            String Detail = "";
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

                    GameDetailResponse = HttpGet("http://www.arcade-museum.com/game_detail.php?game_id=" + GameID);
                }
            }

            //String DetailMarkerStart = "game_detail.php?game_id=";
            //String DetailMarkerEnd = ">";
            //Int32 respLength = ScrapeResponse.Length;
            //Int32 detailStPos = ScrapeResponse.IndexOf(DetailMarkerStart) + DetailMarkerStart.Length;

            //Int32 detailEndPos = detailStPos;
            //while (ScrapeResponse.Substring(detailEndPos, 1) != DetailMarkerEnd)
            //{
            //    detailEndPos++;
            //}

            //if (detailStPos < detailEndPos)
            //{
            //    Int32 titleLength = (detailEndPos -1)- detailStPos;
            //    GameID = ScrapeResponse.Substring(detailStPos, titleLength);

            //    GameDetailResponse = HttpGet("http://www.arcade-museum.com/game_detail.php?game_id="+GameID);
            //}

            Game game = new Game();
            game.desc = GameDetailResponse;

            //http://www.arcade-museum.com/results.php
            //q=Alex+Kidd+the+lost+stars&boolean=AND&search_desc=%3D%220%22&type=
            //<a href="game_detail.php?game_id=6845">Alex Kidd: The Lost Stars</a>
            //http://www.arcade-museum.com/game_detail.php?game_id=6845

            return game;

            //NameValueCollection parameters = new NameValueCollection();
            //parameters.Add("q", HttpUtility.UrlEncode(gameName));
            //parameters.Add("boolean", "AND");
            //parameters.Add("search_desc", "%3D%220%22");
            //parameters.Add("type", null);
            //String ScrapeResponse = HttpPostSimple("http://www.arcade-museum.com/results.php", parameters);

            //string ScrapeResponse = Crawl("http://www.arcade-museum.com", "http://www.arcade-museum.com/results.php", "q=" + HttpUtility.UrlEncode(gameName) + "&boolean=AND&search_desc=%3D%220%22&type=");
        }

        public static string HttpPostSimple(string URI, NameValueCollection parameters)
        {
            string result = "";
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(URI, parameters);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            return result;
        }

        public static string Crawl(string rootUrl, string Url, string postParameters)
        {
            var cookieContainer = new CookieContainer();
            //string test =  HttpGet(rootUrl);
            /* initial GET Request */
            //var getRequest = (HttpWebRequest)WebRequest.Create(rootUrl);

            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(Url);
            getRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

            getRequest.CookieContainer = cookieContainer;
            ReadResponse(getRequest); // nothing to do with this, because captcha is f#@%ing dumb :)

            /* POST Request */
            //var postRequest = (HttpWebRequest)WebRequest.Create(Url);
            HttpWebRequest postRequest = (HttpWebRequest)WebRequest.Create(Url);
            postRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

            postRequest.AllowAutoRedirect = false; // we'll do the redirect manually; .NET does it badly
            postRequest.CookieContainer = cookieContainer;
            postRequest.Method = "POST";
            postRequest.ContentType = "application/x-www-form-urlencoded";

            //var postParameters =
            //    "_EventName=E%27CONFIRMAR%27.&_EventGridId=&_EventRowId=&_MSG=&_CONINSEST=&" +
            //    "_CONINSESTG=08775724000119&cfield=much&_VALIDATIONRESULT=1&BUTTON1=Confirmar&" +
            //    "sCallerURL=";

            var bytes = Encoding.UTF8.GetBytes(postParameters);

            postRequest.ContentLength = bytes.Length;

            using (var requestStream = postRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);

            var webResponse = postRequest.GetResponse();

            ReadResponse(postRequest); // not interested in this either

            var redirectLocation = webResponse.Headers[HttpResponseHeader.Location];

            var finalGetRequest = (HttpWebRequest)WebRequest.Create(redirectLocation);


            /* Apply fix for the cookie */
            FixMisplacedCookie(cookieContainer,Url,rootUrl);

            /* do the final request using the correct cookies. */
            finalGetRequest.CookieContainer = cookieContainer;

            var responseText = ReadResponse(finalGetRequest);

            return responseText; // Hooray!
        }

        private static string ReadResponse(HttpWebRequest getRequest)
        {
            using (var responseStream = getRequest.GetResponse().GetResponseStream())
            using (var sr = new StreamReader(responseStream, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        private static void FixMisplacedCookie(CookieContainer cookieContainer, string Url, string goodUrl)
        {
            var misplacedCookie = cookieContainer.GetCookies(new Uri(Url))[0];

            misplacedCookie.Path = "/"; // instead of "/sintegra/servlet/hwsintco"

            //place the cookie in thee right place...
            cookieContainer.SetCookies(
                new Uri(goodUrl),//"https://www.sefaz.rr.gov.br/"),
                misplacedCookie.ToString());
        }
    }

}
