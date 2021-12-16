using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Parser
{
    public class GroupIdFinder : IGroupIdFinder
    {
        private GroupsDataParser parser = new();
        private HashSet<string> MathMechGroups { get; }

        public bool IsGroupNumber(string text)
        {
            return MathMechGroups.Contains(text);
        }

        public GroupIdFinder(string allGroupsFilename, string mathMechGroupsFilename)
        {
            parser.GroupsFilename = allGroupsFilename;
            if (File.Exists(mathMechGroupsFilename))
            {
                var groups = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(mathMechGroupsFilename));
                if (groups is not null)
                {
                    MathMechGroups = groups.ToHashSet();
                    return;
                }
            }

            Console.Error.WriteLine($"{mathMechGroupsFilename} not found or corrupted");
            Environment.Exit(1);
        }

        public string FindGroupId(string groupName)
        {
            var json = parser.GetJson();
            var properties = parser.ParseJson(json)
                .Where(property => property.Title == groupName.ToUpper());
            return properties.First().Id.ToString();
        }

        private IEnumerable<GroupsDataParser> SelectMathMechGroups(IEnumerable<GroupsDataParser> properties)
        {
            return properties.Where(p => MathMechGroups.Contains(p.Title));
        }

        public IEnumerable<string> ExtractInstituteGroupsId(IEnumerable<string> institutesIds)
        {
            foreach (var id in institutesIds)
            {
                var json = parser.GetJson(id);
                var parseProperties = parser.ParseJson(json);
                var mathMechProperties = SelectMathMechGroups(parseProperties);
                var ids = mathMechProperties.Select(properties => properties.Id);
                foreach (var groupId in ids)
                    yield return groupId.ToString();
            }
        }
    }
}