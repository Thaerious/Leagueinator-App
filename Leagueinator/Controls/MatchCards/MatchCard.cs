using Leagueinator.Extensions;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Leagueinator.Controls {
    public class MatchCardFormatArgs(RoutedEvent routedEvent, MatchCard matchCard, MatchType matchType) : RoutedEventArgs(routedEvent) {
        public readonly MatchCard MatchCard = matchCard;
        public readonly MatchType MatchType = matchType;
    }

    public abstract class MatchCard : UserControl {
        public delegate void FormatChangedEventHandler(object sender, MatchCardFormatArgs e);

        public static readonly RoutedEvent RegisteredFormatChangedEvent = EventManager.RegisterRoutedEvent(
            "FormatChanged",          // Event name
            RoutingStrategy.Bubble,   // Routing strategy (Bubble, Tunnel, or Direct)
            typeof(FormatChangedEventHandler),    // Delegate type
            typeof(MatchCard)         // Owner type
        );

        public event FormatChangedEventHandler FormatChanged {
            add { AddHandler(RegisteredFormatChangedEvent, value); }
            remove { RemoveHandler(RegisteredFormatChangedEvent, value); }
        }

        public abstract MatchRow MatchRow { get; set; }
        internal CardTarget? CardTarget { get; set; }

        public void HndCheckFormat(object sender, RoutedEventArgs e) {
            MenuItem menuItem = sender as MenuItem;
            Debug.WriteLine($"MatchCard.HndCheckFormat {menuItem.Header}");
            MatchCardFormatArgs newEventArgs = new(RegisteredFormatChangedEvent, this, MatchType.VS4);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Triggered when the value of a players name (MemoryTextBox) is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NullReferenceException"></exception>
        protected void HndUpdateText(object sender, MemoryTextBoxArgs e) {
            if (this.MatchRow is null) throw new NullReferenceException(nameof(MatchRow));

            if (this.MatchRow.Round.AllPlayers.Contains(e.After)) {
                // if the player already exists in the round, reject.
                MessageBox.Show($"Player {e.After} previously assigned.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.TextBox.Text = e.Before;
            }
            else {
                TeamCard parent = e.TextBox.Ancestors<TeamCard>()[0];

                // Remove new name from the idle table
                if (this.MatchRow.Round.IdlePlayers.Has(e.After)) {
                    this.MatchRow.Round.IdlePlayers.Get(e.After)!.Remove();
                }

                // Add new name to the teams table
                if (!e.After.IsEmpty()) {
                    this.MatchRow.League.PlayerTable.AddRowIf(e.After);
                    this.MatchRow.Teams[parent.TeamIndex]!.Members.Add(e.After);
                }

                // Remove the old name from the members table
                if (!e.Before.IsEmpty()) {
                    TeamRow teamRow = this.MatchRow.Teams[parent.TeamIndex] ?? throw new NullReferenceException();
                    MemberRow memberRow = teamRow.Members.Get(e.Before) ?? throw new NullReferenceException();
                    memberRow.Remove();
                }
            }

            if (e.Cause == Cause.EnterPressed) {
                TeamStackPanel parent = (TeamStackPanel)e.TextBox.Parent;
                int index = parent.Children.IndexOf(e.TextBox);
                if (index + 1 < parent.Children.Count) {
                    parent.Children[index + 1].Focus();
                }
            }
        }

        protected MatchRow? _matchRow = default;
    }
}
