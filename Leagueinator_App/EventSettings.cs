namespace Leagueinator_App {
        public enum MATCH_TYPE {
        RoundRobin, Brackets, Ranked, Penache, None
    }

    [Serializable]
    public struct EventSettings {
        public int TeamSize = 2;
        public int LaneCount = 8;
        public int MatchSize = 2;
        public int NumberOfEnds = 10;

        public MATCH_TYPE MatchType = MATCH_TYPE.RoundRobin;
        public string Date = DateTime.Now.ToShortDateString();
        public string Name = "N/A";

        public EventSettings() {}
    }
}
