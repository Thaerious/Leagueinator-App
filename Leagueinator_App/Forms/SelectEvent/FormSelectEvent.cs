
using System.Data;
using Model;

namespace Leagueinator.App.Forms.SelectEvent {
    public partial class FormSelectEvent : Form {
        public string Action = "";
        public LeagueEvent? LeagueEvent { get; private set; }

        public FormSelectEvent(IEnumerable<LeagueEvent> events) {       
            this.InitializeComponent();
            this.SetEvents(events);
        }

        private void SetEvents(IEnumerable<LeagueEvent> events) {
            this.listEvents.Items.Clear();
            this.listEvents.Items.AddRange(
                events.Select(e => new LeagueEventWrapper(e)).ToArray()
            );
        }

        private void ClickSelect(object sender, EventArgs e) {
            this.Action = "Select";
            if (this.listEvents.SelectedItem is not LeagueEventWrapper selected) return;
            this.LeagueEvent = selected.LeagueEvent;
        }

        private void ClickDelete(object sender, EventArgs e) {
            this.Action = "Delete";
            if (this.listEvents.SelectedItem is not LeagueEventWrapper selected) return;
            this.LeagueEvent = selected.LeagueEvent;
        }
    }

    class LeagueEventWrapper {
        public LeagueEvent LeagueEvent;

        public LeagueEventWrapper(LeagueEvent leagueEvent) {
            this.LeagueEvent = leagueEvent;
        }

        public override string ToString() {
            return this.LeagueEvent.EventName + " " + this.LeagueEvent.EventDate;
        }
    }
}
