using Leagueinator_Utility.Utility;
using System.Collections;
using System.Collections.ObjectModel;

namespace Leagueinator_Model.Model {
    [Serializable]
    public class LeagueEvent : IEnumerable<Round> {
        public readonly string Date;
        public readonly LeagueSettings Settings;
        public readonly string Name;

        [Model]
        public ObservableCollection<Round> Rounds {
            get; private set;
        } = new ObservableCollection<Round>();

        public List<Match> Matches => this.SeekDeep<Match>().ToList();

        public List<Team> Teams => this.SeekDeep<Team>().Where(t => !t.Players.Values.IsEmpty()).ToList();

        public List<PlayerInfo> Players => this.SeekDeep<PlayerInfo>().Unique();

        public LeagueEvent(string date, string name, LeagueSettings settings) {
            this.Date = date;
            this.Name = name;
            this.Settings = settings;
        }

        public LeagueEvent(LeagueSettings settings) {
            this.Date = DateTime.Today.ToString("yyyy-MM-dd");
            this.Name = "Event";
            this.Settings = settings;
        }

        /// <summary>
        /// Add a new empty round to this event.
        /// The new round will contain all players (idle) found in this event.
        /// The round will be populated with empty matches equal to the lane cound.
        /// </summary>
        public Round NewRound() {
            var round = new Round(this.SeekDeep<PlayerInfo>().Unique(), this.Settings);
            this.Rounds.Add(round);
            return round;
        }

        /// <summary>
        /// Add an existing round to this event.
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public Round AddRound(Round round) {
            this.Rounds.Add(round);
            return round;
        }

        /// <summary>
        /// Replace a round in this event with a specified round.
        /// </summary>
        /// <param name="replace"></param>
        /// <param name="with"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ReplaceRound(Round replace, Round with) {
            int index = this.Rounds.IndexOf(replace);

            if (index < 0) {
                throw new ArgumentException(
                    $"Attempting to replace round that is not a member of League Event"
                );
            }

            this.Rounds[index] = with;
        }

        public XMLStringBuilder ToXML() {
            var xsb = new XMLStringBuilder();

            _ = xsb.OpenTag("Event", $"name='{this.Name}' hash='{this.GetHashCode():X}'");
            _ = xsb.InlineTag("Players", this.SeekDeep<PlayerInfo>().DelString());
            foreach (Round round in this.Rounds) {
                _ = xsb.AppendXML(round.ToXML());
            }
            _ = xsb.CloseTag();

            return xsb;
        }
        public override string ToString() {
            return this.ToXML().ToString();
        }

        public IEnumerator<Round> GetEnumerator() {
            return this.Rounds.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.Rounds.GetEnumerator();
        }

        public int IndexOf(Round round) {
            return this.Rounds.IndexOf(round);
        }

        public Round PrevRound(Round round) {
            if (this.Rounds.Count < 2) throw new InvalidOperationException("Previous round not available");
            if (this.Rounds[0] == round) throw new InvalidOperationException("Previous round not available");

            for (int i = 1; i < this.Rounds.Count; i++) {
                if (this.Rounds[i] == round) return this.Rounds[i - 1];
            }
            throw new InvalidOperationException("Previous round not found");
        }

        public Round NextRound(Round round) {
            if (this.Rounds.Count < 2) throw new InvalidOperationException("Next round not available");
            if (this.Rounds.Last() == round) throw new InvalidOperationException("Next round not available");

            for (int i = 0; i < this.Rounds.Count - 1; i++) {
                if (this.Rounds[i] == round) return this.Rounds[i + 1];
            }
            throw new InvalidOperationException("Next round not found");
        }

        internal void RemoveRound(Round round) {
            _ = this.Rounds.Remove(round);
        }
    }

    public interface IModelLeagueEvent {
        LeagueEvent LeagueEvent { get; set; }
    }
}
