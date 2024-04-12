using Leagueinator.Printer.Elements;
using System.Xml.Linq;

namespace Leagueinator.Printer {
    public class XMLLoader {
        public static Element Load(string xmlString) {
            XDocument xml = XDocument.Parse(xmlString);

            if (xml.Root == null) throw new NullReferenceException();
            XElement xElement = xml.Root;

            Element rootElement = new(xElement.Name.ToString(), xElement.Attributes());

            Stack<XElement> xmlStack = new Stack<XElement>();
            Stack<Element> eleStack = new Stack<Element>();
            xmlStack.Push(xElement);
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

            return rootElement;
        }
    }
}
