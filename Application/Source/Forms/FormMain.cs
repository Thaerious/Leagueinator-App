using Leagueinator.Forms.ResultsPlus;
using Leagueinator.Model;
using Leagueinator.Utility;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Leagueinator.Forms {
    public partial class FormMain : Form {
        private League? League = null;
        private readonly SaveState SaveState = new();

        public FormMain() {
            this.InitializeComponent();
            this.League = new MockModel();
            this.eventPanel.EventRow = this.League.EventTable.GetRow(0);
        }

        private void HndMenuViewTeams(object sender, EventArgs e) {
            new FormViewTable().Show("Teams", this.League.TeamTable);
        }

        private void HndMenuViewPlayers(object sender, EventArgs e) {
            new FormViewTable().Show("Players", this.League.PlayerTable);
        }

        private void HndMenuViewMembers(object sender, EventArgs e) {
            new FormViewTable().Show("Members", this.League.MemberTable);
        }

        private void HndMenuViewEvents(object sender, EventArgs e) {
            new FormViewTable().Show("Events", this.League.EventTable);
        }

        private void HndMenuViewRounds(object sender, EventArgs e) {
            new FormViewTable().Show("Rounds", this.League.RoundTable);
        }

        private void HndMenuViewMatches(object sender, EventArgs e) {
            new FormViewTable().Show("Matches", this.League.MatchTable);
        }

        private void HndMenuViewIdle(object sender, EventArgs e) {
            new FormViewTable().Show("Idle Players", this.League.IdleTable);
        }

        private void HndMenuViewSettings(object sender, EventArgs e) {
            new FormViewTable().Show("Settings", this.League.SettingsTable);
        }

        private void HndMenuViewEventResults(object sender, EventArgs e) {
            if (this.eventPanel.EventRow is null) throw new ArgumentNullException("event row not set");

            ResultsPlusForm form = new(this.eventPanel.EventRow) {
                StartPosition = FormStartPosition.Manual
            };

            form.Location = new Point(
                this.Bounds.Center(form.Bounds).X,
                this.Location.Y
            );

            form.Show();
        }

        private void HndMenuNew(object sender, EventArgs e) {
            
        }

        private void HndMenuLoad(object sender, EventArgs e) {
            using OpenFileDialog dialog = new();
            dialog.Filter = "League Files (*.league)|*.league";

            if (dialog.ShowDialog() == DialogResult.OK) {
                League newLeague = new();
                newLeague.ReadXml(dialog.FileName);
                this.SaveState.Filename = dialog.FileName;
                this.SaveState.IsSaved = true;

                this.League = newLeague;
                this.eventPanel.EventRow = this.League.EventTable.GetLast();
            }
        }

        private void HndMenuSave(object sender, EventArgs e) {
            if (this.League is null) return;
            if (this.SaveState.Filename.IsEmpty()) this.HndMenuSaveAs(null, null);
            else this.League.WriteXml(this.SaveState.Filename);
        }

        private void HndMenuSaveAs(object? sender, EventArgs? e) {
            if (this.League is null) return;
            using SaveFileDialog dialog = new();
            dialog.Filter = "League Files (*.league)|*.league";

            if (dialog.ShowDialog() == DialogResult.OK) {
                this.League.WriteXml(dialog.FileName);
                this.SaveState.Filename = dialog.FileName;
                this.SaveState.IsSaved = true;
            }
        }

        private void HndMenuClose(object sender, EventArgs e) {
            this.League = null;
            this.SaveState.IsSaved = false;
        }

        private void HndMenuExit(object sender, EventArgs e) {
            this.Close();
        }
    }


    class SaveState {
        public bool IsSaved = false;
        public string Filename = "";
    }
}
