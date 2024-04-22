using Leagueinator.Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Forms.ResultsPlus {
    internal class ResultsList {

        public void AddResult(MatchRow matchRow) {

        }
    }

    internal struct Result {
        int index;
        int lane;
        string result;
        int bowlsFor;
        int bowlsAgainst;
        int tie;
        int scoreFor;
        int plusFor;
        int scoreAgainst;
        int plusAgainst;
        int endsPlayed;
    }
}
