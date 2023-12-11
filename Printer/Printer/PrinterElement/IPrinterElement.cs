using Leagueinator.Utility;
using System.Drawing;

namespace Leagueinator.Printer {
    public interface IPrinterElement {
        PrinterElementList this[string query] { get; }

        Dictionary<string, string> Attributes { get; }
        RectangleF BorderRect { get; }
        SizeF BorderSize { get; set; }
        PrinterElementList Children { get; }
        List<string> ClassList { get; }
        RectangleF ContentRect { get; }
        SizeF ContentSize { get; set; }
        string? InnerText { get; set; }
        PointF Location { get; }
        RectangleF OuterRect { get; }
        SizeF OuterSize { get; set; }
        PrinterElement? Parent { get; }
        PointF Translation { get; set; }

        PrinterElement AddChild(PrinterElement child, bool applyStyle = true);
        PrinterElementList AddChildren(PrinterElementList children, bool applyStyle = true);
        void ClearChildren();
        PrinterElement Clone();
        void Draw(Graphics g);
        void RemoveChild(PrinterElement child);
        string ToString();
        XMLStringBuilder ToXML();
        void Translate(float x, float y);
        void Translate(PointF p);
        void Update();
    }
}
