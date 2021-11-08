using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Parser
{
    public static class GroupIdFinder
    {
        private static string GetJson(string course)
        {
            using var client = new WebClient();
            var htmlCode = client.DownloadString(
                $"https://urfu.ru/api/schedule/groups/25714/{course}/");
            var regex = new Regex("<pre.*\">(.+?)</pre>");
            return regex.Matches(htmlCode).First().Value;
        }

        private static List<ParseProperties> ParseJson(string json) =>
            JsonSerializer.Deserialize<List<ParseProperties>>(json);

        public static int FindGroupId(string id, string course)
        {
            var json = GetJson(course);
            var properties = ParseJson(json)
                .Where(property => property.title.Contains(id));
            return properties.First().id;
        }
    }
}
