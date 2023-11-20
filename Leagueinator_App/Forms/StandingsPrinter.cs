using Leagueinator.Model;
using Leagueinator.Printer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator_App.Forms {
    internal class StandingsPrinter {
        private LeagueEvent lEvent;

        public StandingsPrinter(LeagueEvent lEvent) {
            this.lEvent = lEvent;
        }

        public void HndPrint(object sender, PrintPageEventArgs e) {
            e.HasMorePages = this.DrawNextPage(e.Graphics);
        }

        private bool DrawNextPage(Graphics? graphics) {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            var root = new PrinterElement() {
                Name = "root",
                Style = new Flex() {
                    Width = 1100f,
                    Height = 800f
                }
            };
            
            var styleSheet = new StandingsStyleSheet();

            var eventTable = lEvent.ToDataSet().Tables["event"];
            if (eventTable == null) throw new NullReferenceException(nameof(eventTable));

            foreach (String team in this.GetTeams(eventTable)) {
                var child = root.AddChild();
                child.AddChild(new TextElement(team));
            }

            Debug.WriteLine(root);

            styleSheet.ApplyTo(root);
            root.Draw(graphics);

            return false;
        }

        public List<string> GetTeams(DataTable eventTable) {
            List<string> teams = new List<string>();
            foreach (DataRow row in eventTable.Rows) {
                if (teams.Contains(row["team"])) continue;
                teams.Add((string)row["team"]);
            }
            return teams;
        }
    }
}
