using Leagueinator.Printer.Elements;
using Printer.Source;
using System.Reflection;

namespace Leagueinator.AssetControllers {
    internal class TeamScore : Element {
        public TeamScore() { }

        public static TeamScore New() {
            return Assembly.GetExecutingAssembly().LoadXMLResource<TeamScore>("Leagueinator.Assets.TeamScore.xml");
        }

        public void AddName(string text) {
            this["names"][0].AddChild(
                new Element {
                    TagName = "Name",
                    InnerText = text
                }
            );
        }

        public Element AddRow() {
            Element row = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.ScoreRow.xml");
            this["rounds"][0].AddChild(row);
            return row;
        }
    }
}
