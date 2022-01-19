using System.Collections.Generic;

namespace Parser
{
    public interface IGroupIdFinder
    {
        string FindGroupId(string groupName);

        IEnumerable<string> ExtractInstituteGroupsId(
            IEnumerable<string> institutesIds);

        bool IsGroupName(string text);
    }
}