using System.Net;

namespace Common.URL
{
    public class URL
    {
        public static string GetURLRedirect(string url)
        {
            var reqiest = WebRequest.Create(url);
            var response = reqiest.GetResponse();
            var redir = response.ResponseUri;
            response.Close();
            return redir.ToString();
        }
    }
}