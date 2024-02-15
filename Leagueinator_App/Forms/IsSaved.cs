namespace Leagueinator.App.Forms {
    public class IsSaved {
        public delegate void IsSavedEvent(bool value);
        public static event IsSavedEvent Update = delegate { };

        private IsSaved() { }

        private static bool _value = true;
        public static bool Value {
            get => _value;
            set {
                if (value == _value) return;
                _value = value;
                Update?.Invoke(value);
            }
        }
    }
}
