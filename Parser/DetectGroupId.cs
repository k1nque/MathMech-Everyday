using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Parser
{
    public static class DetectGroupId
    {
        private static string GetJson(string course)
        {
            using WebClient client = new WebClient();
            string htmlCode = client.DownloadString($"https://urfu.ru/api/schedule/groups/25714/{course}/");
            var regex = new Regex("<pre.*\">(.+?)</pre>");
            MatchCollection matches = regex.Matches(htmlCode);
            var match = matches[0].Value;
            return match;
        }

        private static List<ParseProperties> ParseJson(string json)
        {
            var parseProperties = JsonSerializer.Deserialize<List<ParseProperties>>(json);
            return parseProperties;
        }

        public static int FindId(string id, string course)
        {
            var json = GetJson(course);
            var parseProperties = ParseJson(json);

            var parseProperty = new ParseProperties();
            foreach (var property in parseProperties)
            {
                if (property.title.Contains(id))
                {
                    return property.id;
                }
            }

            return -1;
        }
    }
}
