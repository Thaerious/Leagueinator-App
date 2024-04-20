using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Printer.Source;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace Leagueinator.AssetControllers {
    internal class EventScoreForm : Element {
        private static readonly LoadedStyles loadedStyles;

        static EventScoreForm() {
            loadedStyles = Assembly.GetExecutingAssembly().LoadStyleResource("Leagueinator.Assets.EventScoreForm.style");
        }

        public static EventScoreForm New() {
            EventScoreForm form =  Assembly.GetExecutingAssembly().LoadXMLResource<EventScoreForm>("Leagueinator.Assets.EventScoreForm.xml");
            loadedStyles.ApplyTo(form);
            return form;
        }

        public TeamScore AddTeam() {
            var teamScore = TeamScore.New();
            this["page"][0].AddChild(teamScore);
            return teamScore;
        }

        internal void DoLayout() {
            loadedStyles.ApplyTo(this);
        }
    }
}
