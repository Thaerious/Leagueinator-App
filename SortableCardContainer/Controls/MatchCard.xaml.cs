using System.Windows.Controls;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for MatchCard.xaml
    /// </summary>
    public partial class MatchCard : UserControl {
        internal CardTarget? CardTarget { get; set; }

        public MatchCard() {
            InitializeComponent();
        }
    }
}
