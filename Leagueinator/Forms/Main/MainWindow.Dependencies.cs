using Leagueinator.Scoring;
using Leagueinator.Scoring.Plus;

namespace Leagueinator.Forms.Main {
    public partial class MainWindow {
        public IEventXMLBuilder ResultsXMLBuilder { get; set; } = new PointsPlusXMLBuilder();
    }
}
