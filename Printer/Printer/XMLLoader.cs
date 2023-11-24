using Leagueinator.CSSParser;
using System.Diagnostics;
using System.Xml.Linq;

namespace Leagueinator.Printer {
    public class XMLLoader {
        Dictionary<string, Style> loadedStyles = new();
        PrinterElement rootElement = new();

        public static PrinterElement Load(string xmlString, string ssString) {
            var xmlLoader = new XMLLoader(xmlString, ssString);
            if (xmlLoader.Root == null) throw new NullReferenceException();
            return xmlLoader.Root;
        }

        public Dictionary<string, Style> Styles {
            get => loadedStyles;
        }

        public PrinterElement? Root {
            get => rootElement;
        }

        public XMLLoader(string xmlString, string ssString) {
            this.LoadXML(xmlString);
            this.LoadStyle(ssString);
            this.ApplyStyles();
        }

        /// <summary>
        /// Merge current styles with loaded styles.
        /// Typically the current styles are empty.
        /// </summary>
        public void ApplyStyles() {
            this.ApplyStylesTo(this.rootElement);
        }

        void ApplyStylesTo(PrinterElement current) {
            if (loadedStyles.ContainsKey(current.Name)) {
                current.Style.MergeWith(loadedStyles[current.Name]);
            }

            foreach (var className in current.ClassList) {
                if (loadedStyles.ContainsKey("." + className)) {
                    current.Style.MergeWith(loadedStyles["." + className]);
                }
            }

            foreach (PrinterElement child in current.Children) {
                ApplyStylesTo(child);
            }
        }

        public void LoadStyle(string ssString) {
            var styleSheet = StyleLoader.Load(ssString);
            if (styleSheet == null) throw new NullReferenceException(nameof(styleSheet));

            foreach (string key in styleSheet.Keys) {
                StyleDeclaration styleDeclaration = styleSheet[key];
                var flex = new Flex();
                styleDeclaration.ApplyTo(flex);
                loadedStyles[key] = flex;
            }
        }

        public PrinterElement LoadXML(string xmlString) {
            XDocument xml = XDocument.Parse(xmlString);

            if (xml.Root == null) throw new NullReferenceException();
            XElement xmlRoot = xml.Root;

            PrinterElement printRoot = new() {
                Name = xmlRoot.Name.ToString()
            };

            Stack<XElement> xmlStack = new Stack<XElement>();
            Stack<PrinterElement> printStack = new Stack<PrinterElement>();
            xmlStack.Push(xmlRoot);
            printStack.Push(printRoot);
            SeekClasses(xmlRoot, printRoot);

            while (xmlStack.Count > 0) {
                XElement xmlCurrent = xmlStack.Pop();
                PrinterElement printCurrent = printStack.Pop();

                foreach (var xmlChild in xmlCurrent.Nodes().ToList()) {
                    if (xmlChild is null) continue;

                    if (xmlChild is XElement) {
                        var printChild = printCurrent.AddChild(new PrinterElement() {
                            Name = ((XElement)xmlChild).Name.ToString()
                        });
                        xmlStack.Push((XElement)xmlChild);
                        printStack.Push(printChild);
                        SeekClasses((XElement)xmlChild, printChild);
                    }
                    else if (xmlChild is XText) {
                        printCurrent.AddChild(new TextElement(((XText)xmlChild).Value));
                    }
                }
            }

            this.rootElement = printRoot;
            return printRoot;
        }

        private static void SeekClasses(XElement xml, PrinterElement pele) {
            XAttribute? attr = xml.Attribute("class");
            if (attr == null) return;
            string[] strings = attr.Value.Split(" ");
            foreach (var s in strings) pele.ClassList.Add(s);
        }
    }
}
