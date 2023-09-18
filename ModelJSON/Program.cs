// See https://aka.ms/new-Debug-template for more information
using Leagueinator.Model;
using Leagueinator.Utility;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Diagnostics;

League league = new League();
league.AddEvent("", "", new LeagueSettings());
league.Events[0].Rounds[0].Matches[0].Teams[0].Players[0] = new PlayerInfo("Adam");

List<PlayerInfo> list = league.SeekDeep<PlayerInfo>();

Debug.WriteLine(league);
Debug.WriteLine(list.DelString());
Debug.WriteLine(list.Count);
