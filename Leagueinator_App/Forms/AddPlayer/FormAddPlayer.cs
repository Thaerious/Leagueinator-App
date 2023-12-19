

namespace Leagueinator.App.Forms.AddPlayer {
    public class PlayerAddedArgs {
        readonly public string PlayerInfo;
        readonly public bool CurrentRoundOnly;

        public PlayerAddedArgs(string playerInfo, bool currentRoundOnly) {
            this.PlayerInfo = playerInfo;
            this.CurrentRoundOnly = currentRoundOnly;
        }
    }

    public partial class FormAddPlayer : Form {
        public delegate void NotifyPlayerAdded(FormAddPlayer source, PlayerAddedArgs args);
        public event NotifyPlayerAdded OnPlayerAdded = delegate { };

        public bool CurrentRoundOnly => this.chkCurrentRound.Checked;

        public FormAddPlayer() {
            this.InitializeComponent();
        }

        private void TxtName_KeyDown(object sender, KeyEventArgs e) {                   
            if (e.KeyCode == Keys.Escape) this.butOK.PerformClick();
            if (e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true;
                if (this.txtName.Text == "") this.butOK.PerformClick();
                if (OnPlayerAdded != null && this.txtName.Text != null && this.txtName.Text.Trim() != "") {
                    var args = new PlayerAddedArgs(this.txtName.Text, this.CurrentRoundOnly);
                    this.OnPlayerAdded(this, args);
                }
                this.txtName.Text = null;
            }
        }

        private void ButOK_Click(object sender, System.EventArgs e) {
            if (OnPlayerAdded != null) {
                if (OnPlayerAdded != null && this.txtName.Text != null && this.txtName.Text.Trim() != "") {
                    var args = new PlayerAddedArgs(this.txtName.Text, this.CurrentRoundOnly);
                    this.OnPlayerAdded(this, args);
                }
                this.txtName.Text = null;
            }
        }
    }
}
