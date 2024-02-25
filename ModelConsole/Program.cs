// See https://aka.ms/new-console-template for more information

using Model;
using Model.Tables;

League league = new();
EventRow eventRow = league.EventTable.AddRow("my_event");
RoundRow roundRow = eventRow.Rounds.Add();
MatchRow matchRow = roundRow.Matches.Add(0, 10);
TeamRow teamRow = matchRow.Teams.Add();

Console.ReadLine();
