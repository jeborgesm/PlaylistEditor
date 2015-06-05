using PlaylistEditor.Properties;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace PlaylistEditor
{
    class HTTPHandler
    {
        public static string HttpGet(string URI)
        {
            try
            {
                //String ProxyString = "";
                //System.Net.WebRequest req = System.Net.WebRequest.Create(URI);

                // Create a new 'HttpWebRequest' object to the mentioned URL.
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URI);
                req.UserAgent = Settings.Default.UserAgent;//"Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

                //req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string HttpPost(string URI, string Parameters)
        {
            //String ProxyString = "";
            //System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URI);
            req.UserAgent = Settings.Default.UserAgent;// "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

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
            /* initial GET Request */
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(Url);
            getRequest.UserAgent = Settings.Default.UserAgent;//"Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

            getRequest.CookieContainer = cookieContainer;
            ReadResponse(getRequest); // nothing to do with this, because captcha is f#@%ing dumb :)

            /* POST Request */
            HttpWebRequest postRequest = (HttpWebRequest)WebRequest.Create(Url);
            postRequest.UserAgent = Settings.Default.UserAgent;//"Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

            postRequest.AllowAutoRedirect = false; // we'll do the redirect manually; .NET does it badly
            postRequest.CookieContainer = cookieContainer;
            postRequest.Method = "POST";
            postRequest.ContentType = "application/x-www-form-urlencoded";

            var bytes = Encoding.UTF8.GetBytes(postParameters);

            postRequest.ContentLength = bytes.Length;

            using (var requestStream = postRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);

            var webResponse = postRequest.GetResponse();

            ReadResponse(postRequest); // not interested in this either

            var redirectLocation = webResponse.Headers[HttpResponseHeader.Location];

            var finalGetRequest = (HttpWebRequest)WebRequest.Create(redirectLocation);


            /* Apply fix for the cookie */
            FixMisplacedCookie(cookieContainer, Url, rootUrl);

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

            misplacedCookie.Path = "/"; 

            //place the cookie in thee right place...
            cookieContainer.SetCookies(
                new Uri(goodUrl),
                misplacedCookie.ToString());
        }

    }
}
