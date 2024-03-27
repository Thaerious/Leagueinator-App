
namespace Leagueinator.Printer.Elements {
    public class Attributes {
        public readonly Element element;
        private Dictionary<string, string> attributes = [];

        public Attributes(Element element) {
            this.element = element;
        }

        public string this[string key] {
            get => this.attributes[key];
            set {
                this.element.Invalid = true;
                this.attributes[key] = value;
            }
        }

        public IEnumerable<string> Keys { 
            get => this.attributes.Keys; 
        }

        internal bool TryGetValue(string v, out string? value) {
            return attributes.TryGetValue(v, out value);
        }
    }
}
