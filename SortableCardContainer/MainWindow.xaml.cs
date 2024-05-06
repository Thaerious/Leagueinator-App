using Leagueinator.Model;
using Leagueinator.Controls;
using Leagueinator.Utility;
using Microsoft.Win32;
using System.Windows;
using Leagueinator.Forms;
using Leagueinator.Model.Tables;
using System.Diagnostics;

namespace SortableCardContainer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private League _league = new();
        public League League { 
            get => this._league;
            set {
                if (this.League is not null) {
                    this.League.RowChanged -= this.HndLeagueRowChanged;
                }

                this._league = value;
                this.EventRow = this.League?.EventTable.GetLast() ?? null;

                if (this.League is not null) {
                    this.League.RowChanged += this.HndLeagueRowChanged;
                }
            }
        }

        /// <summary>
        /// Indirectly triggered when a change is made to the underlying model.
        /// </summary>
        /// <param name="IsSaved">The new state value</param>
        private void HndStateChanged(bool IsSaved) {
            if (IsSaved) this.Title = $"Leagueinator [{SaveState.Filename}]";
            else this.Title = $"Leagueinator [{SaveState.Filename}] *";
        }

        /// <summary>
        /// Triggered when a change is made to the underlying model.
        /// </summary>
        /// <param name="IsSaved">The new state value</param>
        private void HndLeagueRowChanged(object sender, System.Data.DataRowChangeEventArgs e) {
            SaveState.IsSaved = false;
        }

        public MainWindow() {
            InitializeComponent();
            this.NewLeague();
            SaveState.StateChanged += this.HndStateChanged;
            this.CardStackPanel.CardStackPanelReorder += this.HndCardStackPanelReorder;
        }

        private void HndCardStackPanelReorder(CardStackPanel panel, ReorderArgs args) {
            if (this.CurrentRoundRow is null) return;

            Dictionary<int, MatchRow> matchRows = [];

            foreach (int key in args.ReorderMap.Keys) {
                Debug.WriteLine($"HndCardStackPanelReorder {key} -> {args.ReorderMap[key]}");
                MatchRow? matchRow = this.CurrentRoundRow.Matches.Get(key);
                if (matchRow is not null) matchRows[matchRow.Lane] = matchRow;
            }

            foreach (int key in args.ReorderMap.Keys) {
                matchRows[key].Lane = args.ReorderMap[key];
            }

            Debug.WriteLine(this.CurrentRoundRow.League.MatchTable.PrettyPrint());
        }

        private void HndNewClick(object sender, RoutedEventArgs e) {
            this.NewLeague();
        }

        private void HndLoadClick(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "League Files (*.league)|*.league"
            };

            if (dialog.ShowDialog() == true) {
                League newLeague = new();
                newLeague.ReadXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;
                SaveState.IsSaved = true;

                this.League = newLeague;
                this.EventRow = this.League.EventTable.GetLast();
            }
        }

        private void NewLeague() {
            League league = new();
            EventRow eventRow = league.EventTable.AddRow("Default Event", DateTime.Today.ToString("yyyy-MM-dd"));
            eventRow.Settings["lanes"] = "8";
            eventRow.Settings["teams"] = "2";
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.PopulateMatches(8, 2);

            this.League = league;
            this.Title = $"Leagueinator ";
            SaveState.IsSaved = false;
        }

        private void HndSaveClick(object sender, RoutedEventArgs e) {
            if (this.League is null) return;
            if (SaveState.Filename.IsEmpty()) this.HndSaveAsClick(null, null);
            else this.League.WriteXml(SaveState.Filename);
        }

        private void HndSaveAsClick(object sender, RoutedEventArgs e) {
            if (this.League is null) return;
            SaveFileDialog dialog = new();
            dialog.Filter = "League Files (*.league)|*.league";

            if (dialog.ShowDialog() == true) {
                this.League.WriteXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;
                SaveState.IsSaved = true;
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
            public delegate void StateChangedHandler(bool IsSaved);
            public static event StateChangedHandler StateChanged = delegate { };

            private static bool _isSaved = false;
            private static string _filename = "";

            public static bool IsSaved { 
                get => _isSaved;
                set {
                    _isSaved = value;
                    StateChanged.Invoke(value);
                }
            }
            public static string Filename { get => _filename; set => _filename = value; }
        }
    }
}
