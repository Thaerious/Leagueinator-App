
using Leagueinator.Printer.Aspects;
using Leagueinator.Printer.Styles;
using System.Collections.ObjectModel;

namespace Leagueinator.Printer.Elements {

    public partial class Element : TreeNode<Element>{

        public Flex Style { 
            get; 
            private set; 
        }

        public Attributes Attributes { get; init; }

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

        private Element? _parent;
        private Dictionary<string, string> _attributes = [];
    }
}
