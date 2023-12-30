using Model;
using Model.Tables;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

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
                this.UpdateIdlePlayers();
            }
        }

        private void UpdateIdlePlayers() {
            this.flowIdlePlayers.Controls.Clear();
            if (this.CurrentRound is null) return;

            foreach (string name in this.CurrentRound.IdlePlayers.Cast<string>()) {
                this.AddIdleTextBox(name);
            }

            this.AddIdleTextBox();
        }

        internal PlayerTextBox AddIdleTextBox(string name = "") {
            var textBox = new PlayerTextBox {
                Text = name,
                Font = new Font("Segoe UI Black", 14F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Width = 400,
                Height = 64,
            };

            this.flowIdlePlayers.Controls.Add(textBox);
            return textBox;
        }

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

            this.CurrentRound = round;
            var button = this.AddRoundButton(round);
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
        }

        private LeagueEvent? _currentEvent = null;
    }

    public class PlayerTextBox : MemoryTextBox {

        public PlayerTextBox() : base() {
            this.TextChanged += OnTextChanged;
            this.KeyDown += OnKeyDown;
            this.LostFocus += OnLostFocus;
        }

        private void OnKeyDown(object? sender, KeyEventArgs e) {
            if (this.Parent == null) throw new NullReferenceException(nameof(this.Parent));
            var controls = this.Parent.Controls;

            if (e.KeyCode == Keys.Enter) {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (controls[controls.Count - 1] == this) {
                    if (this.Text == "") return;

                    EventPanel? eventPanel = this.Parent<EventPanel>();
                    if (eventPanel == null) throw new NullReferenceException(nameof(eventPanel));
                    var textBox = eventPanel.AddIdleTextBox();
                    textBox.Focus();
                }
                else {
                    int index = controls.IndexOf(this);
                    if (this.Text == "") {
                        controls.Remove(this);
                        controls[index].Focus();
                    }
                    else {
                        controls[index + 1].Focus();
                    }
                }
            }
        }

        private void OnLostFocus(object? sender, EventArgs e) {
            if (this.Parent == null) throw new NullReferenceException(nameof(this.Parent));
            var controls = this.Parent.Controls;

            if (controls[controls.Count - 1] == this) {
                if (this.Text == "") return;

                EventPanel? eventPanel = this.Parent<EventPanel>();
                if (eventPanel == null) throw new NullReferenceException(nameof(eventPanel));
                var textBox = eventPanel.AddIdleTextBox();
                textBox.Focus();
            }
            else {
                int index = controls.IndexOf(this);
                if (this.Text == "") {
                    controls.Remove(this);
                    controls[index].Focus();
                }
                else {
                    controls[index + 1].Focus();
                }
            }
        }

        private void OnTextChanged(object? sender, MemoryUpdateArgs e) {
            EventPanel eventPanel = this.Parent<EventPanel>() ?? throw new AppStateException();
            Round round = eventPanel.CurrentRound ?? throw new AppStateException();

            var textAfter = e.TextAfter.Trim();

            if (textAfter == "") {
                round.IdlePlayers.Remove(e.TextBefore);
            }
            else if (!round.IdlePlayers.Contains(e.TextBefore)) {
                round.IdlePlayers.Add(textAfter);
            }
            else {
                round.IdlePlayers.Update(e.TextBefore, textAfter);
            }
        }
    }
}
