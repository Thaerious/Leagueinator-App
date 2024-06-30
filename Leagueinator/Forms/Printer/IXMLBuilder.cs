using Leagueinator.Printer.Elements;

namespace Leagueinator.Scoring {

    /// <summary>
    /// Pass this interface to PrinterForm instance.
    /// BuildElement should return the root element of the xml.
    /// </summary>
    public interface IXMLBuilder {
        public Element BuildElement();
    }
}
