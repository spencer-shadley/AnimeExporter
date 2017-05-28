using System.Diagnostics;
using System.IO;
using System.Net;

namespace AnimeExporter {
    public class NetworkHandler {

        // PROPERTIES

        private string Url { get; set; }

        // CONSTRUCTORS

        public NetworkHandler(string url) {
            Url = url;
        }

        public static string GetHtml(string url) {
            var webRequest = WebRequest.Create(url);
            var webResponse = webRequest.GetResponse();
            var stream = webResponse.GetResponseStream();
            Debug.Assert(stream != null);
            var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();

            // clean up
            reader.Close();
            stream.Close();
            webResponse.Close();

            return content;
        }
    }
}