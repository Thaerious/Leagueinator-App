
namespace Leagueinator.Printer.Styles {
    internal static class GraphicsExt {
        public static void FillRectangle(this Graphics g, Brush brush, FlexRect flexRect) {
            g.FillRectangle(brush, (RectangleF)flexRect);
        }
    }
}
