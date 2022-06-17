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
    public static class Downloader
    {
        public enum ServerHost
        {
            GameBanana,
            GitHub,
            Other
        }

        public static string GetFileNameFromServer(string url, ServerHost? host=null)
        {
            if (host == null) host = GetServerHost(url);
            if (host == ServerHost.GitHub)
                return url.Split('/')[^1];
            return url.Split('/')[^1];
        }

        public static ServerHost GetServerHost(string url)
        {
            if (url.Contains("gamebanana.com"))
                return ServerHost.GameBanana;
            else if (url.Contains("github.com"))
                return ServerHost.GitHub;
            return ServerHost.Other;
        }

        public static string Download(string inputURL, Action<object, AsyncCompletedEventArgs> then, Action<object, DownloadProgressChangedEventArgs> onProgress)
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
