using PlaylistEditor.Properties;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

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

        public static string HttpGetwithThreads(string URI)
        {
            try
            {
                ServicePointManager.MaxServicePoints = 5;

                ServicePointManager.MaxServicePointIdleTime = 5000;

                ServicePointManager.UseNagleAlgorithm = true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;

                // Use the FindServicePoint method to find an existing  
                // ServicePoint object or to create a new one.  
                ServicePoint servicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + (new Uri(URI).Host)));

                ShowProperties(servicePoint);

                int hashCode = servicePoint.GetHashCode();

                // Create a new 'HttpWebRequest' object to the mentioned URL.
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URI);
                req.UserAgent = Settings.Default.UserAgent;

                System.Net.WebResponse resp = req.GetResponse();

                ServicePoint currentServicePoint = req.ServicePoint;

                // Display new service point properties. 
                int currentHashCode = currentServicePoint.GetHashCode();

                Console.WriteLine("New service point hashcode: " + currentHashCode);
                Console.WriteLine("New service point max idle time: " + currentServicePoint.MaxIdleTime);
                Console.WriteLine("New service point is idle since " + currentServicePoint.IdleSince);

                // Check that a new ServicePoint instance has been created. 
                if (hashCode == currentHashCode)
                    Console.WriteLine("Service point reused.");
                else
                    Console.WriteLine("A new service point created.");

                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string HttpPostwithThreads(string URI, string Parameters)
        {
            ServicePointManager.MaxServicePoints = 5;

            ServicePointManager.MaxServicePointIdleTime = 5000;

            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;

            // Use the FindServicePoint method to find an existing  
            // ServicePoint object or to create a new one.  
            ServicePoint servicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + (new Uri(URI).Host)));

            ShowProperties(servicePoint);

            int hashCode = servicePoint.GetHashCode();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URI);
            req.UserAgent = Settings.Default.UserAgent;

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();

            ServicePoint currentServicePoint = req.ServicePoint;

            // Display new service point properties. 
            int currentHashCode = currentServicePoint.GetHashCode();

            Console.WriteLine("New service point hashcode: " + currentHashCode);
            Console.WriteLine("New service point max idle time: " + currentServicePoint.MaxIdleTime);
            Console.WriteLine("New service point is idle since " + currentServicePoint.IdleSince);

            // Check that a new ServicePoint instance has been created. 
            if (hashCode == currentHashCode)
                Console.WriteLine("Service point reused.");
            else
                Console.WriteLine("A new service point created.");

            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        private static void ShowProperties(ServicePoint sp)
        {
            Console.WriteLine("Done calling FindServicePoint()...");

            // Display the ServicePoint Internet resource address.
            Console.WriteLine("Address = {0} ", sp.Address.ToString());

            // Display the date and time that the ServicePoint was last  
            // connected to a host.
            Console.WriteLine("IdleSince = " + sp.IdleSince.ToString());

            // Display the maximum length of time that the ServicePoint instance   
            // is allowed to maintain an idle connection to an Internet   
            // resource before it is recycled for use in another connection.
            Console.WriteLine("MaxIdleTime = " + sp.MaxIdleTime);

            Console.WriteLine("ConnectionName = " + sp.ConnectionName);

            // Display the maximum number of connections allowed on this  
            // ServicePoint instance.
            Console.WriteLine("ConnectionLimit = " + sp.ConnectionLimit);

            // Display the number of connections associated with this  
            // ServicePoint instance.
            Console.WriteLine("CurrentConnections = " + sp.CurrentConnections);

            if (sp.Certificate == null)
                Console.WriteLine("Certificate = (null)");
            else
                Console.WriteLine("Certificate = " + sp.Certificate.ToString());

            if (sp.ClientCertificate == null)
                Console.WriteLine("ClientCertificate = (null)");
            else
                Console.WriteLine("ClientCertificate = " + sp.ClientCertificate.ToString());

            Console.WriteLine("ProtocolVersion = " + sp.ProtocolVersion.ToString());
            Console.WriteLine("SupportsPipelining = " + sp.SupportsPipelining);

            Console.WriteLine("UseNagleAlgorithm = " + sp.UseNagleAlgorithm.ToString());
            Console.WriteLine("Expect 100-continue = " + sp.Expect100Continue.ToString());
        }

    }
}
