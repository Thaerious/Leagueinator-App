using Leagueinator.Printer.Query;

namespace Leagueinator.Printer.Elements {
    public partial class Element {
        private QueryEngine? queryEngine = null;

        public List<Element> this[string query] {
            get {
                if (this.queryEngine == null) {
                    this.queryEngine = new QueryEngine();
                    this.queryEngine.AddAll(this);
                }

                return this.queryEngine[query];
            }
        }

        private void InvalidateQueryEngine() {
            Element? current = this;
            while (current is not null) {
                current.queryEngine = null;
                current = current.Parent;
            }
        }
    }
}
