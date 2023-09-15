using System.Text;

namespace Leagueinator.Utility {
    public class XMLStringBuilder {
        private readonly StringBuilder sb = new();
        private int Depth => this.CurrentTag.Count;
        public char Indent = '\t';
        private readonly List<IndentedObject> lines = new();
        private readonly Stack<string> CurrentTag = new();

        public XMLStringBuilder OpenTag(string name, params string[] attributes) {
            string text = $"<{name} {attributes.DelString(" ")}>";
            this.lines.Add(new IndentedString(this.Depth, text));
            this.CurrentTag.Push(name);
            return this;
        }

        public XMLStringBuilder OpenTag(string name) {
            string text = $"<{name}>";
            this.lines.Add(new IndentedString(this.Depth, text));
            this.CurrentTag.Push(name);
            return this;
        }

        public XMLStringBuilder AppendLine(string text) {
            this.lines.Add(new IndentedString(this.Depth, text));
            return this;
        }

        public XMLStringBuilder AppendLines(string text) {
            string[] lines = text.Split('\n');

            foreach (string line in lines) {
                _ = this.AppendLine(line);
            }
            return this;
        }

        public XMLStringBuilder InlineTag(string tag, string text = "") {
            string t = text == "" ? $"<{tag}/>" : $"<{tag}>{text}</{tag}>";
            this.lines.Add(new IndentedString(this.Depth, t));
            return this;
        }

        public XMLStringBuilder InlineTag(string tag, string text, params string[] attributes) {
            string t = $"<{tag} {attributes.DelString(" ")}>{text}</{tag}>";
            this.lines.Add(new IndentedString(this.Depth, t));
            return this;
        }

        public XMLStringBuilder CloseTag(string name) {
            string text = $"</{name}>";
            this.lines.Add(new IndentedString(this.Depth, text));
            return this;
        }

        public XMLStringBuilder CloseTag() {
            string text = $"</{this.CurrentTag.Pop()}>";
            this.lines.Add(new IndentedString(this.Depth, text));
            return this;
        }

        public XMLStringBuilder AppendXML(XMLStringBuilder xsb) {
            foreach (IndentedString iString in xsb.lines) {
                this.lines.Add(new IndentedString(iString.indent + this.Depth, iString.text));
            }
            return this;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            foreach (IndentedString iString in this.lines) {
                _ = sb.AppendLine(iString.ToString());
            }
            return sb.ToString();
        }
    }

    internal class IndentedObject {
        public object target = "";
        public int indent = 0;
        public char c = '\t';

        public IndentedObject(int indent, object target) {
            this.indent = indent;
            this.target = target;
        }

        public override string ToString() {
            return $"{new string(this.c, this.indent)}{this.target}";
        }
    }

    internal class IndentedString : IndentedObject {

#pragma warning disable CS8603 // Possible null reference return.
        public string text => this.target.ToString();
#pragma warning restore CS8603 // Possible null reference return.

        public IndentedString(int indent, string text) : base(indent, text) { }
    }
}
