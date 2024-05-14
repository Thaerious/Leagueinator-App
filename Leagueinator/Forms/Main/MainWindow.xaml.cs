using Leagueinator.Model;
using Leagueinator.Controls;
using Leagueinator.Utility;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using Leagueinator.Model.Tables;
using System.Diagnostics;

namespace Leagueinator.Forms.Main {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private League _league = NewLeague();
        public League League {
            get => this._league;
            set {
                this.League.LeagueUpdate -= this.HndLeagueUpdate;
                this._league = value;
                this.EventRow = this.League.EventTable.GetLast();
                value.LeagueUpdate += this.HndLeagueUpdate;
            }
        }

        public MainWindow() {
            InitializeComponent();
            SaveState.StateChanged += this.HndStateChanged;
            this.CardStackPanel.CardStackPanelReorder += this.HndCardStackPanelReorder;
            this._league.LeagueUpdate += this.HndLeagueUpdate;
            this.EventRow = this.League.EventTable.GetLast();
        }

        /// <summary>
        /// Indirectly triggered when a change is made to the underlying model.
        /// </summary>
        /// <param name="isSaved">The new state isSaved</param>
        private void HndStateChanged(object? sender, bool isSaved) {
            if (isSaved) this.Title = $"Leagueinator [{SaveState.Filename}]";
            else this.Title = $"Leagueinator [{SaveState.Filename}] *";
        }

        /// <summary>
        /// Triggered when a change is made to the underlying model.
        /// </summary>
        /// <param name="IsSaved">The new state isSaved</param>
        private void HndLeagueUpdate(object? sender, EventArgs? e) {
            SaveState.ChangeState(sender, false);
        }

        private void HndCardStackPanelReorder(CardStackPanel panel, ReorderArgs args) {
            if (this.CurrentRoundRow is null) return;

            Dictionary<int, MatchRow> matchRows = [];

            foreach (int key in args.ReorderMap.Keys) {
                MatchRow? matchRow = this.CurrentRoundRow.Matches.Get(key);
                if (matchRow is not null) matchRows[matchRow.Lane] = matchRow;
            }

            foreach (int key in args.ReorderMap.Keys) {
                matchRows[key].Lane = args.ReorderMap[key];
            }
        }

        private void HndEventManagerClick(object sender, RoutedEventArgs e) {
            EventManager dialog = new EventManager(this.League);
            if (dialog.ShowDialog() == true) {
                if (dialog.Selected is null) return;

                League league = dialog.Selected.EventRow.League;
                EventRow eventRow = dialog.Selected.EventRow;
                if (eventRow.Rounds.Count == 0) {
                    RoundRow roundRow = eventRow.Rounds.Add();
                    roundRow.PopulateMatches();
                }

                this.League = dialog.Selected.EventRow.League;
                this.EventRow = dialog.Selected.EventRow;
            }
        }

        private void HndNewClick(object sender, RoutedEventArgs e) {
            this.Focus();
            this.League = NewLeague();
        }

        private void HndLoadClick(object sender, RoutedEventArgs e) {
            this.Focus();

            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "League Files (*.league)|*.league"
            };

            if (dialog.ShowDialog() == true) {
                League newLeague = new();
                newLeague.ReadXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;

                this.League = newLeague;
                this.EventRow = this.League.EventTable.GetLast();

                SaveState.ChangeState(sender, true);
            }
        }

        private static League NewLeague() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("Default Event", DateTime.Today.ToString("yyyy-MM-dd"));
            eventRow.Settings.Add("lanes", "8");
            eventRow.Settings.Add("teams", "2");
            eventRow.Settings.Add("ends", "10");

            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.PopulateMatches(8, 2);
            return league;
        }

        public void ClearFocus() {
            FocusManager.SetFocusedElement(this, null);
        }

        private void HndSaveClick(object sender, RoutedEventArgs e) {
            if (this.League is null) return;
            this.ClearFocus();

            if (SaveState.Filename.IsEmpty()) this.HndSaveAsClick(null, null);
            else this.League.WriteXml(SaveState.Filename);
            SaveState.ChangeState(sender, true);
        }

        private void HndSaveAsClick(object sender, RoutedEventArgs e) {
            if (this.League is null) return;
            this.ClearFocus();

            SaveFileDialog dialog = new();
            dialog.Filter = "League Files (*.league)|*.league";

            if (dialog.ShowDialog() == true) {
                this.League.WriteXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;
                SaveState.ChangeState(sender, true);
            }
        }
        private void HndCloseClick(object sender, RoutedEventArgs e) {
            // TODO check for unsaved changes
            // TODO remove current league
        }
        private void HndExitClick(object sender, RoutedEventArgs e) {
            this.Close(); // TODO check for unsaved changes
        }

        private void HndEventsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.EventTable);
        }
        private void HndRoundsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.RoundTable);
        }
        private void HndMatchesClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.MatchTable);
        }
        private void HndTeamsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.TeamTable);
        }
        private void HndMembersClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.MemberTable);
        }
        private void HndPlayersClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.PlayerTable);
        }
        private void HndIdleClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.IdleTable);
        }
        private void HndSettingsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.SettingsTable);
        }
        private void HndViewResults(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) throw new NullReferenceException(nameof(this.EventRow));
            var form = new PrinterForm(this.EventRow);
            form.Owner = this;
            form.Show();
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

        private void HndMenuClick(object sender, RoutedEventArgs e) {
            Debug.WriteLine("HndMenuClick");
            this.Focus();
        }
    }
}
