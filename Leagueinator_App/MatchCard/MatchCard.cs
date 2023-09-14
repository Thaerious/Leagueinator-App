using Leagueinator.Model;
using Leagueinator.Utility;
using System.Diagnostics;
using static Leagueinator.Utility.ObservableDiscreteCollection<Leagueinator.Model.PlayerInfo>;

namespace WinFormsApp1.MatchCard {
    public partial class MatchCard : UserControl {
        CollectionChangedHnd? hnd1, hnd2;

        public Match Match {
            get => this._match;
            set {
                if (this._match == value) return;

                if (this._match != null) {
                    value.Teams[0].Players.CollectionChanged -= this.hnd1;
                    value.Teams[1].Players.CollectionChanged -= this.hnd2;
                }

                this.ClearLabels();
                for (int i = 0; i < value.Teams[0].Players.MaxSize; i++) {
                    Label label = this.AddLabel(0, "");
                    label.Text = value.Teams[0].Players[i]?.Name;
                }
                for (int i = 0; i < value.Teams[1].Players.MaxSize; i++) {
                    Label label = this.AddLabel(1, "");
                    label.Text = value.Teams[1].Players[i]?.Name;
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
            Debug.WriteLine("PlayerCollectionChanged");
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

            new ControlDragHandlers<PlayerInfo>(label,
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

        private Match? _match;
    }
}
