using Leagueinator.Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Extensions {
    internal static class MatchRowExtensions {
        /// <summary>
        /// Move all members from excess teams to idle and remove the team
        /// until the MatchRow has 'count' teams.
        /// </summary>
        /// <param name="count"></param>
        public static void ReduceTeams(this MatchRow matchRow, int count) {
            while (matchRow.Teams.Count > count) {
                TeamRow teamRow = matchRow.Teams[^1]!;
                foreach (MemberRow memberRow in teamRow.Members) {
                    matchRow.Round.IdlePlayers.Add(memberRow.Player);
                }
                teamRow.Remove();
            }
        }

        /// <summary>
        /// Set teams to the MatchRow until it has 'count' teams.
        /// </summary>
        /// <param name="matchRow"></param>
        /// <param name="count"></param>
        public static void IncreaseTeams(this MatchRow matchRow, int count) {
            while (matchRow.Teams.Count < count) {
                matchRow.Teams.Add(matchRow.Teams.Count);
            }
        }

        /// <summary>
        /// Reduce the size of each team until it has 'count' members or less.
        /// </summary>
        /// <param name="matchRow"></param>
        /// <param name="teamSize"></param>
        public static void TrimTeams(this MatchRow matchRow, int count) {
            foreach (TeamRow teamRow in matchRow.Teams) {
                while (teamRow.Members.Count > count) {
                    string player = teamRow.Members[^1]!.Player;
                    // model will remove player from team when added to idle.
                    matchRow.Round.IdlePlayers.Add(player);
                }
            }
        }
    }
}
