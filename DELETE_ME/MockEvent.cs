using Leagueinator.Model;

internal class MockEvent : LeagueEvent {

    public static LeagueSettings settings = new() {
        TeamSize = 2,
        LaneCount = 8,
        MatchSize = 2,
        NumberOfEnds = 10
    };

    public MockEvent() : base("2023-11-23", "mock", MockEvent.settings) {
        var round1 = this.NewRound();

        round1.Matches[0].Teams[0].AddPlayer(new("Adam"));
        round1.Matches[0].Teams[0].AddPlayer(new("Betty"));
        round1.Matches[0].Teams[1].AddPlayer(new("Charles"));
        round1.Matches[0].Teams[1].AddPlayer(new("Dianne"));

        round1.Matches[0].Teams[0].Bowls = 20;
        round1.Matches[0].Teams[1].Bowls = 7;

        var round2 = this.NewRound();

        round2.Matches[0].Teams[0].AddPlayer(new("Adam"));
        round2.Matches[0].Teams[0].AddPlayer(new("Betty"));
        round2.Matches[0].Teams[1].AddPlayer(new("Charles"));
        round2.Matches[0].Teams[1].AddPlayer(new("Dianne"));

        round2.Matches[0].Teams[0].Bowls = 3;
        round2.Matches[0].Teams[1].Bowls = 7;
    }
}
