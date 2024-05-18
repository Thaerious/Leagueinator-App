using Leagueinator.Model.Tables;
using Leagueinator.Model;
using System.Windows;

namespace MatchCard {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            League league = new();
            EventRow eventRow = league.EventTable.AddRow("Default Event", DateTime.Today.ToString("yyyy-MM-dd"));
            eventRow.Settings.Add("format", "assigned_ladder");
            eventRow.Settings.Add("lanes", "8");
            eventRow.Settings.Add("teams", "2");
            eventRow.Settings.Add("ends", "10");

            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.PopulateMatches();
            this.League = league;

            this.League.PlayerTable.AddRow("Adam");
            this.League.PlayerTable.AddRow("Eve");
            this.League.PlayerTable.AddRow("Cain");
            this.League.PlayerTable.AddRow("Able");
            this.League.PlayerTable.AddRow("Bart");
            this.League.PlayerTable.AddRow("Lisa");
            this.League.PlayerTable.AddRow("Marge");
            this.League.PlayerTable.AddRow("Homer");
            roundRow.Matches[0].Teams[0].Members.Add("Adam");
            roundRow.Matches[0].Teams[0].Members.Add("Eve");
            roundRow.Matches[0].Teams[1].Members.Add("Cain");
            roundRow.Matches[0].Teams[1].Members.Add("Able");

            roundRow.Matches[1].Teams[0].Members.Add("Bart");
            roundRow.Matches[1].Teams[0].Members.Add("Lisa");
            roundRow.Matches[1].Teams[1].Members.Add("Marge");
            roundRow.Matches[1].Teams[1].Members.Add("Homer");

            this.Card0.MatchRow = roundRow.Matches[0];
            this.Card1.MatchRow = roundRow.Matches[1];
        }

        public League League { get; }
    }
}
