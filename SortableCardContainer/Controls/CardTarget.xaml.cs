using System.Windows.Controls;

namespace Leagueinator.Controls {
    /// <summary>
    /// Interaction logic for CardTarget.xaml
    /// </summary>
    public partial class CardTarget : UserControl {
        public CardTarget() {
            InitializeComponent();
        }

        public UIElementCollection Children {
            get => this.Canvas.Children;
        }

        public int Lane {
            get => int.Parse(this.LaneLabel.Content.ToString() ?? "0");
            set => this.LaneLabel.Content = value;
        }

        public MatchCard? MatchCard {
            get {
                if (this.Canvas.Children.Count == 0) return null;
                return (MatchCard)this.Canvas.Children[0];
            }

            set {
                if (this.MatchCard is not null) this.Canvas.Children.Remove(this.MatchCard);
                if (value == null) return;
                this.Canvas.Children.Add(value);
                value.CardTarget = this;
            }
        }

        public MatchCard? SetMatchCard(MatchCard matchcard) {
            MatchCard? previous = null;
            if (this.Canvas.Children.Count > 0) {
                previous = (MatchCard)this.Canvas.Children[0];
                this.Canvas.Children.Remove(previous);
                previous.CardTarget = null;
            }
            this.Canvas.Children.Add(matchcard);
            matchcard.CardTarget = this;
            return previous;
        }

        public static implicit operator Canvas(CardTarget target) {
            return target.Canvas;
        }
    }
}
