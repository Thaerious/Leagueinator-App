using Model;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Leagueinator.App.Components {

    public partial class EventPanel : UserControl {


        /// <summary>
        /// Retrieve or set the currently selected round.
        /// </summary>
        public Round? CurrentRound {
            get {
                return this.matchCardPanel.Round;
            }
            private set {
                this.matchCardPanel.Round = value;
            }
        }

        public PlayerListBox PlayerListBox => this.playerListBox;

        public EventPanel() {
            this.InitializeComponent();
        }

        public LeagueEvent? LeagueEvent {
            get => this._currentEvent;
            set {
                if (value == this.LeagueEvent) return;
                this._currentEvent = value;
                if (value == null) return;

                this.flowRounds.Controls.Clear();
                this.playerListBox.Items.Clear();

                foreach (Round round in value.Rounds) this.AddRoundButton(round);
                this.SelectLastRoundButton();
            }
        }  

        /// <summary>
        /// AddChild a round to this panel.<br>
        /// </summary>
        /// <param TagName="round"></param>
        private RoundButton AddRoundButton(Round round) {
            var button = new RoundButton(round) {
                Text = $"Round #{this.flowRounds.Controls.Count + 1}",
                Width = (int)(this.flowRounds.Width * 0.9),
                Left = (int)(this.flowRounds.Width * 0.05),
                Height = 45
            };

            this.flowRounds.Controls.Add(button);
            button.Click += (sender, args) => this.SelectRoundButton(button);
            return button;
        }

        public void HndAddRound(object _, EventArgs __) {
            if (this.LeagueEvent is null) throw new AppStateException();
            var round = this.LeagueEvent.NewRound();

            Debug.WriteLine(round.League.PrettyPrint()) ;

            this.CurrentRound = round;
            var button = this.AddRoundButton(round);
            this.SelectRoundButton(button);
        }

        public void HndDeleteRound(object _, EventArgs __) {
            if (this.LeagueEvent is null) throw new AppStateException();
            if (this.CurrentRound is null) throw new AppStateException();
            this.CurrentRound.Delete();

            int index = 1;
            foreach (var button in this.flowRounds.Controls.OfType<RoundButton>()) {
                if (button.Round == this.CurrentRound) {
                    this.flowRounds.Controls.Remove(button);
                    break;
                }
            }

            foreach (var button in this.flowRounds.Controls.OfType<RoundButton>()) {
                button.Text = $"Round #{index++}";
            }

            this.SelectLastRoundButton();
        }

        private void SelectLastRoundButton() {
            var buttons = this.flowRounds.Controls.OfType<RoundButton>().ToList();
            if (buttons.Count == 0) return;
            this.SelectRoundButton(buttons.Last());
        }

        private void SelectRoundButton(RoundButton button) {
            foreach (var b in this.flowRounds.Controls.OfType<RoundButton>()) {
                b.BackColor = Color.White;
            }

            this.CurrentRound = button.Round;
            button.BackColor = Color.LightGreen;
            this.matchCardPanel.Round = button.Round;
        }

        private LeagueEvent? _currentEvent = null;
    }
}
