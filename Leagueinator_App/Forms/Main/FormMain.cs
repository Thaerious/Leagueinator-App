using Leagueinator.App.Forms.AddEvent;
using Leagueinator.App.Forms.AddPlayer;
using Leagueinator.App.Forms.RenamePlayer;
using Leagueinator.App.Forms.Report;
using Leagueinator.App.Forms.SelectEvent;
using Leagueinator.Utility;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using Model;
using Leagueinator_App;
using Leagueinator.App.Components;
using Newtonsoft.Json.Linq;

namespace Leagueinator.App.Forms.Main {
    public partial class FormMain : Form {
        private League? League {
            get => _league;
            set {
                _league = value;

                if (value == null) {
                    this.saveToolStripMenuItem.Enabled = false;
                    this.saveAsToolStripMenuItem.Enabled = false;
                    this.printToolStripMenuItem.Enabled = false;
                    this.closeToolStripMenuItem.Enabled = false;
                    this.eventsToolStripMenuItem.Enabled = false;
                    this.playersToolStripMenuItem.Enabled = false;
                    this.eventPanel.Visible = false;
                }
                else {
                    this.saveToolStripMenuItem.Enabled = true;
                    this.saveAsToolStripMenuItem.Enabled = true;
                    this.printToolStripMenuItem.Enabled = true;
                    this.closeToolStripMenuItem.Enabled = true;
                    this.eventsToolStripMenuItem.Enabled = true;
                    this.playersToolStripMenuItem.Enabled = true;
                    this.eventPanel.Visible = false;

                    this.OnSetLeague(value);
                }
            }
        }

        private string filename = "";

        public FormMain() {
            InitializeComponent();

            LeagueSingleton.ModelUpdate += (src, args) => {
                IsSaved.Value = false;
            };

            IsSaved.Update += (value) => {
                if (!value) this.Text = this.filename + " *";
                else this.Text = this.filename;
            };
        }

        private void OnSetLeague(League league) {
            if (league.LeagueEvents.Count > 0) {
                var lEvent = league.LeagueEvents.Last();
                this.eventPanel.Visible = true;
                this.eventPanel.LeagueEvent = lEvent;
            }
        }

        private void DoRenamePlayer(string before, string after) {
            throw new NotImplementedException();
        }

        private void PlayerListBox_OnRename(PlayerListBox source, PlayerListBoxArgs args) {
            if (this.League is null) return;
            if (this.eventPanel.LeagueEvent is null) return;

            var form = new FormRenamePlayer {
                StartPosition = FormStartPosition.CenterParent
            };
            var result = form.ShowDialog();
            if (result == DialogResult.Cancel) return;

            DoRenamePlayer(args.PlayerName, form.PlayerName);
        }

        private void PlayerListBox_OnDelete(PlayerListBox source, PlayerListBoxArgs args) {
            Round? round = this.eventPanel.CurrentRound;
            if (round is null) return;
            round.DeletePlayer(args.PlayerName);
        }

        private static void SetupFileDialog(FileDialog dialog) {
            //dialog.InitialDirectory = Properties.EventSettings.Default.save_dir; TODO
            dialog.Filter = "league files (*.league)|*.league|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
        }

        private void LoadFile(string filename) {
            try {
                using (StreamReader inputFile = new StreamReader(filename)) {
                    string leagueJson = inputFile.ReadToEnd();
                    this.League = JsonConvert.DeserializeObject<League>(leagueJson);
                }
                this.filename = filename;
                this.Text = filename;
                IsSaved.Value = true;
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAs(string filename) {
            try {
                var leagueJson = JsonConvert.SerializeObject(this.League);
                using (StreamWriter outputFile = new StreamWriter(filename)) {
                    outputFile.Write(leagueJson);
                }
                this.filename = filename;
                IsSaved.Value = true;
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void File_Load(object sender, EventArgs e) {
            using OpenFileDialog dialog = new OpenFileDialog();
            SetupFileDialog(dialog);

            if (dialog.ShowDialog() == DialogResult.OK) {
                this.LoadFile(dialog.FileName);
            }
        }

        private void File_New(object sender, EventArgs e) {
            this.League = new League();
            this.Text = "Leagueinator *";
        }

        private void File_Close(object sender, EventArgs e) {
            this.Text = "Leagueinator";
            this.League = null;
        }

        private void File_Exit(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private void File_SaveAs(object sender, EventArgs e) {
            using SaveFileDialog dialog = new SaveFileDialog();
            SetupFileDialog(dialog);

            if (dialog.ShowDialog() == DialogResult.OK) {
                this.SaveAs(dialog.FileName);
            }
        }

        private void File_Save(object sender, EventArgs e) {
            if (this.filename.IsEmpty()) {
                this.File_SaveAs(sender, e);
            }
            else {
                this.SaveAs(this.filename);
            }
        }

        private void File_Print(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private void Events_AddEvent(object sender, EventArgs e) {
            if (this.League is null) throw new AppStateException("League is Null");

            var formAddEvent = new FormAddEvent();
            if (formAddEvent.ShowDialog() == DialogResult.Cancel) return;

            var lEvent = this.League.NewLeagueEvent(
                formAddEvent.EventName,
                formAddEvent.EventSettings.Date
            );

            lEvent.Settings["team_size"] = formAddEvent.EventSettings.TeamSize.ToString();
            lEvent.Settings["lane_count"] = formAddEvent.EventSettings.LaneCount.ToString();

            IsSaved.Value = false;
            this.eventPanel.Visible = true;
            this.eventPanel.LeagueEvent = lEvent;
        }

        private void Players_Add(object sender, EventArgs e) {
            var form = new FormAddPlayer();
            form.OnPlayerAdded += this.Form_OnPlayerAdded;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private void Form_OnPlayerAdded(FormAddPlayer source, PlayerAddedArgs args) {
            if (this.eventPanel.LeagueEvent is null) return;
            if (this.eventPanel.CurrentRound is null) return;

            if (args.CurrentRoundOnly) {
                var list = this.eventPanel.CurrentRound.IdlePlayers;
                if (!list.Contains(args.PlayerInfo)) list.Add(args.PlayerInfo);
            }
            else {
                foreach (Round round in this.eventPanel.LeagueEvent.Rounds) {
                    var list = round.IdlePlayers;
                    if (!list.Contains(args.PlayerInfo)) list.Add(args.PlayerInfo);
                }
            }
        }

        private void Events_SelectEvent(object sender, EventArgs e) {
            if (this.League is null) throw new AppStateException();

            FormSelectEvent formSelectEvent = new FormSelectEvent(this.League.LeagueEvents);

            DialogResult result = formSelectEvent.ShowDialog();
            if (result == DialogResult.Cancel) return;
            if (formSelectEvent.LeagueEvent is null) return;

            if (formSelectEvent.Action == "Select") {
                LeagueEvent lEvent = formSelectEvent.LeagueEvent;
                this.eventPanel.LeagueEvent = lEvent;
                this.eventPanel.Visible = true;
            }
            else if (formSelectEvent.Action == "Delete") {
                this.League.LeagueEvents.Remove(formSelectEvent.LeagueEvent);
                if (this.eventPanel.LeagueEvent == formSelectEvent.LeagueEvent) {
                    this.eventPanel.Visible = false;
                    this.eventPanel.LeagueEvent = null;
                }
            }
        }

        private void Help_About(object sender, EventArgs e) {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Version version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new NullReferenceException("version");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
            string msg = $"Version\n{version}\n({buildDate})";
            MessageBox.Show(msg, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private League? _league;

        private void Dev_PrintCurrentEvent(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) {
                Debug.WriteLine("League Event is [NULL]");
            }
            else {
                Debug.WriteLine(this.eventPanel.LeagueEvent.ToString());
            }
        }

        private void Dev_PrintLeague(object sender, EventArgs e) {
            if (this.League == null) {
                Debug.WriteLine("League is [NULL]");
            }
            else {
                Debug.WriteLine(this.League.PrettyPrint());
            }
        }

        private void Dev_IsSaved(object sender, EventArgs e) {
            Debug.WriteLine(IsSaved.Value);
        }

        private void Dev_HashCode(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent is null) throw new AppStateException();
            Debug.WriteLine("Rounds collection for current event");
            Debug.WriteLine(message: $"Hash Code {this.eventPanel.LeagueEvent.Rounds.GetHashCode().ToString():X}");
        }

        private FormReport InitFormReport(FormReport.RowGenerator generator) {
            FormReport form = new FormReport(generator);

            form.InitColumns(
                new string[] {
                    "TagName", "Round", "Rank", "Bowls", "Ends",
                    "Wins", "Losses", "Ties",
                    "PointsFor", "PlusFor", "PointsAgainst", "PlusAgainst"
                },
                new string[] {
                    "TagName", "Round", "Rank", "Bowls", "Ends",
                    "W", "L", "T",
                    "F", "F+", "A", "A+"
                }
                ,
                new int[] {
                    100, 50, 50, 50, 50,
                    40, 40, 40,
                    40, 40, 40, 40
                }
            );

            form.StartPosition = FormStartPosition.CenterParent;

            return form;
        }

        private void View_Report(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private void View_RoundSummary(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private void View_EventSummary(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private void Players_Copy(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;
            if (this.eventPanel.CurrentRound == this.eventPanel.LeagueEvent.Rounds.First()) return;

            this.Players_Clear(sender, e);

            Round? current = this.eventPanel.CurrentRound;
            Round? previous = this.eventPanel.LeagueEvent.Rounds.Prev(current);
            if (previous == null) return;

            foreach (string playerName in previous.IdlePlayers) {
                current.IdlePlayers.Add(playerName);
            }

            for (int m = 0; m < current.Matches.Count; m++) {
                Match? match = current.Matches[m];
                if (match is null) continue;
                for (int t = 0; t < match.Teams.Count; t++) {
                    Team? team = match.Teams[t];
                    if (team is null) continue;
                    throw new NotImplementedException();
                    //for (int p = 0; p < team.Players.MaxSize; p++) {
                        //team.Players[p] = previous.Matches[m]?.Teams[t]?.Players[p];
                    //}
                }
            }
        }

        private void Players_Randomize(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;

            Random random = new Random();
            Round current = this.eventPanel.CurrentRound;

            for (int m = 0; m < current.Matches.Count; m++) {
                Match? match = current.Matches[m];
                if (match is null) continue;
                for (int t = 0; t < match.Teams.Count; t++) {
                    Team? team = match.Teams[t];
                    if (team is null) continue;
                    throw new NotImplementedException();
                    //for (int p = 0; p < team.Players.MaxSize; p++) {
                    //    if (current.IdlePlayers.Count == 0) return;
                    //    if (team.Players[p] is not null) continue;
                    //    int r = random.Next(current.IdlePlayers.Count);
                    //    team.Players[p] = current.IdlePlayers[r];
                    //    current.IdlePlayers.RemoveAt(r);
                    //}
                }
            }
        }

        private void Players_Clear(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;

            Round current = this.eventPanel.CurrentRound;
            throw new NotImplementedException();
            //current.IdlePlayers.Clear();
            //foreach (Team team in current.Teams) { team.Clear(); }
        }

        private void Players_Reset(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;

            Round current = this.eventPanel.CurrentRound;
            throw new NotImplementedException();
            //foreach (Team team in current.Teams) {
            //    foreach (PlayerInfo pInfo in team.Players.Values.NotNull()) {
            //        current.IdlePlayers.Add(pInfo);
            //    }
            //    team.Clear();
            //}
        }

        private void File_Print_Preview(object sender, EventArgs e) {
            var lEvent = this.eventPanel.LeagueEvent;
            if (lEvent == null) return;
            var round = this.eventPanel.CurrentRound;
            if (round == null) return;

            int currentRoundIndex = lEvent.Rounds.IndexOf(round);
            throw new NotImplementedException();
            //var mcp = new MatchCardPrinter(lEvent, round, currentRoundIndex);
            //this.printDocument.PrintPage += mcp.HndPrint;

            //this.printPreviewDialog.Document = this.printDocument;
            //this.printPreviewDialog.ShowDialog();
        }

        private void File_Print_Card(object sender, EventArgs e) {
            var lEvent = this.eventPanel.LeagueEvent;
            if (lEvent == null) return;
            var round = this.eventPanel.CurrentRound;
            if (round == null) return;


            int currentRoundIndex = lEvent.Rounds.IndexOf(round);
            throw new NotImplementedException();
            //var mcp = new MatchCardPrinter(lEvent, round, currentRoundIndex);

            //if (this.printDialog.ShowDialog() == DialogResult.OK) {
            //    this.printDocument.PrintPage += mcp.HndPrint;
            //    this.printDocument.Print();
            //}
        }

        private void Players_Scramble(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;
            if (this.eventPanel.CurrentRound == this.eventPanel.LeagueEvent.Rounds.First()) return;

            LeagueEvent lEvent = this.eventPanel.LeagueEvent;
            Round target = this.eventPanel.CurrentRound;
            Round first = lEvent.Rounds.First();

            throw new NotImplementedException();
            //Scramble scramble = new Scramble(first, target);
            //scramble.DoScramble(lEvent.Rounds.IndexOf(target));
        }

        private void Players_AssignLanes(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;

            if (this.eventPanel.LeagueEvent.Rounds.Count < 2) {
                MessageBox.Show("Can not assign lanes with only one round.");
                return;
            }

            LeagueEvent lEvent = this.eventPanel.LeagueEvent;
            Round target = this.eventPanel.CurrentRound;

            throw new NotImplementedException();
            //var laneSolution = new LaneSolution(lEvent, target);
            //var algo = new GreedyWalk();

            //var bestSolution = algo.Run(laneSolution, _s => {
            //    LaneSolution? solution = _s as LaneSolution;
            //    if (solution is null) return;
            //});

            //for (int i = 0; i < bestSolution.Count(); i++) {
            //    target.Matches[i].CopyFrom(bestSolution[i]);
            //}
        }

        private void File_Print_Standings(object sender, EventArgs e) {
            var lEvent = this.eventPanel.LeagueEvent;
            if (lEvent == null) return;

            throw new NotImplementedException();
            //var printer = new StandingsPrinter(lEvent);

            //this.printDocument.DefaultPageSettings.Landscape = true;
            //this.printDocument.PrintPage += printer.HndPrint;
            //this.printPreviewDialog.Document = this.printDocument;
            //this.printPreviewDialog.ShowDialog();
        }
    }
}
