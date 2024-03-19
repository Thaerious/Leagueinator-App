namespace Leagueinator.Printer {
    public partial class Element {

        public Style Style { get; internal set; }

        public List<Element> Children => new(this._children);

        public Element? Parent { get; private set; }

        public bool IsRoot { get => this.Parent == null; }

        public Dictionary<string, string> Attributes { get; init; } = new();

        public string TagName { get; init; } = "";

        public string Identifier {
            get {
                if (this.Attributes.TryGetValue("id", out string? value)) {
                    return this.TagName + "#" + value;
                }
                else {
                    return this.TagName;
                }
            }
        }

        public List<string> ClassList {
            get {
                if (this.Attributes.TryGetValue("class", out string? value)) {
                    return [.. value.Split()];
                }
                return [];
            }
        }
    }
}
