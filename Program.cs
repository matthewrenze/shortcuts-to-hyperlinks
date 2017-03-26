using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShortcutsToHyperlinks
{
    class Program
    {
        static void Main(string[] args)
        {
            var folder = args[0];

            var files = Directory.GetFiles(folder, "*.url");

            var links = new List<Link>();

            foreach (var file in files)
            {
                var text = File.ReadAllText(file);

                var pattern = "URL=(?<url>.*)";

                var regex = new Regex(pattern);

                var match = regex.Match(text);

                var group = match.Groups["url"];

                var link = new Link();

                link.Title = Path.GetFileNameWithoutExtension(file);

                link.Url = group.Value;

                links.Add(link);
            }

            var html = new StringBuilder()
                .AppendLine("<!DOCTYPE html>")
                .AppendLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">")
                .AppendLine("<head>")
                .AppendLine("    <meta charset=\"utf-8\" />")
                .AppendLine("    <title></title>")
                .AppendLine("</head>")
                .AppendLine("<body>");

            foreach (var link in links)
                html.AppendLine("    <a href=\"" + link.Url + "\" target = \"_blank\">" + link.Title + "</a></br>");

            html.AppendLine("</body>")
                .AppendLine("</html>");

            var targetFile = Path.Combine(folder, "Hyperlinks.html");

            File.WriteAllText(targetFile, html.ToString());
            
            Console.WriteLine("Hyperlinks.html has been created in the source folder.");

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();
        }
    }
}
