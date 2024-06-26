﻿using Leagueinator.Controls;
using Leagueinator.Forms.MatchAssignments;
using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using static Leagueinator.Controls.MatchCard;

namespace Leagueinator.Forms.Main {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static string VERSION = "0.1.1";

        public MainWindow() {
            this.Closed += (s, e) => {
                Debug.WriteLine("TimeTrace Report");
                Debug.WriteLine(TimeTrace.Report());
            };

            this.InitializeComponent();
            this.League = NewLeague();

            SaveState.StateChanged += this.HndStateChanged;
            this._league.LeagueUpdate += this.HndLeagueUpdate;
            this.EventRow = this.League.EventTable.GetLast();

            this.AddHandler(MatchCard.RegisteredFormatChangedEvent, new FormatChangedEventHandler(this.HndFormatChanged));
        }

        /// <summary>
        /// Indirectly triggered when a change is made to one of the underlying tables.
        /// </summary>
        /// <param name="isSaved">The new state isSaved</param>        
        private void HndStateChanged(object? sender, bool isSaved) {
            if (isSaved) this.Title = $"Leagueinator [{SaveState.Filename}]";
            else this.Title = $"Leagueinator [{SaveState.Filename}] *";
        }

        /// <summary>
        /// Triggered when a change is made to the underlying model.
        /// </summary>
        /// <param name="IsSaved">The new state isSaved</param>
        private void HndLeagueUpdate(object? sender, EventArgs? e) {
            SaveState.ChangeState(sender, false);
        }

        private void HndEventManagerClick(object sender, RoutedEventArgs e) {
            EventManager dialog = new EventManager(this.League);
            if (dialog.ShowDialog() == true) {
                if (dialog.Selected is null) return;

                League league = dialog.Selected.EventRow.League;
                EventRow eventRow = dialog.Selected.EventRow;
                if (eventRow.Rounds.Count == 0) {
                    RoundRow roundRow = eventRow.Rounds.Add();
                    roundRow.PopulateMatches();
                }

                this.League = dialog.Selected.EventRow.League;
                this.EventRow = dialog.Selected.EventRow;
            }
        }

        private void HndNewClick(object sender, RoutedEventArgs e) {
            this.Focus();
            this.League = NewLeague();
        }

        private void HndLoadClick(object sender, RoutedEventArgs e) {
            this.Focus();

            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "League Files (*.league)|*.league"
            };

            if (dialog.ShowDialog() == true) {
                League newLeague = new();
                newLeague.ReadXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;

                this.League = newLeague;
                this.EventRow = this.League.EventTable.GetLast();

                SaveState.ChangeState(sender, true);
            }
        }

        private static League NewLeague() {
            League league = new();
            EventRow eventRow = league.Events.Add("Default Event", DateTime.Today.ToString("yyyy-MM-dd"));
            RoundRow roundRow = eventRow.Rounds.Add();
            roundRow.PopulateMatches();
            return league;
        }

        private void ClearFocus() {
            FocusManager.SetFocusedElement(this, null);
        }

        private void HndSaveClick(object sender, RoutedEventArgs e) {
            if (this.League is null) return;
            this.ClearFocus();

            if (SaveState.Filename.IsEmpty()) this.HndSaveAsClick(null, null);
            else this.League.WriteXml(SaveState.Filename);
            SaveState.ChangeState(sender, true);
        }

        private void HndSaveAsClick(object sender, RoutedEventArgs e) {
            if (this.League is null) return;
            this.ClearFocus();

            SaveFileDialog dialog = new();
            dialog.Filter = "League Files (*.league)|*.league";

            if (dialog.ShowDialog() == true) {
                this.League.WriteXml(dialog.FileName);
                SaveState.Filename = dialog.FileName;
                SaveState.ChangeState(sender, true);
            }
        }
        private void HndCloseClick(object sender, RoutedEventArgs e) {
            // TODO check for unsaved changes
            // TODO remove current league
        }
        private void HndExitClick(object sender, RoutedEventArgs e) {
            this.Close(); // TODO check for unsaved changes
        }

        private void HndEventsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.EventTable);
        }
        private void HndRoundsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.RoundTable);
        }
        private void HndMatchesClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.MatchTable);
        }
        private void HndTeamsClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.TeamTable);
        }
        private void HndMembersClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.MemberTable);
        }
        private void HndIdleClick(object sender, RoutedEventArgs e) {
            new TableViewer().Show(this.League.IdleTable);
        }
        private void HndViewResults(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) throw new NullReferenceException(nameof(this.EventRow));
            this.ClearFocus();
            PrinterForm form = new(new ResultPlusXMLBuilder(this.EventRow)) {
                Owner = this
            };
            form.Show();
        }

        private void HndMatchAssignments(object sender, RoutedEventArgs e) {
            if (this.EventRow is null) throw new NullReferenceException(nameof(this.EventRow));
            this.ClearFocus();
            PrinterForm form = new(new MatchAssignmentsBuilder(this.CurrentRoundRow)) {
                Owner = this
            };
            form.Show();
        }


        private void HndMenuClick(object sender, RoutedEventArgs e) {
            this.Focus();
        }

        public void HndHelpAbout(object sender, RoutedEventArgs e) {
            MessageBox.Show(MainWindow.VERSION, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        static class SaveState {
            public delegate void StateChangedHandler(object? source, bool IsSaved);
            public static event StateChangedHandler StateChanged = delegate { };
            private static string _filename = "";

            public static bool IsSaved {
                get; private set;
            }

            public static void ChangeState(object? sender, bool isSaved) {
                IsSaved = isSaved;
                StateChanged.Invoke(sender, isSaved);
            }

            public static string Filename { get => _filename; set => _filename = value; }
        }
    }
}
