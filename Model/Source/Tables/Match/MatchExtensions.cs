namespace Leagueinator.Model.Tables.Match {
    public static class MatchExtensions {
        public static int TeamCount(this MatchFormat matchFormat) {
            switch (matchFormat) {
                case MatchFormat.A4321:
                    return 4;
                default:
                    return 2;
            }
        }
    }
}
