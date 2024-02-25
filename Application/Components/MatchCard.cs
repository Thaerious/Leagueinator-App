using Leagueinator.Utility;
using Model.Tables;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Leagueinator.Components {
    public partial class MatchCard : UserControl {

        [Category("Custom Properties")]
        [Description("Defines the lane index for this control.")]
        public int Lane { get; set; } = -1;

        [Category("Custom Properties")]
        [Description("Model Controller.")]
        public Controller? Controller {
            get => _controller;
            set {
                if (_controller == value) return;
                _controller = value;
                if (value == null) return;

                value.OnUpdateTeam += this.OnUpdateTeam;
                value.OnUpdateMatch += this.Value_OnUpdateMatch;
            }
        }

        private void Value_OnUpdateMatch(MatchRow matchRow) {
            if (matchRow.Lane != Lane) return;
            this.membersGrid.DataSource = matchRow.Members;
            this.teamsGrid.DataSource = matchRow.Teams;

            this.teamsGrid.Columns[TeamTable.COL.MATCH].Visible = false;
            this.teamsGrid.Columns[TeamTable.COL.UID].HeaderText = "team";
        }

        private void OnUpdateTeam(TeamRow teamRow) {
            if (teamRow.Match.Lane != Lane) return;
            this.txtEnds.Text = teamRow.Match.Ends.ToString();
        }

        public MatchCard() {
            InitializeComponent();
        }

        private Controller? _controller;

        private void LayoutEventHandler(object sender, LayoutEventArgs e) {
        }
    }
}
