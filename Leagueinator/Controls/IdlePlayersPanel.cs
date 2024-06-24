using Leagueinator.Extensions;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Data;
using System.Diagnostics;
using System.Windows.Controls;
using static Leagueinator.Controls.MemoryTextBox;

namespace Leagueinator.Controls {
    public class IdlePlayersPanel : StackPanel {
        public IdlePlayersPanel() : base() {
            // Handle text box updates.
            this.AddHandler(MemoryTextBox.RegisteredUpdateEvent, new MemoryEventHandler(this.HndUpdateText));
        }

        /// <summary>
        /// Called when the text box updates a value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private void HndUpdateText(object sender, MemoryTextBoxArgs e) {
            if (this.RoundRow is null) throw new NullReferenceException(nameof(this.RoundRow));

            string before = e.Before?.Trim() ?? "";
            string after = e.After?.Trim() ?? "";

            Debug.WriteLine($"Idle Panel Update Text '{before}' => '{after}' {e.Cause}");

            // If idle table doesn't already have after, add it
            // Else clear text box
            if (!after.IsEmpty()) {
                if (this.RoundRow.IdlePlayers.Has(after)) {
                    e.TextBox.Text = before;
                    return;
                }
                else {
                    this.RoundRow.IdlePlayers.Add(after);
                }
            }

            // Remove the value that was in the text box before.
            if (!before.IsEmpty()) {
                IdleRow idleRow = this.RoundRow.IdlePlayers.Get(e.Before) ?? throw new ArgumentNullException(nameof(idleRow));
                idleRow.Remove();
            }

            // Advance to the next text box if ENTER was pressed.
            if (e.Cause.Equals(Cause.EnterPressed)) {
                int index = this.Children.IndexOf(e.TextBox);
                if (index + 1 < this.Children.Count) {
                    this.Children[index + 1].Focus();
                    (this.Children[index + 1] as TextBox)!.SelectAll();
                }
            }

            // If after is empty and this isn't the last text box, remove it
            if (after.IsEmpty() && e.TextBox != this.Children[^1]) {
                this.Children.Remove(e.TextBox);
            }
        }

        /// <summary>
        /// Set the current round row for this panel.
        /// </summary>
        public RoundRow? RoundRow {
            get => this._roundRow;
            set {
                if (this.RoundRow is not null) {
                    this.RoundRow.League.IdleTable.RowChanged -= this.HndIdleTableNewRow;
                    this.RoundRow.League.IdleTable.RowDeleting -= this.HndIdleTableDeletingRow;
                }

                this._roundRow = value;
                this.Children.Clear();
                if (this.RoundRow == null) return;

                this.RoundRow.League.IdleTable.RowChanged += this.HndIdleTableNewRow;
                this.RoundRow.League.IdleTable.RowDeleting += this.HndIdleTableDeletingRow;

                foreach (IdleRow idleRow in this.RoundRow.IdlePlayers) {
                    this.AddTextBox(idleRow.Player);
                }

                this.AddTextBox();
            }
        }

        private void HndIdleTableDeletingRow(object sender, DataRowChangeEventArgs e) {
            Debug.WriteLine("HndIdleTableDeletingRow");
            IdleRow idleRow = new(e.Row);

            MemoryTextBox? target = null;
            foreach (MemoryTextBox textBox in this.Children) {
                if (textBox.Text == idleRow.Player) {
                    textBox.Text = "";
                    if (textBox != this.Children[^1]) target = textBox;
                    break;
                }
            }

            if (target != null) this.Children.Remove(target);
        }

        private void HndIdleTableNewRow(object sender, DataRowChangeEventArgs e) {
            Debug.WriteLine("HndIdleTableNewRow");
            IdleRow idleRow = new(e.Row);

            // Check that the name is exists
            bool textBoxExists = false;
            foreach (MemoryTextBox textBox in this.Children) {
                if (textBox.Text == idleRow.Player) {
                    textBoxExists = true;
                    break;
                }
            }

            if (!textBoxExists) {
                (this.Children[^1] as MemoryTextBox)!.Text = idleRow.Player;
            }

            // Add a new empty text box as needed
            if (!(this.Children[^1] as MemoryTextBox)!.Text.IsEmpty()) {
                MemoryTextBox newTextBox = (this.Children[0] as MemoryTextBox)!.XAMLClone<MemoryTextBox>()!;
                newTextBox.Text = "";
                this.Children.Add(newTextBox);
            }
        }

        public MemoryTextBox AddTextBox(string playerName = "") {
            PlayerTextBox textBox = new(playerName) {
                Margin = new System.Windows.Thickness(3)
            };

            this.Children.Add(textBox);
            return textBox;
        }

        private RoundRow? _roundRow = null;
    }
}
