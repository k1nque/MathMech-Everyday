using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Parser
{
    public static class GroupIdFinder
    {
        private class ParseProperties
        {
            public int external_id { get; set; }
            public bool actual { get; set; }
            public int course { get; set; }
            public bool updated { get; set; }
            public int id { get; set; }
            public int institute_id { get; set; }
            public string title { get; set; }
        }
        
        private static string GetJson() =>
            new WebClient().DownloadString("https://urfu.ru/api/schedule/groups/");

        private static List<ParseProperties> ParseJson(string json) =>
            JsonSerializer.Deserialize<List<ParseProperties>>(json);

        public static int FindGroupId(string id)
        {
            var json = GetJson();
            var properties = ParseJson(json)
                .Where(property => property.title == id.ToUpper());
            return properties.First().id;
        }
    }
}
