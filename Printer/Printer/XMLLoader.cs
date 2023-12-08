using Leagueinator.CSSParser;
using System.Reflection;
using System.Xml.Linq;

namespace Leagueinator.Printer {

    public static class XMLLoaderExt {

        public static PrinterElement LoadResource(this Assembly assembly, string xml, string css) {
            using Stream? xmlStream = assembly.GetManifestResourceStream(xml) ?? throw new NullReferenceException(xml);
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlString = xmlReader.ReadToEnd();

            using Stream? cssStream = assembly.GetManifestResourceStream(css) ?? throw new NullReferenceException(css);
            using StreamReader cssReader = new StreamReader(cssStream);
            string cssString = cssReader.ReadToEnd();

            return XMLLoader.Load(xmlString, cssString);
        }
    }

    public class XMLLoader {
        Dictionary<string, NullableStyle> loadedStyles = new();
        PrinterElement rootElement = new();
        public float LoadTime { get; private set; } = -1f;

        public static PrinterElement LoadResource(string xml, string css) {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using Stream? xmlStream = assembly.GetManifestResourceStream(xml) ?? throw new NullReferenceException(xml);
            using StreamReader xmlReader = new StreamReader(xmlStream);
            string xmlString = xmlReader.ReadToEnd();

            using Stream? cssStream = assembly.GetManifestResourceStream(css) ?? throw new NullReferenceException(css);
            using StreamReader cssReader = new StreamReader(cssStream);
            string cssString = cssReader.ReadToEnd();

            return XMLLoader.Load(xmlString, cssString);
        }


        public static PrinterElement Load(string xmlString, string ssString) {
            var xmlLoader = new XMLLoader(xmlString, ssString);
            if (xmlLoader.Root == null) throw new NullReferenceException();
            return xmlLoader.Root;
        }

        public PrinterElement? Root {
            get => rootElement;
        }

        public XMLLoader(string xmlString, string ssString) {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            this.LoadXML(xmlString);
            this.loadedStyles = StyleLoader.Load(ssString);
            if (this.Root is not null) this.ApplyStyles(this.Root);

            sw.Stop();
            this.LoadTime = sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// MergeCSS current currentStyles with loaded currentStyles.
        /// Typically the current currentStyles are empty.
        /// </summary>
        public void ApplyStyles(PrinterElement root) {
            Queue<PrinterElement> queue = new ();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                PrinterElement current = queue.Dequeue();

                var nullableStyle = new NullableStyle();

                this.ApplyNameStyles(this.rootElement, nullableStyle);
                this.ApplyClassStyles(this.rootElement, nullableStyle);
                this.ApplyIDStyles(this.rootElement, nullableStyle);                
                this.ApplyWildcardStyles(this.rootElement, nullableStyle);

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
            if (this.loadedStyles.ContainsKey(current.Name)) {
                NullableStyle.MergeCSS(nStyle, loadedStyles[current.Name]);
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
            XElement xmlRoot = xml.Root;

            PrinterElement printRoot = new(xmlRoot.Attributes()) {
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

                    if (xmlChild is XElement element) {
                        var printChild = printCurrent.AddChild(new PrinterElement(element.Attributes()) {
                            Name = element.Name.ToString()
                        });
                        xmlStack.Push(element);
                        printStack.Push(printChild);
                        SeekClasses(element, printChild);
                    }
                    else if (xmlChild is XText text) {
                        printCurrent.AddChild(new TextElement(text.Value));
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
