using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using static Leagueinator.Controls.MemoryTextBox;

namespace Leagueinator.Controls {
    public class IdlePlayersPanel : StackPanel {

        public IdlePlayersPanel() : base() {
            this.AddHandler(MemoryTextBox.RegisteredUpdateEvent, new MemoryEventHandler(HndUpdateText));
        }

        private void HndUpdateText(object sender, MemoryTextBoxArgs e) {
            if (this.RoundRow is null) throw new NullReferenceException(nameof(this.RoundRow));

            if (!e.Before.IsEmpty()) {
                this.RoundRow.IdlePlayers.Get(e.Before)!.Remove();
            }

            if (e.After.IsEmpty() && e.TextBox != this.Children[^1]) {
                // Remove empty text boxes unless it's the last one.
                this.Children.Remove(e.TextBox);
            }
            else {
                this.RoundRow.League.PlayerTable.AddRowIf(e.After);
                this.RoundRow.IdlePlayers.Add(e.After);

                // If it's the last text box add a new empty text box.
                if (e.TextBox == this.Children[^1]) {
                    this.AddTextBox();
                }

                if (e.Cause.Equals(Cause.KeyDown)) {
                    int index = this.Children.IndexOf(e.TextBox);
                    if (index + 1 < this.Children.Count) {
                        this.Children[index + 1].Focus();
                    }
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
            IdleRow idleRow = new(e.Row);

            Debug.WriteLine(e.Row.RowState);
            Debug.WriteLine(e.Row.Field<string>("Player"));

            //Debug.WriteLine($"IdlePlayersPanel.HndIdleTableDeleteRow {idleRow.Player}");

            //foreach (MemoryTextBox textBox in this.Children) {
            //    if (textBox.Text == idleRow.Player) this.Children.Remove(textBox);
            //}
        }

        private void HndIdleTableNewRow(object sender, DataRowChangeEventArgs e) {
            if (e.Action != DataRowAction.Add) return;

            //Debug.WriteLine($"IdlePlayersPanel.HndIdleTableNewRow {e.Action} {e.Row.Table.TableName}");
            //Debug.WriteLine(e.Row.PrettyPrint());

            //for (int i = 0; i < this.ForeignKeyColumn.Length; i++) {
            //    DataColumn column = this.ForeignKeyColumn[i];
            //    object value = this.ForeignKeyValue[i];

            //    Debug.WriteLine($"row[{column.ColumnName}] = {e.Row[column]}");

            //    if (e.Row[column] != value) return;
            //}

            //Debug.WriteLine($"RowBoundView.HndTableNewRow {e.Row.Table.TableName} Invoke");
            //NewBoundRow.Invoke(this, NewRow(e.Row));
        }

        public MemoryTextBox AddTextBox(string playerName = "") {
            MemoryTextBox textBox = new(playerName) {
                Margin = new System.Windows.Thickness(3)
            };

            this.Children.Add(textBox);
            return textBox;
        }
 
        private RoundRow? _roundRow = null;
    }
}
