namespace Leagueinator.Printer {
    public class ElementList : List<Element> {

        public ElementList() { }

        public ElementList(IEnumerable<Element> collection) : base(collection) { }
    }
}
