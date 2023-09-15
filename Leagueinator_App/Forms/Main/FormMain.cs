
using Leagueinator.App.Forms.AddEvent;
using Leagueinator.App.Forms.AddPlayer;
using Leagueinator.Model;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

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
                }
                else {
                    this.saveToolStripMenuItem.Enabled = true;
                    this.saveAsToolStripMenuItem.Enabled = true;
                    this.printToolStripMenuItem.Enabled = true;
                    this.closeToolStripMenuItem.Enabled = true;
                    this.eventsToolStripMenuItem.Enabled = true;
                }
            }
        }

        private string filename = "";


        public FormMain() {
            InitializeComponent();
            this.eventPanel.OnAddRound += (s, args) => {
                this.eventPanel.LeagueEvent.NewRound();
            };

            this.eventPanel.PlayerListBox.OnDelete += this.PlayerListBox_OnDelete;
            this.eventPanel.PlayerListBox.OnRename += this.PlayerListBox_OnRename;
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
            //try {
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    using (FileStream stream = new FileStream(filename, FileMode.Open)) {
            //        this.League = (League)formatter.Deserialize(stream);
            //    }
            //    this.filename = filename;
            //    IsSaved.Singleton.Value = true;
            //}
            //catch (Exception ex) {
            //    MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    Debug.WriteLine(ex.Message);
            //    Debug.WriteLine(ex.StackTrace);
            //}
        }

        private void SaveAs(string filename) {
            using FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
            JsonSerializer.Serialize(stream, this.League);
            this.filename = filename;
            IsSaved.Singleton.Value = true;
        }

        private void File_Load(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private void File_New(object sender, EventArgs e) {
            this.League = new League();
        }

        private void File_Close(object sender, EventArgs e) {
            this.Text = "Leagueinator";
            this.League = null;
        }

        private void File_Exit(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        //private void File_Load(object sender, EventArgs e) {
        //    using OpenFileDialog dialog = new OpenFileDialog();
        //    SetupFileDialog(dialog);

        //    if (dialog.ShowDialog() == DialogResult.OK) {
        //        this.LoadFile(dialog.FileName);
        //    }
        //}

        private void File_SaveAs(object sender, EventArgs e) {
            using SaveFileDialog dialog = new SaveFileDialog();
            SetupFileDialog(dialog);

            if (dialog.ShowDialog() == DialogResult.OK) {
                this.SaveAs(dialog.FileName);
            }
        }

        private void File_Save(object sender, EventArgs e) {
            throw new NotImplementedException();
            //    if (this.filename.IsEmpty()) {
            //        this.File_SaveAs(sender, e);
            //    }
            //    else {
            //        this.SaveAs(this.filename);
            //    }
        }

        private void File_Print(object sender, EventArgs e) {
            throw new NotImplementedException();
            //    //var round = this.editEventPanel.CurrentRound;
            //    //if (round == null) return;
            //    ////ScoreCardPrinter.Print(round);

            //    //int currentRoundIndex = this.editEventPanel.LeagueEvent.Rounds.IndexOf(this.editEventPanel.CurrentRound);
            //    //var mcp = new MatchCardPrinter(round, currentRoundIndex);

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

            IsSaved.Singleton.Value = false;
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
            if (args.CurrentRoundOnly) {
                this.eventPanel.CurrentRound.IdlePlayers.Add(args.PlayerInfo);
            }
            else {
                foreach (Round round in this.eventPanel.LeagueEvent.Rounds) {
                    round.IdlePlayers.Add(args.PlayerInfo);
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
            Debug.WriteLine(this.eventPanel.LeagueEvent.ToString());
        }
    }
}
