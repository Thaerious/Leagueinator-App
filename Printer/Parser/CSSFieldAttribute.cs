namespace Leagueinator.CSSParser {

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CSS : Attribute {
        public readonly string? Key;
        public bool Inherited { get; init; } = false;

        public CSS() {
            this.Key = default;
        }

        public CSS(string key, bool inhertied) {
            this.Key = key;
            this.Inherited = inhertied;
        }
        public CSS(string key) {
            this.Key = key;
        }

        public CSS(bool inhertied) {
            this.Inherited = inhertied;
        }
    }
}
