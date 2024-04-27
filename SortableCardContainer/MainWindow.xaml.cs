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

        private League _league;
        public League League { 
            get => this._league;
            set {
                this._league = value;
                this.EventRow = this.League.EventTable.GetLast();
            }
        }

        public MainWindow() {

            InitializeComponent();
            this.CardStackPanel.Add(new MatchCard());
            this.CardStackPanel.Add(new MatchCard());
            this.CardStackPanel.Add(new MatchCard());
            this.CardStackPanel.Add(new MatchCard());
            this.CardStackPanel.Add(new MatchCard());
            this.CardStackPanel.Add(new MatchCard());
            this.CardStackPanel.Add(new MatchCard());
            this.CardStackPanel.Add(new MatchCard());

            League league = new();
            league.EventTable.AddRow("Default Event", DateTime.Today.ToString("yyyy-MM-dd"));
            league.EventTable.GetRow(0).Rounds.Add();
            this.League = league;
        }

        private void HndNewClick(object sender, RoutedEventArgs e) { }
        private void HndLoadClick(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "League Files (*.league)|*.league";

            if (dialog.ShowDialog() == true) {
                League newLeague = new();
                newLeague.ReadXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;
                SaveState.IsSaved = true;

                this.League = newLeague;
                this.EventRow = this.League.EventTable.GetLast();
            }
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

        static class SaveState {
            static public bool IsSaved = false;
            static public string Filename = "";
        }
    }
}
