using Leagueinator.Model;
using System.Collections.Specialized;

namespace Leagueinator.App.Components.EventPanel {

    public partial class EventPanel : UserControl {
        /// <summary>
        /// Retrieve or set the currently selected round.
        /// On set the current round is replaced in both the league event
        /// and this panel.
        /// </summary>
        public Round? CurrentRound {
            get {
                if (this.LeagueEvent == null) return null;
                if (this._currentRoundIndex == -1) return null;
                return this.LeagueEvent.Rounds[this._currentRoundIndex];
            }
        }

        public int CurrentRoundIndex {
            get => this._currentRoundIndex;
            private set {
                if (this._currentRoundIndex == value) return;
                this._currentRoundIndex = value;
                this.UpdateMatchCards();
            }
        }

        public PlayerListBox.PlayerListBox PlayerListBox => this.playerListBox;

        public EventPanel() {
            this.InitializeComponent();
        }

        public LeagueEvent? LeagueEvent {
            get { return this.leagueEvent; }
            set {
                if (value != null) {
                    if (value == this.LeagueEvent) return; 

                    this.leagueEvent = value;
                    this.flowRounds.Controls.Clear();
                    this.playerListBox.Items.Clear();                    

                    foreach (Round round in value.Rounds) this.AddRoundButton(round);

                    this._currentRoundIndex = value.Rounds.Count - 1;
                    if (this._currentRoundIndex >= 0) {
                        this.flowRounds.Controls[this._currentRoundIndex].BackColor = Color.LightGreen;
                        this.playerListBox.Round = this?.LeagueEvent?.Rounds[this._currentRoundIndex];
                    }

                    this.UpdateMatchCards();

                    // Whenever a round is added or removed.
                    this.leagueEvent.Rounds.CollectionChanged += (source, args) => {
                        switch (args.Action) {
                            case NotifyCollectionChangedAction.Add:
                                if (args.NewItems is null) return;
                                foreach (Round round in args.NewItems) this.AddRoundButton(round);
                                break;
                            case NotifyCollectionChangedAction.Remove:
                                this.flowRounds.Controls.RemoveAt(args.OldStartingIndex);
                                for (int i = 0; i < this.flowRounds.Controls.Count; i++) {
                                    this.flowRounds.Controls[i].Text = $"Round #{i + 1}";
                                }

                                if (args.OldStartingIndex == this._currentRoundIndex) {
                                    //this.playerListBox.Items.Clear();
                                    this.CurrentRoundIndex = -1;
                                }
                                break;
                        }
                    };

                    //ScoreKeeper.Singleton.LeagueEvent = value; // TODO
                }
            }
        }

        /// <summary>
        /// AddChild a round to this panel.<br>
        /// </summary>
        /// <param Name="round"></param>
        private void AddRoundButton(Round round) {
            var button = new Button() {
                Text = $"Round #{this.flowRounds.Controls.Count + 1}",
                Width = (int)(this.flowRounds.Width * 0.9),
                Left = (int)(this.flowRounds.Width * 0.05),
                Height = 45
            };
            this.flowRounds.Controls.Add(button);

            button.Click += new EventHandler(this.RoundButtonClick);
        }

        /// <summary>
        /// This is the "Set Round" control point.
        /// </summary>
        /// <param Name="source"></param>
        /// <param Name="_"></param>
        private void RoundButtonClick(object? source, EventArgs _) {
            ArgumentNullException.ThrowIfNull(source, nameof(source));

            Button button = (Button)source;
            int index = this.flowRounds.Controls.IndexOf(button);

            if (this.CurrentRoundIndex > -1) {
                this.flowRounds.Controls[this.CurrentRoundIndex].BackColor = Color.White;
            }

            this.flowRounds.Controls[index].BackColor = Color.LightGreen;
            this.playerListBox.Round = this?.LeagueEvent?.Rounds[index];

            this.CurrentRoundIndex = index;
        }

        /// <summary>
        /// Populate the match card panel with new match cards.
        /// </summary>
        /// <return>The last card added</return>
        /// <param Name="round"></param>
        private void UpdateMatchCards() {
            this.flowMatchCards.Controls.Clear();
            if (this.CurrentRoundIndex < 0) return;
            if (this.CurrentRound == null) return;

            int lane = 1;
            foreach (Match match in this.CurrentRound.Matches) {
                var matchCard = new MatchCard.MatchCard();
                matchCard.Match = match;
                matchCard.Lane = lane++;
                this.flowMatchCards.Controls.Add(matchCard);
            }

        }

        private LeagueEvent? leagueEvent = null;

        private int _currentRoundIndex = -1;
    }
}
