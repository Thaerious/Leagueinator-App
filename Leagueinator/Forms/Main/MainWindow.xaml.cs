using Leagueinator.Controls;
using Leagueinator.Controls.MatchCards;
using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Scoring;
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
                //TimeTrace.Report.WriteFiles("D:/scratch/web/leagueinator/");
                //OpenBrowser("D:/scratch/web/leagueinator/index.html");
            };

            this.InitializeComponent();
            this.League = NewLeague();

            SaveState.StateChanged += this.HndStateChanged;
            this._league.LeagueUpdate += this.HndLeagueUpdate;
            this.EventRow = this.League.EventTable.GetLast();

            SaveState.Filename = "";
            SaveState.ChangeState(this, false);

            this.AddHandler(MatchCard.RegisteredFormatChangedEvent, new FormatChangedEventHandler(this.HndMatchFormatChanged));
        }

        static void OpenBrowser(string url) {
            try {
                Process.Start(new ProcessStartInfo {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex) {
                Console.WriteLine("Could not open browser: " + ex.Message);
            }
        }
        private static League NewLeague() {
            League league = new();
            EventRow eventRow = league.Events.Add("Default Event", DateTime.Today.ToString("yyyy-MM-dd"));
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.PopulateMatches();
            league.LeagueSettingsTable.Set("ScoringFormat", ScoringFormat.POINTS_PLUS);
            return league;
        }

        private void ClearFocus() {
            FocusManager.SetFocusedElement(this, null);
        }

        /// <summary>
        /// Fill matchRow cards with values from "roundRow".
        /// Clears all matchRow cards that does not have a value in "roundRow".
        /// </summary>
        /// <param name="roundRow"></param>
        private void PopulateMatchCards(RoundRow roundRow) {
            if (this.EventRow is null) throw new NullReferenceException();

            // Remove all match cards from panel.
            while (this.MachCardStackPanel.Children.Count > 0) {
                var child = this.MachCardStackPanel.Children[0];
                this.MachCardStackPanel.Children.Remove(child);
            }

            List<MatchRow> matchList = [.. roundRow.Matches];
            matchList.Sort((m1, m2) => m1.Lane.CompareTo(m2.Lane));

            foreach (MatchRow matchRow in matchList) {
                MatchCard matchCard = MatchCardFactory.GenerateMatchCard(matchRow);
                this.MachCardStackPanel.Children.Add(matchCard);
            }
        }
    }
}
