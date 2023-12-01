using Leagueinator.Utility;

namespace Leagueinator.Model {
    public enum PlayerType {
        HUMAN, PLACEHOLDER
    };

    [Serializable]
    public class PlayerInfo : IEquatable<PlayerInfo> {
        private string _name = "";

        public string Name {
            get => this._name;
            set {
                if (value is null) throw new ArgumentNullException(nameof(value));
                if (value.Trim() == "") throw new ArgumentException("value is empty");
                this._name = value.Trim();
            }
        }

        public PlayerType PlayerType { get; set; } = PlayerType.HUMAN;

        public PlayerInfo(string name) {
            this.Name = name;
        }

        public PlayerInfo DeepCopy() {
            return new PlayerInfo(this.Name);
        }

        public override string ToString() {
            return this.Name;
        }

        public override bool Equals(object? obj) {
            return obj is not null && obj is PlayerInfo info && this.Equals(info);
        }

        public override int GetHashCode() {
            return this.Name.GetHashCode();
        }

        public bool Equals(PlayerInfo? obj) {
            if (obj is null) return false;
            if (obj is null) return false;
            PlayerInfo that = obj;
            return this.Name.ToLower().Trim() == that.Name.ToLower().Trim();
        }

        public XMLStringBuilder ToXML() {
            var xsb = new XMLStringBuilder();
            xsb.OpenTag("Player");
            xsb.Attribute("hash", this.GetHashCode("X"));
            return xsb;
        }
    }

    public class PlayerBye : PlayerInfo {
        public PlayerBye() : base("-BYE-") {
            this.PlayerType = PlayerType.PLACEHOLDER;
        }
    }
}
