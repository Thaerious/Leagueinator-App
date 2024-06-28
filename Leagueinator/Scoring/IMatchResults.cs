using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Scoring {
    /// <summary>
    /// The results of a single match for a single team.
    /// </summary>
    /// <param name="teamRow"></param>    
    public record MatchResultsPlus {
        public interface IMatchResults {
        }
    }
}
