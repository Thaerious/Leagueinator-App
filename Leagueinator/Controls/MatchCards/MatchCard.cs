﻿using Leagueinator.Extensions;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using static Leagueinator.Controls.MemoryTextBox;

namespace Leagueinator.Controls {
    public class MatchCardFormatArgs(RoutedEvent routedEvent, MatchCard matchCard, MatchFormat matchFormat) : RoutedEventArgs(routedEvent) {
        public readonly MatchCard MatchCard = matchCard;
        public readonly MatchFormat MatchFormat = matchFormat;
    }

    public abstract class MatchCard : UserControl {
        public delegate void FormatChangedEventHandler(object sender, MatchCardFormatArgs e);

        public static readonly RoutedEvent RegisteredFormatChangedEvent = EventManager.RegisterRoutedEvent(
            "FormatChanged",                      // Event name
            RoutingStrategy.Bubble,               // Routing strategy (Bubble, Tunnel, or Direct)
            typeof(FormatChangedEventHandler),    // Delegate type
            typeof(MatchCard)                     // Owner type
        );

        public event FormatChangedEventHandler FormatChanged {
            add { AddHandler(RegisteredFormatChangedEvent, value); }
            remove { RemoveHandler(RegisteredFormatChangedEvent, value); }
        }

        public abstract MatchFormat MatchFormat { get; }

        public virtual MatchRow MatchRow {
            get => this._matchRow ?? throw new InvalidOperationException("MatchRow not initialized");
            set {
                this._matchRow = value;
                if (this._matchRow is null) {
                    this.Clear();
                    this.DataContext = null;
                    return;
                }
            }
        }
        internal CardTarget? CardTarget { get; set; }

        public MatchCard() {
            this.AddHandler(MemoryTextBox.RegisteredUpdateEvent, new MemoryEventHandler(HndUpdateText));
            this.Loaded += this.HndLoaded;
        }

        private void HndLoaded(object sender, RoutedEventArgs e) {
            this.IsLoaded = true;
            this._deferred.Invoke();
            this._deferred = delegate { };
        }

        public Action Deferred {
            get => this._deferred;

            set {
                if (this.IsLoaded) value.Invoke();
                else this._deferred = value;
            }
        }

        public abstract void Clear();

        public TeamCard? GetTeamCard(int teamIndex) {
            List<TeamCard> teamCards = this.Descendants<TeamCard>();

            foreach (TeamCard teamCard in teamCards) {
                if (teamCard.TeamIndex == teamIndex) return teamCard;
            }

            return null;
        }

        public void HndChangeFormat(object sender, RoutedEventArgs e) {
            MenuItem menuItem = (MenuItem)sender;

            if (menuItem.Tag is null) return;  // tag is null during initialization
            if (menuItem.Tag is not string customData) throw new NullReferenceException("Missing tag on context menu item");

            bool success = Enum.TryParse(customData, out MatchFormat matchFormat);
            if (!success) throw new ArgumentException("Error on tag on context menu item");

            MatchCardFormatArgs newEventArgs = new(RegisteredFormatChangedEvent, this, matchFormat);
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

            string before = e.Before?.Trim() ?? "";
            string after = e.After?.Trim() ?? "";

            Debug.WriteLine($"Match Card Update Text '{before}' => '{after}'");

            TeamCard parent = e.TextBox.Ancestors<TeamCard>()[0];

            // Ensure the team exists
            if (!this.MatchRow.Teams.Has(parent.TeamIndex)) {
                this.MatchRow.Teams.Add(parent.TeamIndex);
            }

            // Add new name to the teams table
            if (!after.IsEmpty()) {
                this.MatchRow.League.PlayerTable.AddRowIf(after);
                this.MatchRow.Teams[parent.TeamIndex]!.Members.Add(after);
            }

            // Remove the old name from the members table
            if (!before.IsEmpty()) {
                TeamRow teamRow = this.MatchRow.Teams[parent.TeamIndex] ?? throw new NullReferenceException();
                if (teamRow.Members.Has(before)) {
                    teamRow.Members.Get(before).Remove();
                }
            }

            if (e.Cause == Cause.EnterPressed) {
                StackPanel stackPanel = (StackPanel)e.TextBox.Parent;
                int index = stackPanel.Children.IndexOf(e.TextBox);
                if (index + 1 < stackPanel.Children.Count) {
                    stackPanel.Children[index + 1].Focus();
                }
            }
        }

        protected void NormalizeTeams(int teamCount, int teamSize) {
            // Ensure there are not too many teams
            while (this.MatchRow.Teams.Count > teamCount) {
                TeamRow teamRow = this.MatchRow.Teams[^1]!;
                foreach (MemberRow memberRow in teamRow.Members) {
                    this.MatchRow.Round.IdlePlayers.Add(memberRow.Player);
                }
                teamRow.Remove();
            }

            // Ensure there are enough teams
            while (this.MatchRow.Teams.Count < teamCount) {
                this.MatchRow.Teams.Add(this.MatchRow.Teams.Count);
            }

            // Ensure that each team is not oversize
            foreach (TeamRow teamRow in this.MatchRow.Teams) {
                while (teamRow.Members.Count > teamSize) {
                    this.MatchRow.Round.IdlePlayers.Add(teamRow.Members[^1]!.Player);
                }
            }

            foreach (TeamCard teamCard in this.Descendants<TeamCard>()) {
                int index = teamCard.TeamIndex;
                teamCard.TeamRow = this.MatchRow.Teams[index];
            }
        }

        protected MatchRow? _matchRow = default;
        private Action _deferred = delegate { };
        private bool IsLoaded = false;
    }
}
