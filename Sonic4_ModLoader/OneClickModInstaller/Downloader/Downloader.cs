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

        public static void Download(string inputURL, Action<object, AsyncCompletedEventArgs> then, Action<object, DownloadProgressChangedEventArgs> onProgress)
        {
            string output;

            var host = Downloader.ServerHost.Other;
            if (inputURL.Contains("gamebanana.com"))
                host = Downloader.ServerHost.GameBanana;
            else if (inputURL.Contains("github.com"))
                host = Downloader.ServerHost.GitHub;

            using WebClient wc = new();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(then);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(onProgress);

            //Download link goes here
            var url = URL.GetURLRedirect(inputURL);

            //Getting file name of the archive
            if (host == Downloader.ServerHost.GitHub)
                //GitHub's redirect link is something like a request rather than a file "path" on a server
                output = url.Split('/')[^1];
            else
                output = url.Split('/')[^1];

            if (host == Downloader.ServerHost.GameBanana)
                //Well, it seems that GB's counter doesn't increase if you download
                //the file directly from the redirect url. But I'm not sure that
                //this works as well
                url = inputURL;

            if (File.Exists(output))
                File.Delete(output);

            wc.DownloadFileAsync(new Uri(url), output);
        }
    }
}
