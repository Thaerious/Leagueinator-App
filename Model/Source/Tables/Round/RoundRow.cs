using Model.Source.Tables.Round;
using System.Data;

namespace Leagueinator.Model.Tables {
    public class RoundRow : CustomRow {
        public readonly RoundBoundIdles IdlePlayers;
        public readonly RoundBoundMatches Matches;

        internal RoundRow(DataRow dataRow) : base(dataRow) {
            this.IdlePlayers = new(this);
            this.Matches = new(this);
        }

        internal int UID => (int)this[RoundTable.COL.UID];

        public EventRow Event => this.League.EventTable.GetRow((int)this[RoundTable.COL.EVENT]);


        public IReadOnlyList<string> AllPlayers {
            get {
                List<string> allPlayers = [];
                foreach (IdleRow idleRow in this.IdlePlayers) {
                    allPlayers.Add(idleRow.Player);
                }

                allPlayers.AddRange(this.Matches
                    .SelectMany(matchRow => matchRow.Teams)
                    .SelectMany(teamRow => teamRow.Members)
                    .Select(memberRow => memberRow.Player)
                    .ToList()
                );

                return allPlayers;
            }
        }

        /// <summary>
        /// Retreive a collection of team rows for all matches in this round.
        /// </summary>
        public IEnumerable<TeamRow> Teams {
            get {
                return this.Matches.SelectMany(matchRow => matchRow.Teams);
            }
        }

        public IEnumerable<MemberRow> Members {
            get {
                return this.Teams.SelectMany(teamRow => teamRow.Members);
            }
        }

        /// <summary>
        /// Add empty matches to this round so that there are as many matches as the
        /// lane count value of the parent event.
        /// </summary>
        /// <param name="laneCount"></param>
        /// <returns></returns>
        public RoundRow PopulateMatches() {
            int ends = this.Event.EndsDefault;
            int laneCount = this.Event.LaneCount;

            for (int lane = 0; lane < laneCount; lane++) {
                if (!this.Matches.Has(lane)) {
                    this.Matches.Add(lane, ends);
                }
            }
            return this;
        }

        /// <summary>
        /// Return a 1-indexed value for the next lane.
        /// The next lane is one larger than the largest lane value.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private int NextLane() {
            int next = 0;
            foreach (MatchRow matchRow in this.Matches) {
                if (next <= matchRow.Lane) next = matchRow.Lane + 1;
            }

            return next;
        }

        /// <summary>
        /// Removes all members with the specified player's name from the Members collection for
        /// for any match/team in this round.
        /// </summary>
        /// <param name="name">The name of the player to be removed from the member table.</param>
        public void RemoveNameFromTeams(string name) {
            // TODO check for performance
            foreach (TeamRow teamRow in this.Teams) {
                foreach (MemberRow memberRow in teamRow.Members) {
                    if (memberRow.Player == name) memberRow.Remove();
                }
            }
        }
    }
}

