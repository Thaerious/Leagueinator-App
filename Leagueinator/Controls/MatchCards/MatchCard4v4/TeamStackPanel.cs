using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Windows.Controls;

namespace Leagueinator.Controls {
    public class TeamStackPanel : StackPanel {
        private TeamRow? _teamRow;

        public TeamRow? TeamRow {
            get => this._teamRow;
            set {
                this._teamRow = value;
                this.Clear();
                if (this.TeamRow == null) return;

                foreach (MemberRow memberRow in this.TeamRow.Members) {
                    this.AddName(memberRow.Player);
                }
            }
        }

        public void AddName(string name) {
            foreach (MemoryTextBox textBox in this.Children) {
                if (textBox.Text.IsEmpty()) {
                    textBox.Text = name;
                    return;
                }
            }
            throw new IndexOutOfRangeException("No available text boxes.");
        }

        public void Clear() {
            foreach (MemoryTextBox textBox in this.Children) {
                textBox.Clear();
            }
        }
    }
}
