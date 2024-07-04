using Leagueinator.Model.Tables;
using Leagueinator.Printer.Elements;

namespace Leagueinator.Scoring {

    /// <summary>
    /// Used to print results to a PrinterForm instance.
    /// BuildElement should return the xml root element.
    /// </summary>
    public interface IEventXMLBuilder {
        public Element BuildElement(EventRow eventRow);
    }
}
