using Leagueinator.CSSParser;
using System.Drawing.Drawing2D;
using Leagueinator.Printer.Styles.Enums;
using Leagueinator.Printer.Aspects;

namespace Leagueinator.Printer.Styles {

    public partial class Style : IComparable<Style> {
        [CSS("Flex")] public Position? Position { get => this.position; [Validated] set => this.position = value; }
        [CSS("Visible")] public Overflow? Overflow { get => this.overflow; [Validated] set => this.overflow = value; }
        [CSS("0px")] public Cardinal<UnitFloat>? BorderSize {get => this.borderSize; [Validated] set => this.borderSize = value;}
        [CSS("0px, 0px")] public Coordinate<UnitFloat>? Translate { get => this.translate; [Validated] set => this.translate = value; }
        [CSS] public UnitFloat? Left { get => left; [Validated] set => left = value; }
        [CSS] public UnitFloat? Right { get => right; [Validated] set => right = value; }
        [CSS] public UnitFloat? Top { get => top; [Validated] set => top = value; }
        [CSS] public UnitFloat? Bottom { get => bottom; [Validated] set => bottom = value; }
        [CSS] public UnitFloat? Width { get => width; [Validated] set => width = value; }
        [CSS] public UnitFloat? Height { get => this.height; [Validated] set => this.height = value; }
        [CSS] public Color? MarginColor { get => marginColor; set => marginColor = value; }
        [CSS("0px")] public Cardinal<UnitFloat>? Margin { get => margin; [Validated] set => margin = value; }
        [CSS] public Color? PaddingColor { get => paddingColor; set => paddingColor = value; }
        [CSS("0px")] public Cardinal<UnitFloat>? Padding { get => this.padding; [Validated] set => this.padding = value; }
        [CSS("Solid")] public Cardinal<DashStyle>? BorderStyle { get => this.borderStyle; set => this.borderStyle = value; }
        [CSS] public Color? BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        [CSS] public Cardinal<Color>? BorderColor { get => this.borderColor; set => this.borderColor = value; }
        [CSS][Inherited] public string? FontFamily { get => fontFamily; [Validated] set => fontFamily = value; }
        [CSS][Inherited] public UnitFloat? FontSize { get => fontSize; [Validated] set => fontSize = value; }
        [CSS][Inherited] public FontStyle? FontStyle { get => this.fontStyle; set => this.fontStyle = value; }
        [CSS("Row")] public Flex_Axis? Flex_Axis { get => flex_Axis; [Validated] set => flex_Axis = value; }
        [CSS("Flex_Start")] public Justify_Content? Justify_Content { get => justify_Content; [Validated] set => justify_Content = value; }
        [CSS("Flex_Start")] public Align_Items? Align_Items { get => align_Items; [Validated] set => align_Items = value; }
        [CSS("Forward")] public Direction? Flex_Direction { get => this.flex_Direction; [Validated] set => this.flex_Direction = value; }

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
