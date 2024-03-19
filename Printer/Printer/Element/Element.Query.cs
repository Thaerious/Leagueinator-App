using Leagueinator.Printer.Query;
using System.Data;

namespace Leagueinator.Printer {
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
    }
}
