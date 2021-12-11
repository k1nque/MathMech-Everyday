using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Parser
{
    public class GroupIdFinder: IGroupIdFinder
    {
        private readonly GroupsDataParser parser = new();
        
        public string FindGroupId(string groupName)
        {
            var json = parser.GetJson();
            var properties = parser.ParseJson(json)
                .Where(property => property.Title == groupName.ToUpper());
            return properties.First().Id.ToString();
        }
        
        private IEnumerable<GroupsDataParser> SelectMathMechGroups(IEnumerable<GroupsDataParser> properties)
        {
            var groups = GetGroups("MathMech.txt");
            return properties.Where(p => groups.Contains(p.Title));
        }

        private HashSet<string> GetGroups(string fileName)
        {
            var folderPath = Path.GetFullPath(@"..\..\..\..\Parser\GroupsData\");
            var data = File.ReadLines(folderPath + fileName);
            return data.ToHashSet();
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