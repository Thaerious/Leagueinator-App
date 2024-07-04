using Leagueinator.Model.Tables;
using Leagueinator.Printer.Elements;

namespace Leagueinator.Scoring {
    public interface IRoundXMLBuilder {
        public Element BuildElement(RoundRow roundRow);
    }
}
