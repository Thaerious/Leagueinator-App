using Model;
using Model.Tables;
using System.Data;
using System.Diagnostics;
using System.Text;

Part5();
Part6();
Part7();
Debug.WriteLine("Done");
Console.WriteLine("Press any key to continue.");
Console.ReadKey();

void Part5() {
    Debug.WriteLine("Part 5");
    League league = new();
    LeagueEvent leagueEvent = league.NewLeagueEvent("myEvent");
    var row1 = league.IdleTable.NewRow();
    row1["event_dir_uid"] = 0;
    row1["round"] = 0;
    row1["player_name"] = "ima_name_5";
    league.IdleTable.Rows.Add(row1);

    Debug.WriteLine(league.IdleTable.PrettyPrint("Part 5"));
}


void Part6() {
    Debug.WriteLine("Part 6");
    League league = new();
    LeagueEvent leagueEvent = league.NewLeagueEvent("myEvent");
    Round round = leagueEvent.NewRound();
    var row1 = league.IdleTable.NewRow();
    row1["event_dir_uid"] = 0;
    row1["round"] = 0;
    row1["player_name"] = "ima_name_6";
    league.IdleTable.Rows.Add(row1);
}

void Part7() {
    Debug.WriteLine("Part 7");
    var wrapper = new Wrapper();
    var row3 = wrapper.IdleTable.NewRow();
    row3["event_dir_uid"] = 0;
    row3["round"] = 0;
    row3["player_name"] = "ima_name_6";
    wrapper.IdleTable.Rows.Add(row3);
}

public class Wrapper : DataSet {

    public RoundTable EventTable { get; } = new();
    public TeamTable TeamTable { get; } = new();
    public IdleTable IdleTable { get; } = new();
    public EventDirectoryTable EventDirectoryTable { get; } = new();
    public EventSettingsTable EventSettings { get; } = new();

    public Wrapper() {
        Tables.Add(EventTable);
        Tables.Add(TeamTable);
        Tables.Add(EventDirectoryTable);
        Tables.Add(IdleTable);
        Tables.Add(EventSettings);
    }

    public string PrettyPrint() {
        return EventTable.PrettyPrint() + "\n" +
            TeamTable.PrettyPrint() + "\n" +
            IdleTable.PrettyPrint() + "\n" +
            EventDirectoryTable.PrettyPrint() + "\n" +
            EventSettings.PrettyPrint();
    }
}
