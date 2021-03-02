using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AndroScrap
{
    class Program
    {
        const string urlOrig = "https://andro4all.com/listas/apps-android";

        static void Main(string[] args)
        {
            List<string> linksToVisit = ParseLinks(urlOrig);

        }

        public static List<String> ParseLinks(String url)
        {
            WebClient webClient = new WebClient();

            byte[] data = webClient.DownloadData(url);
            string download = Encoding.ASCII.GetString(data);

            HashSet<string> list = new HashSet<string>();

            var doc = new HtmlDocument();
            doc.LoadHtml(download);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");

            foreach (var n in nodes)
            {
                string href = n.Attributes["href"].Value;
                if(GetAbsoluteUrlString(url, href).Contains("/listas/apps-android"))
                {
                   list.Add(GetAbsoluteUrlString(url, href));
                }
                
            }

            string result = string.Join(", \n", list);
            Console.WriteLine($"URLs:" +
                $"{result}");

            writeLines(result);

            return list.ToList();


        }

        public static async Task writeLines(string result)
        {
            await File.WriteAllTextAsync("urls.txt", result);
        }
        static string GetAbsoluteUrlString(string baseUrl, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(baseUrl), uri);
            return uri.ToString();
        }

    }
}
