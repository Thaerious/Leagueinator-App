using Leagueinator.Model;
using Leagueinator.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator_App.Forms {
    public class GenerateEventData {
        public static List<EventDatum> ByPlayer(LeagueEvent? lEvent) {
            List<EventDatum> eventData = new();
            if (lEvent == null) return eventData;

            lEvent.Players.ForEach(player => {
                eventData.Add(new EventDatum(lEvent, player));
            });

            eventData.Sort();
            eventData.Reverse();

            int rank = 1;
            eventData.ForEach(datum => {
                var prev = eventData.Prev(datum);

                if (prev == null || datum.BuildScore() == prev.BuildScore()) {
                    datum.Rank = rank;
                }
                else {
                    datum.Rank = ++rank;
                }
            });
            return eventData;
        }
    }
}
