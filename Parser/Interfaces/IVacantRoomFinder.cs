using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parser
{
    public interface IVacantRoomFinder
    {
        Task<IEnumerable<string>> FindVacant(DateTime timeToFind);
    }
}