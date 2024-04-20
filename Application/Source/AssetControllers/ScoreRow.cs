using Leagueinator.Printer.Elements;
using Printer.Source;
using System.Reflection;

namespace Leagueinator.AssetControllers {
    internal class ScoreRow : Element {
        public ScoreRow() { }

        public static ScoreRow New() {
            return Assembly.GetExecutingAssembly().LoadXMLResource<ScoreRow>("Leagueinator.Assets.ScoreRow.xml");
        }
    }
}
