using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace StockBuddy.Common.Utilities
{
    public static class HttpUtils
    {
        private static StreamReader _strm = null;
        private static HttpWebResponse _webresp = null;
        private static XmlReader _reader = null;
        private static Stream _stream = null;

        public static ManualResetEvent allDone = null;
        const int BUFFER_SIZE = 1024;

        public static XmlReader ExecuteReader(string connectionString, params object[] args)
        {
            string query = String.Format(connectionString, args);
            HttpWebRequest webreq = null;

            try
            {
                // Initialize a new WebRequest.
                webreq = (HttpWebRequest)WebRequest.Create(query);
                webreq.KeepAlive = false;
                // Get the response from the Internet resource.
                _webresp = (HttpWebResponse)webreq.GetResponse();
                // Read the body of the response from the server.

                _strm =
                    new StreamReader(_webresp.GetResponseStream(), Encoding.ASCII);

                //string temp = _strm.ReadToEnd();

                XmlReaderSettings settings;
                settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Document;

                _reader = XmlReader.Create(_strm, settings);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return _reader;
        }

        public static void CloseConnections(XmlReader xr, bool close)
        {
            if (close)
            {
                if (_reader != null) _reader.Close();
                if (_stream != null) _stream.Close();
                if (_webresp != null) _webresp.Close();
                if (_strm != null) _strm.Close();
            }
        }
    }
}
