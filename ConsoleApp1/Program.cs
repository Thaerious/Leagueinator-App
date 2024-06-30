using Leagueinator.Model;
using Leagueinator.Model.Tables;

internal class Program {
    private static void Main(string[] args) {
        Type? type = Type.GetType("Leagueinator.Model.Tables.MatchRow, Model");
        Console.WriteLine(type is not null);
        Console.ReadKey();
    }
}

internal enum TestEnum { Test };
