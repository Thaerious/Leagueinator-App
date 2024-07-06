using Leagueinator.Extensions;
using Leagueinator.Forms.Main;
using Leagueinator.Model;
using Leagueinator.Model.Tables;
using Leagueinator.Scoring;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Leagueinator.Forms {

    /// <summary>
    /// The list of available events.
    /// </summary>
    public class EventsListCard : Grid {
        public EventsListCard(EventRow eventRow) {
            this.EventRow = eventRow;

            this.ColumnDefinitions.Add(new() {
                Width = new GridLength(1, GridUnitType.Star)
            });

            this.ColumnDefinitions.Add(new() {
                Width = new GridLength(1, GridUnitType.Star)
            });

            Label nameLabel = new() {
                Name = "LabelName",
                Content = eventRow.Name
            };

            Binding nameBinding = new Binding("Name") {
                Source = eventRow,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            nameLabel.SetBinding(Label.ContentProperty, nameBinding);
            this.Children.Add(nameLabel);

            Label dateLabel = new() {
                Name = "LabelDate",
                Content = eventRow.Date
            };

            Binding dateBinding = new Binding("Date") {
                Source = eventRow,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            dateLabel.SetBinding(Label.ContentProperty, dateBinding);
            this.Children.Add(dateLabel);

            Grid.SetColumn(this.Children[0], 0);
            Grid.SetColumn(this.Children[1], 1);

            this.MouseEnter += this.HndMouseEnter;
            this.MouseLeave += this.HndMouseLeave;
        }

        private void HndMouseLeave(object sender, MouseEventArgs e) {
            if (this.IsSelected) this.Background = this.selectedBrush;
            else this.Background = null;
            Mouse.OverrideCursor = null;
        }

        private void HndMouseEnter(object sender, MouseEventArgs e) {
            this.Background = this.hoverBrush;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        public bool IsSelected {
            get => this._isSelected;
            set {
                this._isSelected = value;
                if (value) this.Background = this.selectedBrush;
                else this.Background = null;
            }
        }

        public EventRow EventRow { get; }
        private bool _isSelected;
        private SolidColorBrush selectedBrush = new SolidColorBrush(Colors.LightCyan);
        private SolidColorBrush hoverBrush = new SolidColorBrush(Colors.LightBlue);
    }


    /// <summary>
    /// Interaction logic for EventManager.xaml
    /// </summary>
    public partial class EventManager : Window {
        public EventManager(MainWindow mainWindow) {
            this.MainWindow = mainWindow;
            this.InitializeComponent();
            this.InitializePanels();

            // Default select last available event.
            if (this.League.EventTable.Rows.Count > 0) {
                this.Selected = (EventsListCard?)this.NamePanel.Children[^1];
            }
        }

        private void InitializePanels() {
            ScoringFormat scoringFormat = this.League.LeagueSettingsTable.Get<ScoringFormat>("ScoringFormat");

            var tagSource = this.ScoringFormatOptions;

            RadioButton radioButton = this.ScoringFormatOptions.FindChildByTag<RadioButton>(scoringFormat.ToString()) ?? throw new NullReferenceException();
            radioButton.IsChecked = true;

            this.PopulateNamePanel();
        }

        public MainWindow MainWindow { get; }

        public League League { get => this.MainWindow.League; }

        private void PopulateNamePanel() {
            foreach (EventRow eventRow in this.League.EventTable) {
                EventsListCard card = new(eventRow);
                this.NamePanel.Children.Add(card);
                card.MouseDown += this.HndCardMouseDown;
            }
        }

        private void HndCardMouseDown(object sender, MouseButtonEventArgs e) {
            if (sender is not EventsListCard card) return;
            this.Selected = card;
        }

        private void HndClickNew(object sender, RoutedEventArgs args) {
            string eventName = "Default Event";

            int i = 1;
            while (this.League.Events.Has(eventName)) {
                eventName = $"Default Event {i++}";
            }

            EventRow eventRow = this.League.Events.Add(eventName);

            EventsListCard card = new(eventRow);
            this.NamePanel.Children.Add(card);
            card.MouseDown += this.HndCardMouseDown;
            this.Selected = card;
        }

        private void HndClickOpen(object sender, RoutedEventArgs args) {
            this.DialogResult = true;
        }

        private void HndClickDelete(object sender, RoutedEventArgs args) {
            if (this.Selected is null) return;
            this.Selected.EventRow.Remove();
            this.NamePanel.Children.Remove(this.Selected);
            this.Selected = null;
        }

        private void HndClickCancel(object sender, RoutedEventArgs args) {
            this.DialogResult = false;
        }

        private void HndEndsChanged(object sender, RoutedEventArgs args) {
            if (this.Selected is null) return;
            int ends = int.Parse(this.TxtEnds.Text);
            this.Selected.EventRow.EndsDefault = ends;
        }

        private void HndLaneChanged(object sender, RoutedEventArgs args) {
            if (this.Selected is null) return;
            int lanes = int.Parse(this.TxtLanes.Text);
            this.Selected.EventRow.LaneCount = lanes;
        }

        private void HndTourneyFormatChecked(object sender, RoutedEventArgs args) {
            if (this.Selected is null) return;
            if (sender is not RadioButton radioButton) return;

            this.Selected.EventRow.EventFormat = (EventFormat)Enum.Parse(typeof(EventFormat), (string)radioButton.Tag);
        }

        private void HndScoringChecked(object sender, RoutedEventArgs args) {
            if (sender is not RadioButton radioButton) return;
            var scoringFormat = (ScoringFormat)Enum.Parse(typeof(ScoringFormat), (string)radioButton.Tag);
            this.MainWindow.ScoringFormat = scoringFormat;
        }

        public EventsListCard? Selected {
            get => this._selected;
            set {
                if (this._selected is not null) {
                    this._selected.IsSelected = false;
                }

                this._selected = value;

                if (value == null) {
                    this.DatePicker.DataContext = null;
                    this.TxtEventName.DataContext = null;
                    this.OptionPanel.DataContext = null;
                    this.ButDelete.IsEnabled = false;
                    this.ButOpen.IsEnabled = false;
                }
                else {
                    value.IsSelected = true;
                    if (this.League.EventTable.Rows.Count > 1) this.ButDelete.IsEnabled = true;

                    this.TxtEnds.Text = this.Selected?.EventRow.EndsDefault.ToString();
                    this.TxtLanes.Text = this.Selected?.EventRow.LaneCount.ToString();

                    this.ButOpen.IsEnabled = true;
                    this.DatePicker.DataContext = value.EventRow;
                    this.TxtEventName.DataContext = value.EventRow;
                    //this.OptionPanel.DataContext = scoringFormat.EventRow.Settings; // TODO
                }
            }
        }

        private EventsListCard? _selected = null;
        private League _league = new();
    }
}
