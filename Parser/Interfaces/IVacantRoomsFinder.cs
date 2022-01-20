using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parser
{
    public interface IVacantRoomsFinder
    {
        Task<IEnumerable<string>> FindVacant(DateTime timeToFind);
    }
}