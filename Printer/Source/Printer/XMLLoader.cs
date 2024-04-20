using Leagueinator.Printer.Elements;
using System.Xml.Linq;

namespace Leagueinator.Printer {
    public class XMLLoader {

        public static void Load<T>(string xmlString, out T rootElement) where T : Element, new() {
            XDocument xml = XDocument.Parse(xmlString);

            if (xml.Root == null) throw new NullReferenceException();
            XElement xmlElement = xml.Root;

            rootElement = new() {
                TagName = xmlElement.Name.ToString()
            };

            Stack<XElement> xmlStack = new Stack<XElement>();
            Stack<Element> eleStack = new Stack<Element>();
            xmlStack.Push(xmlElement);
            eleStack.Push(rootElement);

            while (xmlStack.Count > 0) {
                XElement xmlCurrent = xmlStack.Pop();
                Element eleCurrent = eleStack.Pop();

                foreach (var xmlChild in xmlCurrent.Nodes().ToList()) {
                    if (xmlChild is null) continue;

                    if (xmlChild is XElement xChild) {
                        Element childElement = new Element(xChild.Name.ToString(), xChild.Attributes()) {
                            TagName = xChild.Name.ToString(),
                        };

                        eleCurrent.AddChild(childElement);
                        xmlStack.Push(xChild);
                        eleStack.Push(childElement);
                    }
                    else if (xmlChild is XText xText) {
                        string[] lines = xText.Value.Split("\n");
                        foreach (string line in lines) {
                            Element childElement = new TextElement(line.Trim());
                            eleCurrent.AddChild(childElement);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an element tree from a source string.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns>The root element</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static Element Load(string xmlString) {
            Load(xmlString, out Element element);
            return element;
        }
    }
}
