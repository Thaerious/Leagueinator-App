using Leagueinator.Model.Tables;
using System.Diagnostics;
using System.Windows.Controls;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    public partial class MatchCard : UserControl {
        private MatchRow? _matchRow;

        internal CardTarget? CardTarget { get; set; }

        public MatchRow? MatchRow {
            get => _matchRow;
            set {
                _matchRow = value;
                if (MatchRow is null) {
                    this.Clear();
                    return;
                }
                if (MatchRow.Teams[0] is not null) {
                    int i = 0;
                    foreach (MemberRow member in MatchRow.Teams[0].Members) {
                        TextBox textBox = (TextBox)this.Team0.Children[i++];
                        textBox.Text = member.Player;
                    }
                }
                if (MatchRow.Teams[1] is not null) {
                    int i = 0;
                    foreach (MemberRow member in MatchRow.Teams[1].Members) {
                        TextBox textBox = (TextBox)this.Team1.Children[i++];
                        textBox.Text = member.Player;
                    }
                }
            }
        }

        public MatchCard() {
            InitializeComponent();
        }

        private void Clear() {
            foreach (TextBox textBox in this.Team0.Children) textBox.Text = string.Empty;
            foreach (TextBox textBox in this.Team1.Children) textBox.Text = string.Empty;
            this.TxtBowls0.Text = "0";
            this.TxtBowls1.Text = "1";
            this.TxtEnds.Text = "0";
        }
    }
}
