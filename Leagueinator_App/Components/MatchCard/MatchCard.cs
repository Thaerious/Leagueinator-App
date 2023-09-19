using Leagueinator.Components;
using Leagueinator.Model;
using Leagueinator.Utility.ObservableDiscreteCollection;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace Leagueinator.App.Components.MatchCard {
    public partial class MatchCard : UserControl {
        ObservableDiscreteCollection<PlayerInfo>.CollectionChangedHnd hnd1 = delegate { }, hnd2 = delegate { };

        public Match? Match {
            get => this._match;
            set {
                ArgumentNullException.ThrowIfNull(value, "MatchCard set Match");

                Debug.WriteLine(value);

                if (this._match == value) return;

                // if the current match is not null remove the previous handlers
                if (this._match != null) {
                    value.Teams[0].Players.CollectionChanged -= this.hnd1;
                    value.Teams[1].Players.CollectionChanged -= this.hnd2;
                }

                this.ClearLabels();
                for (int i = 0; i < value.Teams[0].Players.MaxSize; i++) {
                    Label label = this.AddLabel(0, "");
                    label.Text = value?.Teams[0]?.Players[i]?.Name;
                }
                for (int i = 0; i < value.Teams[1].Players.MaxSize; i++) {
                    Label label = this.AddLabel(1, "");
                    label.Text = value?.Teams[1]?.Players[i]?.Name;
                }

                this.hnd1 = (src, args) => this.PlayersCollectionChanged(this.flowTeam0, src, args);
                this.hnd2 = (src, args) => this.PlayersCollectionChanged(this.flowTeam1, src, args);

                value.Teams[0].Players.CollectionChanged += this.hnd1;
                value.Teams[1].Players.CollectionChanged += this.hnd2;

                this._match = value;
                this.Reposition();
            }
        }

        private void PlayersCollectionChanged(FlowLayoutPanel flow, ObservableDiscreteCollection<PlayerInfo> source, ObservableDiscreteCollection<PlayerInfo>.Args args) {
            Label label = (Label)flow.Controls[args.Key];

            switch (args.Action) {
                case CollectionChangedAction.Add:
                case CollectionChangedAction.Replace:
                    label.Text = args.NewValue.Name;
                    break;
                case CollectionChangedAction.Remove:
                    label.Text = "";
                    break;
            }
        }

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

        public Label AddLabel(int team, String text) {
            FlowLayoutPanel flowPanel = team == 0 ? this.flowTeam0 : this.flowTeam1;

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

            _ = new ControlDragHandlers<PlayerInfo>(label,
                () => { // get data from source
                    int index = flowPanel.Controls.IndexOf(label);
                    return this.Match.Teams[team].Players[index];
                },
                (pi) => { // set data on target
                    int index = flowPanel.Controls.IndexOf(label);
                    var response = this.Match.Teams[team].Players[index];
                    this.Match.Teams[team].Players[index] = pi;
                    return response;
                },
                (pi) => { // response to source
                    int index = flowPanel.Controls.IndexOf(label);
                    var response = this.Match.Teams[team].Players[index];
                    this.Match.Teams[team].Players[index] = pi;
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
            var text = this.txtScore0.Text;

            try {
                int score = int.Parse(text);
                if (this.Match != null) this.Match.Teams[0].Bowls = score;
            }
            catch {
                if (this.Match == null) this.txtScore0.Text = "0";
                else this.txtScore0.Text = this.Match?.Teams[0].Bowls.ToString();
            }
        }

        private void OnScore1Changed(object sender, EventArgs e) {
            var text = this.txtScore0.Text;

            try {
                int score = int.Parse(text);
                this.Match.Teams[1].Bowls = score;
            }
            catch (Exception ex) {
                this.txtScore0.Text = this.Match.Teams[1].Bowls.ToString();
            }
        }
    }
}
