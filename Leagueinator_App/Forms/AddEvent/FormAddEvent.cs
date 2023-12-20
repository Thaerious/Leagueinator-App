using Leagueinator_App;

namespace Leagueinator.App.Forms.AddEvent {
    public partial class FormAddEvent : Form {

        public EventSettings EventSettings {
            get {
                var setting = new EventSettings {
                    TeamSize = (int)this.spinTeamSize.Value,
                    LaneCount = (int)this.spinLaneCount.Value,
                    NumberOfEnds = (int)this.spinNumEnds.Value,
                    MatchSize = 2,
                    Date = this.dateTimePicker.Text,
                    Name = this.txtName.Text,
                };

                return setting;
            }

            set {
                this.txtName.Text = value.Name;
                this.dateTimePicker.Text = value.Date;
                this.spinTeamSize.Value = value.TeamSize;
                this.spinLaneCount.Value = value.LaneCount;
                this.spinNumEnds.Value = value.NumberOfEnds;
            }
        }

        public string EventName => this.txtName.Text;

        public string Date => this.dateTimePicker.Text;

        public FormAddEvent() {
            this.InitializeComponent();
        }
    }
}
