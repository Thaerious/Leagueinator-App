using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.App.Scoring.Plus {
    public class PlusScore {
        public readonly PlusRounds PlusRounds;
        public readonly PlusTeams PlusTeams;

        public PlusScore(LeagueEvent leagueEvent) {
            this.PlusRounds = new(leagueEvent);
            this.PlusTeams = new();

            this.FillTeams();
        }

        private void FillTeams() {
            
        }
    }
}
