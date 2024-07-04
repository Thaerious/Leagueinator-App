namespace Leagueinator.Forms.Main {
    public partial class MainWindow {
        static class SaveState {
            public delegate void StateChangedHandler(object? source, bool IsSaved);
            public static event StateChangedHandler StateChanged = delegate { };
            private static string _filename = "";

            public static bool IsSaved {
                get; private set;
            }

            public static void ChangeState(object? sender, bool isSaved) {
                IsSaved = isSaved;
                StateChanged.Invoke(sender, isSaved);
            }

            public static string Filename { get => _filename; set => _filename = value; }
        }
    }
}
