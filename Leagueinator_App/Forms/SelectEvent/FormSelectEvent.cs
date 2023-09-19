using Leagueinator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Leagueinator.App.Forms.SelectEvent {
    public partial class FormSelectEvent : Form {
        public string Action = "";
        public LeagueEvent LeagueEvent = null;

        public FormSelectEvent() {
            this.InitializeComponent();
        }

        public void SetEvents(IEnumerable<LeagueEvent> events) {
            this.listEvents.Items.Clear();
            this.listEvents.Items.AddRange(
                events.Select(e => new LeagueEventWrapper { LeagueEvent = e }).ToArray()
            );
        }

        private void clickSelect(object sender, EventArgs e) {
            this.Action = "Select";
            this.LeagueEvent = (this.listEvents.SelectedItem as LeagueEventWrapper).LeagueEvent;
        }

        private void clickDelete(object sender, EventArgs e) {
            this.Action = "Delete";
            this.LeagueEvent = (this.listEvents.SelectedItem as LeagueEventWrapper).LeagueEvent;
        }
    }

    class LeagueEventWrapper {
        public LeagueEvent LeagueEvent;
        public override string ToString() {
            return this.LeagueEvent.Name + " " + this.LeagueEvent.Date;
        }
    }
}
