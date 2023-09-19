namespace Leagueinator.Model {
    [Serializable]
    public enum Result {
        WIN, LOSS, TIE
    }

    [Serializable]
    public class Score : IComparable<Score> {
        public static readonly double factor = 1.5;

        public int Wins { get => this.wins; private set => this.wins = value; }
        public int Ties { get => this.ties; private set => this.ties = value; }
        public int Losses { get => this.losses; private set => this.losses = value; }
        public int PointsFor { get => this.pointsFor; private set => this.pointsFor = value; }
        public int PlusFor { get => this.plusFor; private set => this.plusFor = value; }
        public int PointsAgainst { get => this.pointsAgainst; private set => this.pointsAgainst = value; }
        public int PlusAgainst { get => this.plusAgainst; private set => this.plusAgainst = value; }

        public int Bowls { get => this.bowls; private set => this.bowls = value; }
        public int Against { get => this.against; private set => this.against = value; }
        public int Ends { get => this.ends; private set => this.ends = value; }

        public int[] ToArray() {
            return new int[] {
                this.wins,
                this.ties,
                this.losses,
                this.bowls,
                this.against,
                this.pointsFor,
                this.plusFor,
                this.pointsAgainst,
                this.plusAgainst
            };
        }

        public static bool operator ==(Score left, Score right) {
            return left.CompareTo(right) == 0;
        }

        public static bool operator !=(Score left, Score right) {
            return left.CompareTo(right) != 0;
        }

        public static bool operator >(Score left, Score right) {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Score left, Score right) {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(Score left, Score right) {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Score left, Score right) {
            return left.CompareTo(right) <= 0;
        }
        public Score ApplyFunction(Func<int, int> f) {
            var score = new Score {
                wins = f(this.wins),
                losses = f(this.losses),
                ties = f(this.ties),
                pointsFor = f(this.pointsFor),
                plusFor = f(this.plusFor),
                pointsAgainst = f(this.pointsAgainst),
                plusAgainst = f(this.plusAgainst),

                bowls = f(this.bowls),
                against = f(this.against),
                ends = f(this.ends)
            };

            return score;
        }

        public static Score operator +(Score left, Score right) {
            var score = new Score {
                wins = left.wins + right.wins,
                losses = left.losses + right.losses,
                ties = left.ties + right.ties,
                pointsFor = left.pointsFor + right.pointsFor,
                plusFor = left.plusFor + right.plusFor,
                pointsAgainst = left.pointsAgainst + right.pointsAgainst,
                plusAgainst = left.plusAgainst + right.plusAgainst,

                bowls = left.bowls + right.bowls,
                against = left.against + right.against,
                ends = left.ends + right.ends
            };

            return score;
        }

        public static Score operator /(Score left, int right) {
            return left.ApplyFunction(x => x / right);
        }

        public static Score operator *(Score left, int right) {
            return left.ApplyFunction(x => x * right);
        }

        public Score() { }

        public Score(int bowlsFor, int bowlsAgainst, int endsPlayed) {
            this.Init(bowlsFor, bowlsAgainst, endsPlayed);
        }

        public Score(Match match, Team team) {
            int against = 0;

            foreach (Team? them in match.Teams.Values) {
                if (them is null) continue;
                if (team == them) continue;
                against += them.Bowls;
            }

            this.Init(team.Bowls, against, match.EndsPlayed);
        }

        private void Init(int bowlsFor, int bowlsAgainst, int endsPlayed) {
            if (bowlsFor > bowlsAgainst) this.wins++;
            else if (bowlsFor < bowlsAgainst) this.losses++;
            else if (bowlsFor == bowlsAgainst) this.ties++;

            this.bowls = bowlsFor;
            this.against = bowlsAgainst;
            this.ends = endsPlayed;

            int max = (int)(endsPlayed * 1.5);

            this.pointsFor = this.bowls < max ? this.bowls : max;
            this.plusFor = this.bowls - this.pointsFor;

            this.pointsFor = this.bowls < max ? this.bowls : max;
            this.plusFor = this.bowls - this.pointsFor;

            this.pointsAgainst = this.against < max ? this.against : max;
            this.plusAgainst = this.against - this.pointsAgainst;
        }

        public override string ToString() {
            return $"[w:{this.wins} t:{this.ties} l:{this.losses} f:{this.pointsFor} f+:{this.plusFor} a:{this.against} a+:{this.plusAgainst} ]";
        }
        public override bool Equals(object? obj) {
            return obj is not null && this.CompareTo((Score)obj) == 0;
        }

        public override int GetHashCode() {
            return HashCode.Combine(this.wins, this.ties, this.losses, this.bowls, this.pointsFor, this.plusFor);
        }

        public int CompareTo(Score? that) {
            if (that is null) return 1;

            int[] left = this.ToArray();
            int[] right = that.ToArray();

            for (int i = 0; i < left.Length; i++) {
                if (left[i] > right[i]) return 1;
                if (right[i] > left[i]) return -1;
            }
            return 0;
        }

        private int wins = 0;
        private int ties = 0;
        private int losses = 0;
        private int pointsFor = 0;
        private int plusFor = 0;
        private int pointsAgainst = 0;
        private int plusAgainst = 0;

        private int bowls = 0;
        private int against = 0;
        private int ends = 0;
    }
}
