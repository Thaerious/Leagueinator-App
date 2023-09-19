namespace Leagueinator.App.Forms {
    public class IsSaved {
        public delegate void IsSavedEvent(bool value);
        public static event IsSavedEvent Update = delegate { }; // BLOG

        private IsSaved() { }

        public static bool _value = false;
        public static bool Value {
            get => _value;
            set {
                if (value != _value) {
                    _value = value;
                    Update?.Invoke(value);
                }
            }
        }
    }
}
