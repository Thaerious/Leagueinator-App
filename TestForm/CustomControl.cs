using Leagueinator.Printer;
using System.Diagnostics;

namespace TestForm {
    public partial class CustomControl : UserControl {
        public CustomControl() {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            Debug.WriteLine("Call the OnPaint method of the base class.");
            base.OnPaint(e);

            PrinterElement printer = new PrinterElement(this.Width, this.Height);

            printer.OnDraw = p => {
                p.FillRectangle(Color.LightGray);
            };

            printer.Style.Flex_Direction = Flex_Direction.row;
            printer.Style.Justify_Content = Justify_Content.center;
            printer.Style.Align_Items = Align_Items.center;
            printer.Style.Display = Display.flex;

            printer.OnDraw = p => {
                p.DrawRectangle(new Pen(Color.Black, 5));
                p.FillRectangle(Color.SkyBlue);
            };

            var container = printer.Add(new PointF(200, 500));
            container.Style.Flex_Direction = Flex_Direction.column_reverse;
            container.Style.Justify_Content = Justify_Content.flex_start;
            //container.Style.Align_Items = Align_Items.center;
            //container.Style.Display = Display.flex;
            container.OnDraw = p => {
                p.DrawRectangle(new Pen(Color.Black, 5));
                p.FillRectangle(Color.Purple);
            };

            container.Add(new PrintBox("1"));
            container.Add(new PrintBox("2"));
            container.Add(new PrintBox("3"));
            container.Add(new PrintBox("4"));

            printer.Run(e.Graphics);
        }
    }
}
