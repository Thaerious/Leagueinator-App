using Model;
using System.Diagnostics;

namespace Leagueinator.App.Components {
    public class MatchCardPanel : FlowLayoutPanel {

        public MatchCardPanel() : base() {
            InitializeComponents();
            this.Resize += OnResize;
        }

        public Round? Round {
            get => _round;
            set {
                this.Controls.Clear();
                if (value == null) return;

                int laneCount = int.Parse(value.LeagueEvent.Settings["lane_count"]);
                int teamSize = int.Parse(value.LeagueEvent.Settings["team_size"]);

                for (int i = 0; i < laneCount; i++) {
                    var matchCard = this.AddMatchCard(value.GetMatch(i));
                }
            }
        }

        private void OnResize(object? sender, EventArgs e) {
            int scrollbarWidth = SystemInformation.VerticalScrollBarWidth;

            foreach (Control control in this.Controls) {
                if (control is MatchCard matchCard) {
                    matchCard.Width = this.Width - this.Margin.Left - this.Margin.Right - scrollbarWidth - 5;
                    matchCard.Left = 2;
                }
            }
        }

        public MatchCard AddMatchCard(Match match) {
            MatchCard matchCard = new(match) {
                Width = (int)(this.Width * 0.8)
            };
            this.Controls.Add(matchCard);
            return matchCard;
        }

        private void InitializeComponents() {
            this.SuspendLayout();

            this.FlowDirection = FlowDirection.TopDown;
            this.AutoScroll = true;
            this.WrapContents = false;
            this.HorizontalScroll.Enabled = false;
            this.HorizontalScroll.Visible = false;
            this.HorizontalScroll.Maximum = 0;
            this.AutoScrollMinSize = new Size(0, this.AutoScrollMinSize.Height);

            this.PerformLayout();
            this.ResumeLayout();
        }

        internal MatchCard? Dragging { get; set; }
        private Round? _round = default;
    }
}
