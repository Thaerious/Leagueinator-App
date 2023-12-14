using Leagueinator.Model;

internal class MockEvent : LeagueEvent
{

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

        round1.Matches[0].Teams[0].Bowls = 30;
        round1.Matches[0].Teams[1].Bowls = 2;

        round1.Matches[1].Teams[0].AddPlayer(new("Ethan"));
        round1.Matches[1].Teams[0].AddPlayer(new("Fred"));
        round1.Matches[1].Teams[1].AddPlayer(new("Greg"));
        round1.Matches[1].Teams[1].AddPlayer(new("Harry"));

        round1.Matches[1].Teams[0].Bowls = 15;
        round1.Matches[1].Teams[1].Bowls = 9;

        var round2 = this.NewRound();

        round2.Matches[0].Teams[0].AddPlayer(new("Adam"));
        round2.Matches[0].Teams[0].AddPlayer(new("Betty"));
        round2.Matches[0].Teams[1].AddPlayer(new("Ethan"));
        round2.Matches[0].Teams[1].AddPlayer(new("Fred"));

        round2.Matches[0].Teams[0].Bowls = 1;
        round2.Matches[0].Teams[1].Bowls = 9;

        round2.Matches[1].Teams[0].AddPlayer(new("Greg"));
        round2.Matches[1].Teams[0].AddPlayer(new("Harry"));
        round2.Matches[1].Teams[1].AddPlayer(new("Charles"));
        round2.Matches[1].Teams[1].AddPlayer(new("Dianne"));

        round2.Matches[1].Teams[0].Bowls = 15;
        round2.Matches[1].Teams[1].Bowls = 7;
    }
}
