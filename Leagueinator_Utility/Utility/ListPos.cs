namespace Leagueinator_Utility.Utility {
    public enum ListPos {
        FIRST, LAST
    }

    public static class RelativePosition {
        public static T? Get<T>(this T[] array, ListPos pos) {
            return pos == ListPos.FIRST ? array[0] : pos == ListPos.LAST ? array[^1] : default;
        }
    }
}
