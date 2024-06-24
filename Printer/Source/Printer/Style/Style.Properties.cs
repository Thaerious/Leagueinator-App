using Leagueinator.CSSParser;
using Leagueinator.Printer.Aspects;
using Leagueinator.Printer.Styles.Enums;
using System.Drawing.Drawing2D;

namespace Leagueinator.Printer.Styles {

    public partial class Style : IComparable<Style> {
        [CSS("Flex")] public Position? Position { get => this.position; set => this.position = value; }
        [CSS("Visible")] public Overflow? Overflow { get => this.overflow; set => this.overflow = value; }
        [CSS("0px")] public Cardinal<UnitFloat>? BorderSize { get => this.borderSize; set => this.borderSize = value; }
        [CSS("0px, 0px")] public Coordinate<UnitFloat>? Translate { get => this.translate; set => this.translate = value; }
        [CSS] public UnitFloat? Left { get => left; set => left = value; }
        [CSS] public UnitFloat? Right { get => right; set => right = value; }
        [CSS] public UnitFloat? Top { get => top; set => top = value; }
        [CSS] public UnitFloat? Bottom { get => bottom; set => bottom = value; }
        [CSS("auto")] public UnitFloat? Width { get => width; set => width = value; }
        [CSS("auto")] public UnitFloat? Height { get => this.height; set => this.height = value; }
        [CSS] public Color? MarginColor { get => marginColor; set => marginColor = value; }
        [CSS("0px")] public Cardinal<UnitFloat>? Margin { get => margin; set => margin = value; }
        [CSS] public Color? PaddingColor { get => paddingColor; set => paddingColor = value; }
        [CSS("0px")] public Cardinal<UnitFloat>? Padding { get => this.padding; set => this.padding = value; }
        [CSS("Solid")] public Cardinal<DashStyle>? BorderStyle { get => this.borderStyle; set => this.borderStyle = value; }
        [CSS] public Color? BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        [CSS] public Cardinal<Color>? BorderColor { get => this.borderColor; set => this.borderColor = value; }
        [CSS][Inherited] public string? FontFamily { get => fontFamily; set => fontFamily = value; }
        [CSS("1px")][Inherited] public UnitFloat? FontSize { get => fontSize; set => fontSize = value; }
        [CSS][Inherited] public FontStyle? FontStyle { get => this.fontStyle; set => this.fontStyle = value; }
        [CSS("Row")] public Flex_Axis? Flex_Axis { get => flex_Axis; set => flex_Axis = value; }
        [CSS("Flex_Start")] public Justify_Content? Justify_Content { get => justify_Content; set => justify_Content = value; }
        [CSS("Stretch")] public Align_Items? Align_Items { get => align_Items; set => align_Items = value; }
        [CSS("Forward")] public Direction? Flex_Direction { get => this.flex_Direction; set => this.flex_Direction = value; }

        [CSS]
        public string Border {
            get => $"{this.BorderSize} {this.BorderStyle} {this.BorderColor}";
            [Validated]
            set => this.SetBorder(value);
        }

        private Position? position = null;
        private Overflow? overflow = null;

        private Coordinate<UnitFloat>? translate = null;
        private UnitFloat? left = null;
        private UnitFloat? right = null;
        private UnitFloat? top = null;
        private UnitFloat? bottom = null;
        private UnitFloat? width = null;
        private UnitFloat? height = null;

        private Color? backgroundColor = null;
        private Color? marginColor = null;
        private Color? paddingColor = null;

        private Cardinal<UnitFloat>? margin = null;
        private Cardinal<UnitFloat>? padding = null;

        private Cardinal<DashStyle>? borderStyle;
        private Cardinal<Color>? borderColor = null;
        private Cardinal<UnitFloat>? borderSize = null;

        private string? fontFamily = null;
        private UnitFloat? fontSize = null;
        private FontStyle? fontStyle = null;

        private Flex_Axis? flex_Axis = null;
        private Justify_Content? justify_Content = null;
        private Align_Items? align_Items = null;
        private Direction? flex_Direction = null;
    }
}
