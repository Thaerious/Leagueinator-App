namespace Leagueinator.App.Forms {
    public class IsSaved {
        public delegate void IsSavedEvent(bool value);
        public event IsSavedEvent Update = delegate { }; // BLOG
        public static IsSaved Singleton = new IsSaved();

        private IsSaved() { }

        public bool _value = false;
        public bool Value {
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
