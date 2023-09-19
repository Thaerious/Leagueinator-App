using Leagueinator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leagueinator.Model {

    public interface HasRefresh {
        void Refresh();
    }

    /// <summary>
    /// Object for displaying a row in the FormReport class for a single round.
    /// </summary>
    internal class RoundDatum : HasRefresh {
        public LeagueEvent lEvent;
        private Round round;
        private Match match;
        private Team team;
        private PlayerInfo player;
        private Score score;

        public RoundDatum(LeagueEvent lEvent, Round round, PlayerInfo player) {
            this.lEvent = lEvent;
            this.round = round;
            this.match = round.GetMatch(player);
            this.player = player;

            List<Team> teams = this.match.Teams.Values.NotNull().Where(t => t.Players.Contains(player)).ToList();
            if (teams.Count <= 0) throw new Exception("Match does not contain player");
            this.team = teams.First();
            this.score = new Score(this.match, this.team);
        }

        public string Name {
            get => this.player.Name;
        }

        public int Bowls {
            get => this.team.Bowls;
            set {
                this.team.Bowls = value;
                this.score = new Score(this.match, this.team);
            }
        }

        public int Ends {
            get => this.match.EndsPlayed;
            set => this.match.EndsPlayed = value;
        }

        public int Round { get => this.lEvent.Rounds.IndexOf(this.round) + 1; }
        public int Wins { get => this.score.Wins; }
        public int Ties { get => this.score.Ties; }
        public int Losses { get => this.score.Losses; }
        public int Against { get => this.score.Against; }
        public int PointsFor { get => this.score.PointsFor; }
        public int PlusFor { get => this.score.PlusFor; }
        public int PointsAgainst { get => this.score.PointsAgainst; }
        public int PlusAgainst { get => this.score.PlusAgainst; }

        public void Refresh() {
            this.score = new Score(this.match, this.team);
        }
    }
}
