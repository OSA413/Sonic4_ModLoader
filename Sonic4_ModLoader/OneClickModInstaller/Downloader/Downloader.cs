using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Common.URL;
using System.ComponentModel;
using System.IO;

namespace OneClickModInstaller
{
    public class Downloader
    {
        public ServerHost Host;
        //Sometimes server may break connection when file is not fully downloaded
        //User will be offered to redownload it
        public long Recieved;
        public long Total;

        public enum ServerHost
        {
            Unknown,
            GameBanana,
            GitHub,
        }

        public string GetFileNameFromServer(string url, ServerHost? host=null)
        {
            if (host == null) host = GetServerHost(url);
            if (host == ServerHost.GitHub)
                return url.Split('/')[^1];
            return url.Split('/')[^1];
        }

        public static ServerHost GetServerHost(string url)
        {
            if (url.StartsWith("https://gamebanana.com"))
                return ServerHost.GameBanana;
            else if (url.StartsWith("https://github.com"))
                return ServerHost.GitHub;
            return ServerHost.Unknown;
        }

        public string Download(string inputURL, Action<object, AsyncCompletedEventArgs> then, Action<object, DownloadProgressChangedEventArgs> onProgress)
        {
            var host = GetServerHost(inputURL);

            using WebClient wc = new();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(then);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(onProgress);

            var url = URL.GetURLRedirect(inputURL);
            var output = GetFileNameFromServer(url);

            if (File.Exists(output))
                File.Delete(output);

            wc.DownloadFileAsync(new Uri(url), output);
            return output;
        }
    }
}
