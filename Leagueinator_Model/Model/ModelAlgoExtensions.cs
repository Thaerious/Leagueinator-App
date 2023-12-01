namespace Leagueinator.Model {
    public static class ModelAlgoExtensions {

        /// <summary>
        /// Copy players from the first round to the target round.</br>
        /// Retains the teams composition and order.</br>
        /// </summary>
        /// <param name="lEvent"></param>
        /// <param name="target"></param>
        public static void CopyPlayersTo(this LeagueEvent lEvent, Round target) {
            Round source = lEvent.Rounds[0];
            if (source is null) return;

            target.ResetPlayers();

            for (int i = 0; i < source.Matches.Count; i++) {
                Match match = source.Matches[i];
                for (int j = 0; j < match.Teams.Count; j++) {
                    Team? team = match.Teams[j];
                    if (team is null) continue;
                    for (int k = 0; k < team.Players.Count; k++) {
                        PlayerInfo? player = team.Players[k];
                        if (player != null) {
                            target.IdlePlayers.Remove(player);
                            target.Matches[i].Teams[j].Players[k] = player;
                        }
                    }
                }
            }
        }
    }
}
