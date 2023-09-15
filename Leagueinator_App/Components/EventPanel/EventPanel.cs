using Leagueinator.Model;
using System.Collections.Specialized;
using System.Diagnostics;
using Leagueinator.App.Components.MatchCard;

namespace Leagueinator.App.Components.EventPanel {

    public partial class EventPanel : UserControl {
        /// <summary>
        /// Retrieve or set the currently selected round.
        /// On set the current round is replaced in both the league event
        /// and this panel.
        /// </summary>
        public Round CurrentRound {
            get {
                if (this.currentRoundIndex == -1) return null;
                return this.LeagueEvent.Rounds[this.currentRoundIndex];
            }
        }

        public int CurrentRoundIndex {
            get => this.currentRoundIndex;
            private set {
                this.currentRoundIndex = value;
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
                    this.leagueEvent = value;
                    this.flowRounds.Controls.Clear();
                    this.playerListBox.Items.Clear();
                    this.CurrentRoundIndex = -1;

                    foreach (Round round in value.Rounds) this.AddRoundButton(round);

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
                                    Debug.WriteLine(i);
                                    this.flowRounds.Controls[i].Text = $"Round #{i + 1}";
                                }

                                if (args.OldStartingIndex == this.currentRoundIndex) {
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
        /// Add a round to this panel.<br>
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

            if (this.CurrentRoundIndex == -1) {
                this.RoundButtonClick(button, null);
            }
        }

        /// <summary>
        /// This is the "Set Round" control point.
        /// </summary>
        /// <param Name="source"></param>
        /// <param Name="_"></param>
        private void RoundButtonClick(object source, EventArgs _) {
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

            int lane = 1;
            foreach (Match match in this.CurrentRound.Matches) {
                var matchCard = new MatchCard.MatchCard();
                matchCard.Match = match;
                matchCard.Lane = lane++;
                this.flowMatchCards.Controls.Add(matchCard);
            }

        }

        internal void RefreshRound() {
            // TODO REMOVE THIS METHOD
            this.UpdateMatchCards();
        }

        private LeagueEvent? leagueEvent = null;

        private int currentRoundIndex = -1;
    }
}
