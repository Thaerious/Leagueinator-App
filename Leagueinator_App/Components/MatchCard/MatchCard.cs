using Leagueinator.Components;

using Leagueinator.Utility.ObservableDiscreteCollection;
using Model;

namespace Leagueinator.App.Components {
    public partial class MatchCard : UserControl {

        public Match? Match { get; set; }


        public MatchCard() {
            this.InitializeComponent();
        }

        public void Reposition() {
            this.MaximumSize = new Size(0, 0);
            this.MinimumSize = new Size(0, 0);

            this.Height = Math.Max(this.flowTeam0.Height, this.flowTeam0.Height);
            this.Height = this.Height + 20;

            this.flowTeam0.Top = (this.Height / 2) - (this.flowTeam0.Height / 2);
            this.flowTeam1.Top = (this.Height / 2) - (this.flowTeam0.Height / 2);
            this.txtScore0.Top = (this.Height / 2) - (this.txtScore0.Height / 2);
            this.txtScore1.Top = (this.Height / 2) - (this.txtScore1.Height / 2);
            this.labelLane.Top = (this.Height / 2) - (this.labelLane.Height / 2);
        }

        public Label AddLabel(int teamIndex, String text) {
            FlowLayoutPanel flowPanel = teamIndex == 0 ? this.flowTeam0 : this.flowTeam1;

            var label = new Label {
                BorderStyle = BorderStyle.FixedSingle,
                Font = this.Font,
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 35,
                Width = 175,
                BackColor = Color.WhiteSmoke,
                AllowDrop = true,
            };

            flowPanel.Controls.Add(label);

            new ControlDragHandlers<string>(label,
                () => { // get data from source
                    Team? team = this.Match?.Teams[teamIndex];
                    if (team == null) return null;

                    int index = flowPanel.Controls.IndexOf(label);
                    return team.Players[index];
                },
                (pi, src) => { // called when this is the destination
                    Team? team = this.Match?.Teams[teamIndex];
                    if (team == null) return null;

                    int index = flowPanel.Controls.IndexOf(label);
                    var response = team.Players[index];
                    team.Players[index] = pi;
                    return response;
                },
                (pi, dest) => { // response to source
                    Team? team = this.Match?.Teams[teamIndex];
                    if (team == null) return;

                    int index = flowPanel.Controls.IndexOf(label);
                    var response = team.Players[index];
                    team.Players[index] = pi;
                }
            );

            return label;
        }

        public void ClearLabels() {
            this.flowTeam0.Controls.Clear();
            this.flowTeam1.Controls.Clear();
        }

        public int Lane {
            get => int.Parse(this.labelLane.Text);
            set => this.labelLane.Text = value.ToString();
        }

        private Match? _match;

        private void OnScore0Changed(object sender, EventArgs e) {
            this.OnScoreChanged((TextBox)sender, this.Match?.Teams[0]);
        }

        private void OnScore1Changed(object sender, EventArgs e) {
            this.OnScoreChanged((TextBox)sender, this.Match?.Teams[1]);
        }

        private void OnScoreChanged(TextBox textBox, Team? team) {

            if (this.Match is null) return;
            if (team is null) return;
            var text = textBox.Text;            

            try {
                int score = int.Parse(text);
                team.Bowls = score;
            }
            catch {
                textBox.Text = team.Bowls.ToString();
            }

        }
    }
}
