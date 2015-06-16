using PlaylistEditor.Properties;
using System;
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
                req.Proxy = null;
                req.UserAgent = Settings.Default.UserAgent;//"Mozilla/5.0 (Windows NT 6.3; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";

                //req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

                    RequestThreadsStatus(req);

                    return sr.ReadToEnd().Trim();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorRoutine(false, ex); 
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public static string HttpPost(string URI, string Parameters)
        {
            try
            { 
                //String ProxyString = "";
                //System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URI);
                req.Proxy = null;
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
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    if (resp == null) return null;
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

                    RequestThreadsStatus(req);

                    return sr.ReadToEnd().Trim();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorRoutine(false, ex); 
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public static string HttpGetwithThreads(string URI, out string ThreadStatus)
        {
            try
            {
                ServicePoint servicePoint = RequestThreads(URI);
                ThreadStatus = ShowProperties(servicePoint);
                int hashCode = servicePoint.GetHashCode();

                return HttpGet(URI);
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorRoutine(false, ex); 
                Console.WriteLine(ex.Message);
                ThreadStatus = "";
                return "";
            }
        }

        public static string HttpPostwithThreads(string URI, string Parameters)
        {
            try
            {
                ServicePoint servicePoint = RequestThreads(URI);
                int hashCode = servicePoint.GetHashCode();
                ShowProperties(servicePoint);

                return HttpPost(URI, Parameters);
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorRoutine(false, ex); 
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public static ServicePoint RequestThreads(string URL) 
        {
            ServicePointManager.MaxServicePoints = Settings.Default.SPM_MaxServicePoints;
            ServicePointManager.MaxServicePointIdleTime = Settings.Default.SPM_MaxServicePointIdleTime;
            ServicePointManager.UseNagleAlgorithm = Settings.Default.SPM_UseNagleAlgorithm;
            ServicePointManager.Expect100Continue = Settings.Default.SPM_Expect100Continue;
            ServicePointManager.CheckCertificateRevocationList = Settings.Default.SPM_CheckCertificateRevocationList;
            ServicePointManager.DefaultConnectionLimit = Settings.Default.SPM_DefaultConnectionLimit;//ServicePointManager.DefaultPersistentConnectionLimit;
            ServicePoint servicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + (new Uri(URL).Host)));
            return servicePoint;
        }

        public static void RequestThreadsStatus(HttpWebRequest req)//, int hashCode)
        {
            #if DEBUG
            ServicePoint currentServicePoint = req.ServicePoint;

            // Display new service point properties. 
            int currentHashCode = currentServicePoint.GetHashCode();

            Console.WriteLine("New service point hashcode: " + currentHashCode);
            Console.WriteLine("New service point max idle time: " + currentServicePoint.MaxIdleTime);
            Console.WriteLine("New service point is idle since " + currentServicePoint.IdleSince);

            // Check that a new ServicePoint instance has been created. 
            //if (hashCode == currentHashCode)
            //    Console.WriteLine("Service point reused.");
            //else
            //    Console.WriteLine("A new service point created.");
            #endif
        }

        private static string ShowProperties(ServicePoint sp)
        {
            #if DEBUG
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
            #endif

            StringBuilder SB = new StringBuilder();
            SB.AppendLine("Address = " + sp.Address.ToString());
            SB.AppendLine("ConnectionLimit = " + sp.ConnectionLimit);
            SB.AppendLine("CurrentConnections = " + sp.CurrentConnections);
            SB.AppendLine("Service point hashcode: " + sp.GetHashCode());
            SB.AppendLine("Service point max idle time: " + sp.MaxIdleTime);
            SB.AppendLine("Service point is idle since " + sp.IdleSince);

            return SB.ToString();
        }

        /// <summary>
        /// Blocks until the file is not locked any more.
        /// </summary>
        /// <param name="fullPath"></param>
        public static bool WaitForFile(string fullPath)
        {
            int numTries = 0;
            while (true)
            {
                ++numTries;
                try
                {
                    // Attempt to open the file exclusively.
                    using (FileStream fs = new FileStream(fullPath,
                        FileMode.Open, FileAccess.ReadWrite,
                        FileShare.None, 100))
                    {
                        fs.ReadByte();

                        // If we got this far the file is ready
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //Log.LogWarning(
                    //   "WaitForFile {0} failed to get an exclusive lock: {1}",
                    //    fullPath, ex.ToString());

                    if (numTries > 100)
                    {
                        //Log.LogWarning(
                        //    "WaitForFile {0} giving up after 10 tries",
                        //    fullPath);
                        return false;
                    }

                    // Wait for the lock to be released
                    System.Threading.Thread.Sleep(500);
                }
            }

            //Log.LogTrace("WaitForFile {0} returning true after {1} tries",
            //    fullPath, numTries);
            return true;
        }

    }
}
