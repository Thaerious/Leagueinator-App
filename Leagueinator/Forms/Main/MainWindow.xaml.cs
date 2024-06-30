using Leagueinator.Controls;
using Leagueinator.Forms.MatchAssignments;
using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Scoring;
using Leagueinator.Utility;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using static Leagueinator.Controls.MatchCard;

namespace Leagueinator.Forms.Main {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static string VERSION = "0.1.1";

        public MainWindow() {
            this.Closed += (s, e) => {
                Debug.WriteLine("TimeTrace Report");
                Debug.WriteLine(TimeTrace.Report());
            };

            this.InitializeComponent();
            this.League = NewLeague();

            SaveState.StateChanged += this.HndStateChanged;
            this._league.LeagueUpdate += this.HndLeagueUpdate;
            this.EventRow = this.League.EventTable.GetLast();

            SaveState.Filename = "";
            SaveState.ChangeState(this, false);

            this.AddHandler(MatchCard.RegisteredFormatChangedEvent, new FormatChangedEventHandler(this.HndFormatChanged));
        }               

        private static League NewLeague() {
            League league = new();
            EventRow eventRow = league.Events.Add("Default Event", DateTime.Today.ToString("yyyy-MM-dd"));
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.PopulateMatches();
            league.LeagueTable.Set("ScoringFormat", ScoringFormat.POINTS_PLUS);
            return league;
        }

        private void ClearFocus() {
            FocusManager.SetFocusedElement(this, null);
        }

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
