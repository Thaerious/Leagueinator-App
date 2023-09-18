using Leagueinator.Model;
using System;
using System.Diagnostics;
using System.Linq;

namespace Leagueinator.App.Components.EventPanel {

    public partial class EventPanel {
        public delegate void EventPanelAddRound(EventPanel source);
        public delegate void EventPanelDeleteRound(EventPanel source);
        public event EventPanelAddRound OnAddRound = delegate { };
        public event EventPanelDeleteRound OnDeleteRound = delegate { };
        
        public void HndAddRound(object sender, EventArgs e) {
            OnAddRound?.Invoke(this);
        }

        public void HndDeleteRound(object sender, EventArgs e) {
            if (this.CurrentRound is null) return;
            OnDeleteRound?.Invoke(this);
        }
    }
}
