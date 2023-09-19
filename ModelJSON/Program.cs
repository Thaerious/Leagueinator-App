// See https://aka.ms/new-Debug-template for more information
using Leagueinator.Model;
using Leagueinator.Utility;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Diagnostics;

Match match = new Match(new LeagueSettings());

var json = JsonConvert.SerializeObject(match);
JsonConvert.DeserializeObject<Match>(json);

Debug.WriteLine(json);
