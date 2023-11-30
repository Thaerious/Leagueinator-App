using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Leagueinator.Utility {

    internal class InlineTag : OpenTag{
        public InlineTag(string name) : base(name) {}

        public override string ToString() {
            if (attributes.Count > 0) return $"<{this.Name} {attributes.DelString(" ")}/>";
            return $"<{this.Name}/>";
        }
    }

    internal class OpenTag {
        public readonly string Name = "";
        internal readonly List<string> attributes = new();

        public OpenTag(string name) {
            this.Name = name;
        }

        public void AddAttribute(string key, string value) {
            this.attributes.Add($"{key}='{value}'");
        }

        public void AddAttributes(string[] array) {
            this.attributes.AddRange(array);
        }

        public override string ToString() {
            if (attributes.Count > 0) return $"<{this.Name} {attributes.DelString(" ")}>";
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
        private readonly Stack<OpenTag> CurrentTag = new();

        private void AddLine(object obj, int indent) {
            this.lines.Add(new IndentedObject(obj, indent));
        }

        public XMLStringBuilder OpenTag(string name, params string[] attributes) {
            var openTag = new OpenTag(name);
            openTag.AddAttributes(attributes);
            this.AddLine(openTag, 1);
            this.CurrentTag.Push(openTag);
            return this;
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

        public XMLStringBuilder InlineTag(string tagname) {
            InlineTag inlineTag = new(tagname);
            this.AddLine(inlineTag, 0);
            return this;
        }
 
        public XMLStringBuilder CloseTag() {
            var openTag = this.CurrentTag.Pop();
            CloseTag closeTag = new(openTag.Name);
            this.AddLine(closeTag, -1);
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
