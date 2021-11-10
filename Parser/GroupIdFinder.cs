using System;
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
        
        private static string GetJson(string institute, string course)
        {
            using var client = new WebClient();
            var jsonUrl = $"https://urfu.ru/api/schedule/groups/{institute}/{course}/";
            return client.DownloadString(jsonUrl);
        }

        private static List<ParseProperties> ParseJson(string json) =>
            JsonSerializer.Deserialize<List<ParseProperties>>(json);

        public static int FindGroupId(string institute, string course, string id)
        {
            var json = GetJson(institute, course);
            var properties = ParseJson(json)
                .Where(property => property.title.Contains(id));
            return properties.First().id;
        }
    }
}
