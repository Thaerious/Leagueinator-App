
using Leagueinator.App.Forms.AddEvent;
using Leagueinator.App.Forms.AddPlayer;
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
                    this.eventPanel.Visible = false;
                }
                else {
                    Debug.WriteLine($"setting league {value.Events.Count}");
                    this.saveToolStripMenuItem.Enabled = true;
                    this.saveAsToolStripMenuItem.Enabled = true;
                    this.printToolStripMenuItem.Enabled = true;
                    this.closeToolStripMenuItem.Enabled = true;
                    this.eventsToolStripMenuItem.Enabled = true;
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
            //    //var round = this.editEventPanel.CurrentRound;
            //    //if (round == null) return;
            //    ////ScoreCardPrinter.Print(round);

            //    //int _currentRoundIndex = this.editEventPanel.LeagueEvent.Rounds.IndexOf(this.editEventPanel.CurrentRound);
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

        private void Events_AddPlayers(object sender, EventArgs e) {
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
    }
}
