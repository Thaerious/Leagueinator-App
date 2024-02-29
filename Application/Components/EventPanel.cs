using Model.Tables;
using System.ComponentModel;

namespace Leagueinator.Components {
    public partial class EventPanel : UserControl {
        private Controller? _controller;

        public EventPanel() {
            InitializeComponent();
        }

        [Category("Custom Properties")]
        [Description("Model Controller.")]
        public Controller? Controller {
            get => _controller;
            set {
                if (_controller == value) return;
                _controller = value;
                if (value == null) return;

                this.GetControls<MatchCard>()
                    .ToList()
                    .ForEach(c => c.Controller = value);

                foreach (Control control in this.Controls) {
                    if (control is MatchCard matchCard) {
                        matchCard.Controller = value;
                    }
                }

                value.OnAddRound += this.OnAddRound;
            }
        }

        private void OnAddRound(RoundRow roundRow) {
            RoundButton button = new RoundButton(roundRow);
            button.Text = $"Round {this.flowRounds.Controls.Count + 1}";
            this.flowRounds.Controls.Add(button);

            button.Height = 35;
            button.Width = 280;

            button.Click += this.RoundButtonClick;
        }

        private void RoundButtonClick(object? sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private void LayoutEventHander(object sender, LayoutEventArgs e) {
            foreach (Control child in this.flowLayoutPanel1.Controls) {
                child.Width = this.flowLayoutPanel1.ClientSize.Width
                            - this.flowLayoutPanel1.Padding.Horizontal
                            - child.Margin.Horizontal;
            }
        }
    }
}
