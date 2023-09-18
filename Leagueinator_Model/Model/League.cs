using Leagueinator.Utility;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;

namespace Leagueinator.Model {
    [Serializable]
    public class League {
        [JsonProperty] [DoSeek] public List<LeagueEvent> Events { get; private set; } = new List<LeagueEvent>();


        [JsonConstructor]
        public League() {}

        public League(List<LeagueEvent> events) {
            this.Events = events;
        }

        /// <summary>
        /// Add a new Event to the league.
        /// Will add all current league players to the event.
        /// </summary>
        /// <returns></returns>
        public LeagueEvent AddEvent(string eventName, string date, LeagueSettings settings) {
            var lEvent = new LeagueEvent(eventName, date, settings);
            Round round = lEvent.NewRound();
            this.SeekDeep<PlayerInfo>().Unique().ForEach(round.IdlePlayers.Add);
            this.Events.Add(lEvent);
            return lEvent;
        }

        /// <summary>
        /// Construct a detailed string representation of a League object by combining 
        /// information about players and events, separated by newlines, and presented 
        /// in a formatted manner.
        /// </summary>
        /// <returns></returns>
        public XMLStringBuilder ToXML() {
            var xsb = new XMLStringBuilder();
            _ = xsb.OpenTag("League", $"hash='{this.GetHashCode():X}'");
            _ = xsb.InlineTag("Players", this.SeekDeep<PlayerInfo>().DelString());
            foreach (LeagueEvent lEvent in this.Events) {
                _ = xsb.AppendXML(lEvent.ToXML());
            }
            _ = xsb.CloseTag();
            return xsb;
        }

        public override string ToString() {
            return this.ToXML().ToString();
        }
    }
}
