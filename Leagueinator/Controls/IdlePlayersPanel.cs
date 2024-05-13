using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using static Leagueinator.Controls.MemoryTextBox;

namespace Leagueinator.Controls {
    public class IdlePlayersPanel : StackPanel {
        private readonly Dictionary<DataRow, MemoryTextBox> RowToTextBox = [];

        public IdlePlayersPanel() : base() {
            // Handle text box updates.
            this.AddHandler(MemoryTextBox.RegisteredUpdateEvent, new MemoryEventHandler(HndUpdateText));
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

            if (this.RoundRow.AllPlayers.Contains(e.After)) {
                // if the player already exists reject.
                MessageBox.Show($"Player {e.After} previously assigned.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.TextBox.Text = e.Before;
                return;
            }

            if (!e.Before.IsEmpty()) {
                IdleRow idleRow = this.RoundRow.IdlePlayers.Get(e.Before) ?? throw new ArgumentNullException(nameof(idleRow));
                this.RowToTextBox.Remove(idleRow!);
                idleRow.Remove();
            }

            if (e.After.IsEmpty() && e.TextBox != this.Children[^1]) {
                // Remove empty text boxes unless it's the last one.
                this.Children.Remove(e.TextBox);
            }
            else {
                try {
                    // A new value has been set create a row and save it
                    this.RoundRow.League.PlayerTable.AddRowIf(e.After);
                    IdleRow idleRow = this.RoundRow.IdlePlayers.Add(e.After);
                    this.RowToTextBox[idleRow!] = e.TextBox;

                    // If it's the last text box add a new empty text box.
                    if (e.TextBox == this.Children[^1]) {
                        this.AddTextBox();
                    }

                    // Advance to the next text box if ENTER was pressed.
                    if (e.Cause.Equals(Cause.EnterPressed)) {
                        int index = this.Children.IndexOf(e.TextBox);
                        if (index + 1 < this.Children.Count) {
                            this.Children[index + 1].Focus();
                        }
                    }
                } catch (Exception ex){
                    string msg = "An exception has occurred:\n" + ex.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.TextBox.Text = e.Before;
                }
            }
        }

        public RoundRow? RoundRow {
            get => this._roundRow;
            set {
                if (this.RoundRow is not null) {
                    this.RoundRow.League.IdleTable.RowChanged -= this.HndIdleTableNewRow;
                    this.RoundRow.League.IdleTable.RowDeleted -= this.HndIdleTableDeleteRow;
                }
                
                this._roundRow = value;                
                this.Children.Clear();
                if (this.RoundRow == null) return;

                this.RoundRow.League.IdleTable.RowChanged += this.HndIdleTableNewRow;
                this.RoundRow.League.IdleTable.RowDeleted += this.HndIdleTableDeleteRow;

                foreach (IdleRow idleRow in this.RoundRow.IdlePlayers) {
                    this.AddTextBox(idleRow.Player);
                }

                this.AddTextBox();
            }
        }

        private void HndIdleTableDeleteRow(object sender, DataRowChangeEventArgs e) {
            if (!this.RowToTextBox.TryGetValue(e.Row, out MemoryTextBox? textBox)) return;

            textBox.Clear();
            this.RowToTextBox.Remove(e.Row);

            if (textBox != this.Children[^1]) this.Children.Remove(textBox);
        }

        private void HndIdleTableNewRow(object sender, DataRowChangeEventArgs e) {
            if (e.Action != DataRowAction.Add) return;
            if (this.RowToTextBox.ContainsKey(e.Row)) return;
            if (e.Row is null) throw new NullReferenceException("DataRowChangeEventArgs.Row");
            PlayerTextBox textBox = (this.Children[^1] as PlayerTextBox)! ?? throw new NullReferenceException("this.Children[^1]:PlayerTextBox");

            textBox.Text = new IdleRow(e.Row).Player;
            this.RowToTextBox[e.Row] = textBox;
            this.AddTextBox();
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
