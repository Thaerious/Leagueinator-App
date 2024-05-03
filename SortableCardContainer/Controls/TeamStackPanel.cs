using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Windows;
using System.Windows.Controls;

namespace Leagueinator.Controls {
    public class TeamStackPanel : StackPanel {
        private TeamRow? _teamRow;

        public static readonly DependencyProperty TeamIndexProperty = DependencyProperty.Register(
            "TeamIndex",
            typeof(int),
            typeof(TeamStackPanel),
            new PropertyMetadata(default(int))
        );

        public int TeamIndex {
            get { return (int)GetValue(TeamIndexProperty); }
            set { SetValue(TeamIndexProperty, value); }
        }

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
