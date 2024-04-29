using Leagueinator.Model.Tables;
using System.Diagnostics;
using System.Windows.Controls;
using static Leagueinator.Controls.MemoryTextBox;

namespace Leagueinator.Controls {
    public class IdlePlayersPanel : StackPanel{

        public RoundRow? RoundRow {
            get => this._roundRow;
            set {
                this._roundRow = value;
                this.Children.Clear();
                if (this.RoundRow == null) return;

                foreach (IdleRow idleRow in this.RoundRow.IdlePlayers) {
                    this.AddPlayer(idleRow.Player);
                }
                this.AddPlayer();
            }
        }

        public MemoryTextBox AddPlayer(string playerName = "") {
            MemoryTextBox textBox = new(playerName) {
                Margin = new System.Windows.Thickness(3)
            };

            this.Children.Add(textBox);
            textBox.UpdateText += this.HndTextUpdate;
            return textBox;
        }

        private void HndTextUpdate(TextUpdateData data) {
            if (data.Before.Equals(data.After)) return;

            if (this.RoundRow is null) {
                data.Source.Clear();
                return;
            };

            IdleRow? idleRow = this.RoundRow.IdlePlayers.Get(data.Before);
            if (idleRow is not null) {
                // change player name
                idleRow.Player.Name = data.After;
            }
            else {
                // Add new player
                if (!this.RoundRow.League.PlayerTable.HasRow(data.After)) {
                    this.RoundRow.League.PlayerTable.AddRow(data.After);
                }

                Debug.WriteLine($"{this.RoundRow.UID} {data.After}");
                this.RoundRow.IdlePlayers.Add(data.After);
                var textBox = this.AddPlayer();
                if (data.EventName.Equals("KeyDown")) textBox.Focus();                
            }
        }

        private RoundRow? _roundRow = null;
    }
}
