namespace Leagueinator.CSSParser {

    [AttributeUsage(AttributeTargets.Field)]
    public class CSS : Attribute{
        public readonly string? Key;

        public CSS() {
            this.Key = default;
        }

        public CSS(string key) {
            this.Key = key;
        }
    }
}
