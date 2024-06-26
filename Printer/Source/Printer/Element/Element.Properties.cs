﻿using Leagueinator.Printer.Styles;
using Leagueinator.Utility;

namespace Leagueinator.Printer.Elements {

    public partial class Element : TreeNode<Element> {
        public Style Style { get; private set; }
        public Attributes Attributes { get; }
        public string TagName { get; init; } = "";

        public string Identifier {
            get {
                if (this.Attributes.TryGetValue("id", out string? value)) {
                    return this.TagName + "#" + value;
                }
                else if (this.ClassList.Count > 0) {
                    return this.TagName + "." + this.ClassList.DelString("."); ;
                }
                else {
                    return this.TagName;
                }
            }
        }

        public List<string> ClassList {
            get {
                if (this.Attributes.TryGetValue("class", out string? value)) {
                    return [.. (value ?? "").Split()];
                }
                return [];
            }
        }

        private readonly Dictionary<string, string> _attributes = [];
    }
}
