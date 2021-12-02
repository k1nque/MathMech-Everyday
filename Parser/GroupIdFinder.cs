using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        private static string GetJson(string instituteId = default)
        {
            var result = new WebClient().DownloadString(
                $"https://urfu.ru/api/schedule/groups/{instituteId}");

            // fix for empty answers
            if (result.Length <= 4)
            {
                return File.ReadAllText("groups.json");
            }
            File.WriteAllText("groups.json", result);
            return result;
        }

        private static List<ParseProperties> ParseJson(string json) =>
            JsonSerializer.Deserialize<List<ParseProperties>>(json);

        public static string FindGroupId(string id)
        {
            var json = GetJson();
            var properties = ParseJson(json)
                .Where(property => property.title == id.ToUpper());
            return properties.First().id.ToString();
        }
        
        private static IEnumerable<ParseProperties> SelectMathMechGroups(List<ParseProperties> properties)
        {
            var regex = new Regex("МЕНМ?-..(?:020[1678]|220[13]|010[12]|0705|" +
                                  "08[01][0129]|10[01][15]|3201|0[12789][01][0123])");
            return properties
                .Where(property => regex.IsMatch(property.title));
        }
        
        public static IEnumerable<string> FindInstituteGroupsId(IEnumerable<string> institutesIds)
        {
            var ids = new List<IEnumerable<int>>();
            foreach (var id in institutesIds)
            {
                var json = GetJson(id);
                var parseProperties = ParseJson(json);
                var mathMechProperties = SelectMathMechGroups(parseProperties);
                ids.Add(mathMechProperties.Select(properties => properties.id));
            }
            return ids.SelectMany(list => 
                list.Select(id => id.ToString()));
        }
    }
}