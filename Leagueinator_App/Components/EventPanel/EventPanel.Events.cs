using Leagueinator.Model;
using System;
using System.Diagnostics;
using System.Linq;

namespace Leagueinator.App.Components.EventPanel {
    public class EventPanelArgs {
        public readonly Round Round;
        public EventPanelArgs(Round round) {
            this.Round = round;
        }
    }

    public partial class EventPanel {
        public delegate void EventPanelAddRound(EventPanel source, EventPanelArgs args);
        public delegate void EventPanelDeleteRound(EventPanel source, EventPanelArgs args);
        public event EventPanelAddRound OnAddRound = delegate { };
        public event EventPanelDeleteRound OnDeleteRound = delegate { };

        public void HndAddRound(object sender, EventArgs e) {
            OnAddRound?.Invoke(this, new EventPanelArgs(this.CurrentRound));
        }

        public void HndDeleteRound(object sender, EventArgs e) {
            if (this.CurrentRound is null) return;
            OnDeleteRound?.Invoke(this, new EventPanelArgs(this.CurrentRound));
        }
    }
}
