using Leagueinator.CSSParser;
using Printer.Printer;
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
            var sw = Stopwatch.StartNew();

            this.LoadXML(xmlString);
            this.loadedStyles = StyleLoader.Load(ssString);
            this.ApplyStyles();

            sw.Stop();

            Debug.WriteLine($"XMLLoader elapsed time {sw.Elapsed.TotalMilliseconds} ms");
        }

        /// <summary>
        /// Merge current styles with loaded styles.
        /// Typically the current styles are empty.
        /// </summary>
        public void ApplyStyles() {
            this.ApplyStylesTo(this.rootElement);
        }

        void ApplyStylesTo(PrinterElement current) {
            if (this.loadedStyles.ContainsKey("*")) {
                current.Style.MergeWith(loadedStyles["*"]);
            }

            if (this.loadedStyles.TryGetValue(current.Name, out Style? value)) {
                current.Style.MergeWith(value);
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
