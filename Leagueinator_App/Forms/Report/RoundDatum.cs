using Leagueinator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leagueinator.Model {

    public interface HasRefresh {
        void BuildScore();
    }

    /// <summary>
    /// Object for displaying a row in the FormReport class for a single round.
    /// Combines an Event, a Round, and a _player.
    /// </summary>
    internal class RoundDatum : IComparable<RoundDatum>, IHasPlayer {
        public LeagueEvent lEvent;
        private Round round;
        private Match match;
        private Team team;
        private PlayerInfo player;
        private Score _score;

        public PlayerInfo Player => this.player;

        public RoundDatum(LeagueEvent lEvent, Round round, PlayerInfo player) {
            this.lEvent = lEvent;
            this.round = round;
            this.match = round.GetMatch(player);
            this.player = player;

            List<Team> teams = this.match.Teams.Values.NotNull().Where(t => t.Players.Contains(player)).ToList();
            if (teams.Count <= 0) throw new Exception("Match does not contain _player");
            this.team = teams.First();
            this._score = new Score(this.match, this.team);
        }

        public string Name {
            get => this.player.Name;
        }

        public int Bowls {
            get => this.team.Bowls;
            set {
                this.team.Bowls = value;
                this._score = new Score(this.match, this.team);
            }
        }

        public int Ends {
            get => this.match.EndsPlayed;
            set => this.match.EndsPlayed = value;
        }

        public Score Score { get => this._score; }
        [Editable(false)] public int Rank { get; set; } = -1;
        public int Round { get => this.lEvent.Rounds.IndexOf(this.round) + 1; }
        public int Wins { get => this._score.Wins; }
        public int Ties { get => this._score.Ties; }
        public int Losses { get => this._score.Losses; }
        public int Against { get => this._score.Against; }
        public int PointsFor { get => this._score.PointsFor; }
        public int PlusFor { get => this._score.PlusFor; }
        public int PointsAgainst { get => this._score.PointsAgainst; }
        public int PlusAgainst { get => this._score.PlusAgainst; }

        public int CompareTo(RoundDatum? that) {
            if (that == null) return 1;
            return this._score.CompareTo(that._score);
        }
    }
}
