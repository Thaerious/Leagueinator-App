using Leagueinator.Model;
using Leagueinator.Model.Tables;

internal class Program {
    private static void Main(string[] args) {
        League league = new();
        EventRow eventRow = league.Events.Add("Ima Event");

        eventRow.Name = "Ima new name";

        Console.ReadLine();
    }
}
