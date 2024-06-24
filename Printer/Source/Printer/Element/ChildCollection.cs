using System.Collections.ObjectModel;

namespace Leagueinator.Printer.Elements {
    public class ChildCollection(IList<Element> list) : ReadOnlyCollection<Element>(list) {
    }
}
