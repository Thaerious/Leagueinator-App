using Leagueinator.Model;
using System.Diagnostics;

namespace DevPrint {
    internal class MockEvent : LeagueEvent {

        public static LeagueSettings settings = new() {
            TeamSize = 2,
            LaneCount = 8,
            MatchSize = 2,
            NumberOfEnds = 10
        };

        public MockEvent() : base("2023-11-23", "mock", MockEvent.settings) {
            var round = this.NewRound();

            round.Matches[0].Teams[0].AddPlayer(new("Adam"));
            round.Matches[0].Teams[0].AddPlayer(new("Betty"));
            round.Matches[0].Teams[1].AddPlayer(new("Charles"));
            round.Matches[0].Teams[1].AddPlayer(new("Dianne"));

            round.Matches[1].Teams[0].AddPlayer(new("Adam"));
            round.Matches[1].Teams[0].AddPlayer(new("Betty"));
            round.Matches[1].Teams[1].AddPlayer(new("Charles"));
            round.Matches[1].Teams[1].AddPlayer(new("Dianne"));

            round.Matches[0].Teams[0].Bowls = 5;
            round.Matches[0].Teams[1].Bowls = 7;
        }
    }
}
