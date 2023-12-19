
using Leagueinator.Utility;
using LLeagueinator.App.Forms.Main;
using System.Diagnostics;
using Model;

namespace Leagueinator.App.Forms.Main {
    public class Scramble {
        private readonly Round refRound;
        private readonly Round targetRound;

        public Scramble(Round reference, Round target) {
            this.refRound = reference;
            this.targetRound = target;

            string[] rr = this.refRound.AllPlayers.ToArray();
        }

        public void DoScramble(int count) {
            List<string> players = new();

            foreach(Match match in refRound.Matches) {
                foreach(Team team in match.Teams) {
                    foreach(string player in team.Players) {
                        if (player is null) continue;
                        players.Add(player);
                    }
                }
            }

            if (players.Count == 0) return;

            var opposite = players.ToOpposite();
            var shifted = opposite;
            for (int i = 0; i < count; i++) shifted = shifted.Shift();
            var adjacent = shifted.ToAdjacent();
            this.AssignTeams(adjacent);
        }

        private string[] Shift(string[] players, int count) {
            while (count-- > 0) {
                string temp = players[1];
                for (int i = 1; i < players.Length - 1; i++) {
                    players[i] = players[i + 1];
                }
                players[players.Length - 1] = temp;
            }
            return players;
        }

        private void AssignTeams(List<string> players) {
            this.targetRound.ResetPlayers();
            Queue<string> queue = new Queue<string>(players);
            Debug.WriteLine(players.DelString());

            int m = 0;
            foreach (Match refMatch in this.refRound.Matches) {
                Match targetMatch = this.targetRound.Matches[m++];

                int t = 0;
                foreach (Team refTeam in refMatch.Teams.Values.NotNull()) {
                    Team? targetTeam = targetMatch.Teams[t++];
                    
                    for (int i = 0; i < refTeam.Players.Count; i++) {
                        string player = queue.Dequeue();
                        targetTeam.AddPlayer(player);
                        if (this.targetRound.IdlePlayers.Contains(player) ) {
                            this.targetRound.IdlePlayers.Remove(player);
                        }
                    }
                }
            }
        }
    }
}
