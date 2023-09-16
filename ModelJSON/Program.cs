// See https://aka.ms/new-Debug-template for more information
using Leagueinator.Model;
using Leagueinator.Utility;
using Newtonsoft.Json;
using System.Diagnostics;

//Team team = new Team(new LeagueSettings());
//team.Players[0] = new PlayerInfo("Adam");
//team.Players[1] = new PlayerInfo("Bert");

//Debug.WriteLine(team?.ToXML());

//var teamOutput = JsonConvert.SerializeObject(team);
//Debug.WriteLine(teamOutput);

//Team? teamRestored = JsonConvert.DeserializeObject<Team>(teamOutput);
//Debug.WriteLine(teamRestored?.ToXML());

//Debug.WriteLine("\n");

//Match match = new Match(new LeagueSettings());

//match.Teams[0] = new Team(match.Settings);
//match.Teams[0].Players[0] = new PlayerInfo("Adam");
//match.Teams[0].Players[1] = new PlayerInfo("Bert");

//match.Teams[1] = new Team(match.Settings);
//match.Teams[1].Players[0] = new PlayerInfo("Chuck");
//match.Teams[1].Players[1] = new PlayerInfo("Dave");

//Debug.WriteLine(match?.ToXML(1));

//var matchOutput = JsonConvert.SerializeObject(match);
//Debug.WriteLine(matchOutput);

//Match? matchRestored = JsonConvert.DeserializeObject<Match>(matchOutput);
//Debug.WriteLine(matchRestored?.ToXML(1));

//Debug.WriteLine("\n");

LeagueEvent lEvent = new LeagueEvent(new LeagueSettings());
//lEvent.NewRound();
//lEvent.Rounds[0].Matches[0] = match;
//Debug.WriteLine(lEvent?.ToXML());

var lEventOutput = JsonConvert.SerializeObject(lEvent);
Debug.WriteLine(lEventOutput);

LeagueEvent? lEventRestored = JsonConvert.DeserializeObject<LeagueEvent>(lEventOutput);
Debug.WriteLine(lEventRestored?.ToXML());
