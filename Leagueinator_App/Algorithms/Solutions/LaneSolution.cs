
using Leagueinator.Utility;
using Leagueinator_App;
using Model;

namespace Leagueinator.App.Algorithms.Solutions {
    public class LaneSolution : ASolution<Match> {
        private static readonly Random random = new Random();
        private readonly LeagueEvent LeagueEvent;
        public readonly Round Reference;
        public LeagueSettings Settings;

        public LaneSolution(LeagueEvent leagueEvent, Round round, LeagueSettings settings) : base(settings.LaneCount) {
            this.LeagueEvent = leagueEvent;
            this.Reference = round;
            this.Settings = settings;

            throw new NotImplementedException();
            //for (int lane = 0; lane < settings.LaneCount; lane++) {
            //    this[lane] = new Match(round.Matches[lane].Settings);
            //    this[lane].CopyFrom(round.Matches[lane]);
            //}
        }

        /// <summary>
        /// Using the match associated with each lane.
        /// Sum the number of times each player in that match has been in that lane.
        /// </summary>
        /// <returns></returns>
        public override int Evaluate() {
            int laneCount = this.Settings.LaneCount;
            int sum = 0;

            throw new NotImplementedException();
            //for (int lane = 0; lane < this.Size; lane++) {
            //    Match match = this[lane];
            //    match.Players.ForEach(p => sum += this.Count(p, lane));
            //}

            return sum;
        }

        /// <summary>
        /// Count the number of times 'player' has been in 'lane'.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="lane"></param>
        /// <returns></returns>
        internal int Count(string player, int lane) {
            int sum = 0;

            foreach (Round round in this.LeagueEvent.Rounds) {
                Match match = round.Matches[lane];
                if (match.Players.Contains(player)) {
                    sum++;
                }
            }

            return sum;
        }

        public override bool IsValid() {
            return true;
        }

        public override LaneSolution Clone() {
            throw new NotImplementedException();
            //LaneSolution that = new LaneSolution(this.LeagueEvent, this.Reference);
            //for (int i = 0; i < this.Size; i++) that[i] = this[i];
            //return that;
        }

        public override void Mutate() {
            int r1 = 0, r2 = 0;

            while (r1 == r2) {
                r1 = random.Next(this.Size);
                r2 = random.Next(this.Size);
            }

            (this[r1], this[r2]) = (this[r2], this[r1]);
        }

        public override string ToString() {
            string r = $"({this.GetHashCode().ToString("X")})";
            for (int m = 0; m < this.Size; m++) { 
                r += $"[{this[m].Players.DelString()}]";
            }
            return r;
        }
    }
}
