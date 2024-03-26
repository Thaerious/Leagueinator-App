using System.Text;

namespace Leagueinator.Utility {

    internal class Tag {
        public Tag(string name) {
            this.Name = name;
        }

        public readonly string Name = "";
        internal readonly List<string> Attributes = new();

        public void AddAttribute(string key, string value) {
            this.Attributes.Add($"{key}='{value}'");
        }
    }

    internal class InlineTag(string name) : Tag(name) {
        public string Text = "";

        public override string ToString() {
            if (this.Text == "") {
                if (this.Attributes.Count > 0) return $"<{this.Name} {this.Attributes.DelString(" ")}/>";
                return $"<{this.Name}/>";
            }
            else {
                var s = "";
                if (this.Attributes.Count > 0) s += $"<{this.Name} {this.Attributes.DelString(" ")}>";
                s += this.Text;
                s += $"</{this.Name}>";
                return s;
            }
        }
    }

    internal class OpenTag : Tag {
        public OpenTag(string name) : base(name) { }

        public override string ToString() {
            if (this.Attributes.Count > 0) return $"<{this.Name} {this.Attributes.DelString(" ")}>";
            return $"<{this.Name}>";
        }
    }

    internal class CloseTag {
        public readonly string Name = "";

        public CloseTag(string name) {
            this.Name = name;
        }

        public override string ToString() {
            string text = $"</{this.Name}>";
            return text;
        }
    }

    public class XMLStringBuilder {
        public char Indent = '\t';
        private readonly List<IndentedObject> lines = new();
        private readonly Stack<Tag> CurrentTag = new();

        private void AddLine(object obj, int indent) {
            this.lines.Add(new IndentedObject(obj, indent));
        }

        public XMLStringBuilder OpenTag(string tagname) {
            var openTag = new OpenTag(tagname);
            this.AddLine(openTag, 1);
            this.CurrentTag.Push(openTag);
            return this;
        }

        public XMLStringBuilder Attribute(string key, object value) {
            var openTag = this.CurrentTag.Peek();
            openTag.AddAttribute(key, value.ToString() ?? "true");
            return this;
        }

        public XMLStringBuilder AppendLine(string text) {
            this.AddLine(text, 0);
            return this;
        }

        public XMLStringBuilder CloseTag() {
            var tag = this.CurrentTag.Pop();

            if (tag is InlineTag) {
                this.AddLine(tag, 0);
            }
            else {
                CloseTag closeTag = new(tag.Name);
                this.AddLine(closeTag, -1);
            }

            return this;

        }

        public XMLStringBuilder AppendXML(XMLStringBuilder xsb) {
            this.lines.AddRange(xsb.lines);
            return this;
        }

        public override string ToString() {
            int runningIndent = 0;
            var sb = new StringBuilder();
            foreach (IndentedObject iObj in this.lines) {
                if (iObj.indent < 0) runningIndent += iObj.indent;
                sb.AppendLine(iObj.ToString(runningIndent));
                if (iObj.indent > 0) runningIndent += iObj.indent;
            }
            return sb.ToString();
        }
    }

    internal class IndentedObject {
        public object target = "";
        public int indent = 0;
        public char c = '\t';

        public IndentedObject(object target, int indent) {
            this.target = target;
            this.indent = indent;
        }

        public string ToString(int runningIndent) {
            return $"{new string(this.c, runningIndent)}{this.target}";
        }
    }
}
