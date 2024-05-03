using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Printer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Leagueinator.Forms {
    internal class ResultBuilder {

        /// <summary>
        /// Build a XML element with a results summary organized by team.
        /// </summary>
        /// <param name="eventRow"></param>
        /// <returns></returns>
        public static Element BuildElement(EventRow eventRow) {
            LoadedStyles styles = Assembly.GetExecutingAssembly().LoadStyleResource("Leagueinator.Assets.EventScoreForm.style");
            Element docroot = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.EventScoreForm.xml");
            Element teamXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.TeamScore.xml");
            Element rowXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.ScoreRow.xml");

            var allTeams = eventRow.AllTeams();

            foreach (var pair in allTeams) {
                Element xmlFragment = teamXML.Clone();

                // Add names to the xml fragment.
                foreach (string name in pair.Key.Players) {
                    xmlFragment["names"][0].AddChild(
                        new Element {
                            TagName = "Name",
                            InnerText = name
                        }
                    );
                }

                // Add match summaries to the xml fragment.
                foreach (MatchResults match in pair.Value) {
                    Element row = rowXML.Clone();

                    row["index"][0].InnerText = (match.Round + 1).ToString();
                    row["lane"][0].InnerText = (match.Lane + 1).ToString();

                    row["bowls_for"][0].InnerText = match.BowlsFor.ToString();
                    row["bowls_against"][0].InnerText = match.BowlsAgainst.ToString();
                    row["tie"][0].InnerText = match.Tie.ToString();
                    row["score_for"][0].InnerText = $"{match.PointsFor}+{match.PlusFor}";

                    row["score_against"][0].InnerText = $"{match.PointsAgainst}+{match.PlusAgainst}";
                    row["ends_played"][0].InnerText = match.Ends.ToString();

                    xmlFragment["rounds"][0].AddChild(row);
                }

                // Add fragment to the root.
                docroot["page"][0].AddChild(xmlFragment);
            }

            styles.AssignTo(docroot);
            return docroot;
        }

    }
}
