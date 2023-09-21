
using Leagueinator.App.Forms.AddEvent;
using Leagueinator.App.Forms.AddPlayer;
using Leagueinator.App.Forms.Report;
using Leagueinator.App.Forms.SelectEvent;
using Leagueinator.Model;
using Leagueinator.Utility;
using Leagueinator.Utility.Seek;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

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
                    this.eventPanel.Visible = true;

                    if (value.Events.Count > 0) {
                        this.eventPanel.LeagueEvent = value.Events.Last();
                    }
                    else {
                        this.eventPanel.LeagueEvent = null;
                        this.eventPanel.Visible = false;
                    }
                }
            }
        }

        private string filename = "";


        public FormMain() {
            InitializeComponent();
            this.eventPanel.OnAddRound += (s) => {
                if (this.eventPanel.LeagueEvent is null) return;
                this.eventPanel.LeagueEvent.NewRound();
            };

            this.eventPanel.OnDeleteRound += (s) => {
                if (this.eventPanel is null) return;
                if (this.eventPanel.LeagueEvent is null) return;
                if (this.eventPanel.CurrentRound is null) return;

                this.eventPanel.LeagueEvent.Rounds.Remove(this.eventPanel.CurrentRound);
            };

            this.eventPanel.PlayerListBox.OnDelete += this.PlayerListBox_OnDelete;
            this.eventPanel.PlayerListBox.OnRename += this.PlayerListBox_OnRename;

            LeagueSingleton.ModelUpdate += (src, args) => {
                IsSaved.Value = false;
            };

            IsSaved.Update += (value) => {
                if (!value) this.Text = this.filename + " *";
                else this.Text = this.filename;
            };
        }


        private void PlayerListBox_OnRename(Components.PlayerListBox.PlayerListBox source, Components.PlayerListBox.PlayerListBoxArgs args) {
            throw new NotImplementedException();
        }

        private void PlayerListBox_OnDelete(Components.PlayerListBox.PlayerListBox source, Components.PlayerListBox.PlayerListBoxArgs args) {
            Round round = this.eventPanel.CurrentRound;
            foreach (Team team in round.SeekDeep<Team>()) {
                if (team.Players.Contains(args.PlayerInfo)) {
                    team.RemovePlayer(args.PlayerInfo);
                }
            }

            round.IdlePlayers.Remove(args.PlayerInfo);
        }

        private static void SetupFileDialog(FileDialog dialog) {
            //dialog.InitialDirectory = Properties.Settings.Default.save_dir; TODO
            dialog.Filter = "league files (*.league)|*.league|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
        }

        private void LoadFile(string filename) {
            try {
                using (StreamReader inputFile = new StreamReader(filename)) {
                    string leagueJson = inputFile.ReadToEnd();
                    this.League = JsonConvert.DeserializeObject<League>(leagueJson) as League;
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
            //    //var round = this.eventPanel.CurrentRound;
            //    //if (round == null) return;
            //    ////ScoreCardPrinter.Print(round);

            //    //int _currentRoundIndex = this.eventPanel.LeagueEvent.Rounds.IndexOf(this.eventPanel.CurrentRound);
            //    //var mcp = new MatchCardPrinter(round, _currentRoundIndex);

            //    //if (this.printDialog.ShowDialog() == DialogResult.OK) {
            //    //    this.printDocument.PrintPage += mcp.HndPrint;
            //    //    this.printDocument.Print();
            //    //}
        }

        private void Events_AddEvent(object sender, EventArgs e) {
            if (this.League is null) return;

            var childForm = new FormAddEvent();
            if (childForm.ShowDialog() == DialogResult.Cancel) return;

            var lEvent = this.League.AddEvent(
                childForm.EventName,
                childForm.Date,
                childForm.Settings
            );

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
            if (this.League is null) return;

            FormSelectEvent childForm = new FormSelectEvent();
            childForm.SetEvents(this.League.Events);

            DialogResult result = childForm.ShowDialog();
            if (result == DialogResult.Cancel) return;

            if (childForm.Action == "Select") {
                LeagueEvent lEvent = childForm.LeagueEvent;
                this.eventPanel.LeagueEvent = lEvent;
                this.eventPanel.Visible = true;
            }
            else if (childForm.Action == "Delete") {
                this.League.Events.Remove(childForm.LeagueEvent);
                if (this.eventPanel.LeagueEvent == childForm.LeagueEvent) {
                    this.eventPanel.Visible = false;
                    this.eventPanel.LeagueEvent = null;
                }
            }
        }

        private void Help_About(object sender, EventArgs e) {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
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
                Debug.WriteLine(this.League.ToString());
            }
        }

        private void Dev_IsSaved(object sender, EventArgs e) {
            Debug.WriteLine(IsSaved.Value);
        }

        private void Dev_HashCode(object sender, EventArgs e) {
            Debug.WriteLine("Rounds collection for current event");
            Debug.WriteLine($"Hash Code {this.eventPanel.LeagueEvent.Rounds.GetHashCode().ToString("X")}");
        }

        private FormReport InitFormReport() {
            FormReport form = new FormReport();

            form.InitColumns(
                new string[] {
                    "Name", "Round", "Rank", "Bowls", "Ends",
                    "Wins", "Losses", "Ties",
                    "PointsFor", "PlusFor", "PointsAgainst", "PlusAgainst"
                },
                new string[] {
                    "Name", "Round", "Rank", "Bowls", "Ends",
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

        private void View_Summary(object sender, EventArgs e) {
            var form = this.InitFormReport();

            LeagueEvent? lEvent = this.eventPanel.LeagueEvent;
            if (lEvent == null) return;

            lEvent.Players.ForEach(p => {
                form.AddRow(new EventSummary(lEvent, p));
            });

            foreach (Round round in lEvent.Rounds) {
                round.ActivePlayers.ForEach(p => {
                    form.AddRow(new RoundDatum(lEvent, round, p));
                });
            };

            form.ShowDialog(this);
        }

        private void View_RoundSummary(object sender, EventArgs e) {
            var form = this.InitFormReport();

            LeagueEvent? lEvent = this.eventPanel.LeagueEvent;
            if (lEvent == null) return;

            Round? round = this.eventPanel.CurrentRound;
            if (round == null) return;

            round.ActivePlayers.ForEach(player => {
                form.AddRow(new RoundDatum(lEvent, round, player));
            });

            form.ShowDialog(this);
        }

        private void View_EventSummary(object sender, EventArgs e) {
            var form = this.InitFormReport();

            LeagueEvent? lEvent = this.eventPanel.LeagueEvent;
            if (lEvent == null) return;

            lEvent.Players.ForEach(p => {
                form.AddRow(new EventSummary(lEvent, p));
            });

            form.ShowDialog(this);

        }

        private void Players_Copy(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;
            if (this.eventPanel.CurrentRound == this.eventPanel.LeagueEvent.Rounds.First()) return;

            this.Players_Clear(sender, e);

            Round? current = this.eventPanel.CurrentRound;
            Round previous = this.eventPanel.LeagueEvent.Rounds.Prev(current);

            foreach (PlayerInfo pInfo in previous.IdlePlayers) {
                current.IdlePlayers.Add(pInfo);
            }

            for (int m = 0; m < current.Matches.Count; m++) {
                Match? match = current.Matches[m];
                if (match is null) continue;
                for (int t = 0; t < match.Teams.Count; t++) {
                    Team? team = match.Teams[t];
                    if (team is null) continue;
                    for (int p = 0; p < team.Players.MaxSize; p++) {
                        team.Players[p] = previous.Matches[m]?.Teams[t]?.Players[p];
                    }
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
                    for (int p = 0; p < team.Players.MaxSize; p++) {
                        if (current.IdlePlayers.Count == 0) return;
                        if (team.Players[p] is not null) continue;
                        int r = random.Next(current.IdlePlayers.Count);
                        team.Players[p] = current.IdlePlayers[r];
                        current.IdlePlayers.RemoveAt(r);
                    }
                }
            }
        }

        private void Players_Clear(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;

            Round current = this.eventPanel.CurrentRound;
            current.IdlePlayers.Clear();
            foreach (Team team in current.Teams) { team.Clear(); }
        }

        private void Players_Reset(object sender, EventArgs e) {
            if (this.eventPanel.LeagueEvent == null) return;
            if (this.eventPanel.CurrentRound == null) return;

            Round current = this.eventPanel.CurrentRound;
            foreach (Team team in current.Teams) {
                foreach (PlayerInfo pInfo in team.Players.Values.NotNull()) {
                    current.IdlePlayers.Add(pInfo);
                }
                team.Clear();
            }
        }

        private void File_Print_Preview(object sender, EventArgs e) {
            var round = this.eventPanel.CurrentRound;
            if (round == null) return;
            //ScoreCardPrinter.Print(round);

            int currentRoundIndex = this.eventPanel.LeagueEvent.Rounds.IndexOf(this.eventPanel.CurrentRound);
            var mcp = new MatchCardPrinter(round, currentRoundIndex);
            this.printDocument.PrintPage += mcp.HndPrint;

            //this.printPreview.Document = this.printDocument;
            //this.printPreview.ShowDialog();
        }
    }
}
