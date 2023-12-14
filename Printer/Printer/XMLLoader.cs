using Leagueinator.CSSParser;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

namespace Leagueinator.Printer {
    public class XMLLoader {
        Dictionary<string, NullableStyle> loadedStyles = new();

        public void LoadStyleResource(Assembly assembly, string resourceName) {
            using Stream? styleStream = assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException($"Resource Not Found: {resourceName}");
            using StreamReader styleReader = new StreamReader(styleStream);
            string styleText = styleReader.ReadToEnd();
            this.LoadStyle(styleText);
        }

        public PrinterElement LoadXMLResource(Assembly assembly, string resourceName) {
            using Stream? xmlStream = assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException($"Resource Not Found: {resourceName}");
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlText = xmlReader.ReadToEnd();
            return this.LoadXML(xmlText);
        }

        public void LoadStyle(string styleString) {
            Dictionary<string, NullableStyle> style = StyleLoader.Load(styleString);

            foreach (var keyPair in style) {
                this.loadedStyles[keyPair.Key] = keyPair.Value;
            }
        }

        /// <summary>
        /// Using the loaded style sheet, apply styles to root and all child nodes
        /// recursivly.  Replaces the current style on the element.
        /// </summary>
        public void ApplyStyles(PrinterElement root) {
            Queue<PrinterElement> queue = new();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                PrinterElement current = queue.Dequeue();
                var nullableStyle = new NullableStyle();

                this.ApplyNameStyles(current, nullableStyle);
                this.ApplyClassStyles(current, nullableStyle);
                this.ApplyIDStyles(current, nullableStyle);
                this.ApplyWildcardStyles(current, nullableStyle);

                //nullableStyle.MergeInheritedCSS(current.ContainerProvider.Style);

                current.Style = nullableStyle.ToStyle<Flex>();

                foreach (PrinterElement child in current.Children) {
                    queue.Enqueue(child);
                }
            }
        }

        internal void ApplyClassStyles(PrinterElement current, NullableStyle nStyle) {
            foreach (var className in current.ClassList) {
                if (loadedStyles.ContainsKey("." + className)) {
                    NullableStyle.MergeCSS(nStyle, loadedStyles["." + className]);
                }
            }
        }

        internal void ApplyIDStyles(PrinterElement current, NullableStyle nStyle) {
            if (current.Attributes.ContainsKey("id")) {
                var selector = "#" + current.Attributes["id"];
                if (loadedStyles.ContainsKey(selector)) {
                    NullableStyle.MergeCSS(nStyle, loadedStyles[selector]);
                }
            }
        }

        internal void ApplyNameStyles(PrinterElement current, NullableStyle nStyle) {
            if (this.loadedStyles.ContainsKey(current.TagName)) {
                NullableStyle.MergeCSS(nStyle, loadedStyles[current.TagName]);
            }
        }

        internal void ApplyWildcardStyles(PrinterElement current, NullableStyle nStyle) {
            if (this.loadedStyles.ContainsKey("*")) {
                NullableStyle.MergeCSS(nStyle, loadedStyles["*"]);
            }
        }

        public PrinterElement LoadXML(string xmlString) {
            XDocument xml = XDocument.Parse(xmlString);

            if (xml.Root == null) throw new NullReferenceException();
            XElement xElement = xml.Root;

            PrinterElement rootElement = new(xElement.Attributes()) {
                TagName = xElement.Name.ToString()
            };

            Stack<XElement> xmlStack = new Stack<XElement>();
            Stack<PrinterElement> printStack = new Stack<PrinterElement>();
            xmlStack.Push(xElement);
            printStack.Push(rootElement);

            while (xmlStack.Count > 0) {
                XElement xmlCurrent = xmlStack.Pop();
                PrinterElement printCurrent = printStack.Pop();

                foreach (var xmlChild in xmlCurrent.Nodes().ToList()) {
                    if (xmlChild is null) continue;

                    if (xmlChild is XElement element) {
                        var printChild = printCurrent.AddChild(new PrinterElement(element.Attributes()) {
                            TagName = element.Name.ToString()
                        });
                        xmlStack.Push(element);
                        printStack.Push(printChild);
                    }
                    else if (xmlChild is XText text) {
                        printCurrent.AddChild(new TextElement(text.Value));
                    }
                }
            }

            this.ApplyStyles(rootElement);
            return rootElement;
        }
    }
}
