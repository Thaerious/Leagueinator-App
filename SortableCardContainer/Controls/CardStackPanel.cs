using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Leagueinator.Controls {

    public record ReorderArgs(int prevIndex, int currentIndex);

    public delegate void CardStackPanelReorderHandler(CardStackPanel panel, ReorderArgs args);

    public class CardStackPanel : StackPanel, IEnumerable<MatchCard> {
        public event CardStackPanelReorderHandler CardStackPanelReorder = delegate { };

        private Point Last = new();
        private MatchCard? Active = null;

        public int Count => this.Children.Count;

        public MatchCard GetMatchCard(int index) {
            CardTarget target = (CardTarget)this.Children[index];
            return (MatchCard)target.Children[0];
        }

        public void Add(MatchCard matchCard) {
            CardTarget target = new CardTarget {
                Height = matchCard.Height,
                Lane = this.Children.Count + 1,
                MatchCard = matchCard
            };
            this.Children.Add(target);

            Canvas.SetTop(matchCard, 0);

            matchCard.SetBinding(WidthProperty, new Binding {
                Source = target.Canvas,
                Path = new PropertyPath("ActualWidth")
            });

            matchCard.MouseDown += HndMouseDown;
            matchCard.MouseUp += HndMouseUp;
            matchCard.MouseLeave += HndMouseLeave;
            matchCard.MouseMove += HndMouseMove;
        }

        public void HndMouseDown(object sender, MouseButtonEventArgs e) {
            if (sender is not MatchCard matchCard) return;
            Last = e.GetPosition(this);
            Panel.SetZIndex(matchCard.CardTarget, 1);
            this.Active = matchCard;
        }
        public void HndMouseUp(object sender, MouseButtonEventArgs e) {
            if (sender is not MatchCard matchCard) return;
            if (matchCard != Active) return;

            this.Active = null;
            Panel.SetZIndex(matchCard.CardTarget, 0);

            foreach (CardTarget child in this.Children) {
                Canvas.SetTop(child.Children[0], 0);
            }
        }
        public void HndMouseLeave(object sender, MouseEventArgs e) {
            if (sender is not MatchCard matchCard) return;
            if (matchCard != Active) return;

            this.Active = null;
            Panel.SetZIndex(matchCard.CardTarget, 0);

            foreach (CardTarget child in this.Children) {
                Canvas.SetTop(child.Children[0], 0);
            }
        }
        public void HndMouseMove(object sender, MouseEventArgs e) {
            if (sender is not MatchCard matchCard) return;
            if (Active is null) return;            

            Point currentPosition = e.GetPosition(this);
            Point diff = new(Last.X - currentPosition.X, Last.Y - currentPosition.Y);

            Canvas.SetTop(Active, Canvas.GetTop(Active) - diff.Y);

            Last = currentPosition;
            MatchCard? next = Next(Active);
            MatchCard? prev = Prev(Active);

            if (Canvas.GetTop(Active) > 0) /* moving down */{
                if (next is not null) Canvas.SetTop(next, Canvas.GetTop(next) + diff.Y);
            }
            else /* moving up */{
                if (prev is not null) Canvas.SetTop(prev, Canvas.GetTop(prev) + diff.Y);
            }

            var top = TopAsPercent(Active);

            var dir = 0;
            if (diff.Y > 0) dir = 1;
            if (diff.Y < 0) dir = -1;

            if (top > 0.5 && next is not null && dir == -1) Swap(Active, next);
            if (top < -0.5 && prev is not null && dir == 1) Swap(Active, prev);
        }

        /// <summary>
        /// Get the prevParent (inner) Active realative to child.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        private MatchCard? Next(MatchCard child) {
            CardTarget target = child.CardTarget ?? throw new NotSupportedException();

            int currentIndex = this.Children.IndexOf(target);
            if (currentIndex == -1) {
                return null;
            }
            if (currentIndex + 1 < this.Children.Count) {
                UIElement uiElement = this.Children[currentIndex + 1];
                if (uiElement is not CardTarget nextTarget) throw new NotSupportedException();
                return nextTarget.MatchCard;
            }

            return null;
        }

        /// <summary>
        /// Get the previous (inner) Active realative to child.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        private MatchCard? Prev(MatchCard child) {
            CardTarget target = child.CardTarget ?? throw new NotSupportedException();

            int currentIndex = this.Children.IndexOf(target);
            if (currentIndex == -1) {
                return null;
            }
            if (currentIndex > 0) {
                UIElement uiElement = this.Children[currentIndex - 1];
                if (uiElement is not CardTarget nextTarget) throw new NotSupportedException();
                return nextTarget.MatchCard;
            }

            return null;
        }

        /// <summary>
        /// Swap the two (inner) canvases.
        /// </summary>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        private void Swap(MatchCard card1, MatchCard card2) {
            CardTarget parent1 = card1.CardTarget ?? throw new NotSupportedException(); ;
            CardTarget parent2 = card2.CardTarget ?? throw new NotSupportedException();

            parent1.MatchCard = null;
            parent2.MatchCard = null;

            parent1.MatchCard = card2;
            parent2.MatchCard = card1;

            var t = Canvas.GetTop(card1);
            Canvas.SetTop(card1, Canvas.GetTop(card2));
            Canvas.SetTop(card2, t);

            Panel.SetZIndex(parent1, 0);
            Panel.SetZIndex(parent2, 1);
        }

        private static double TopAsPercent(MatchCard matchCard) {
            return Canvas.GetTop(matchCard) / matchCard.CardTarget.ActualHeight;
        }

        public IEnumerator<MatchCard> GetEnumerator() {
            foreach(CardTarget target in this.Children){
                yield return (MatchCard)target.Children[0];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
