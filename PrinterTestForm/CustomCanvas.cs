using Leagueinator.Printer;
using System.Diagnostics;
using System.Reflection;
using static Leagueinator.Printer.PrinterElement;

namespace PrinterTestForm {
    public partial class PrinterCanvas : UserControl {
        public PrinterElement? RootElement { get; set; }

        public PrinterCanvas() {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            if (this.RootElement is not null) {
                this.RootElement.Style.Width = this.Width;
                this.RootElement.Style.Height = this.Height;
                this.RootElement.Update();
                this.RootElement.Draw(e.Graphics);
            }

            var pen = new Pen(Color.Gray, 1);
            for (int x = 0; x < this.Width; x += 25){
                var p1 = new Point(x, 0);
                var p2 = new Point(x, this.Height);
                e.Graphics.DrawLine(pen, p1, p2);
            }
            for (int y = 0; y < this.Height; y += 25) {
                var p1 = new Point(0, y);
                var p2 = new Point(this.Width, y);
                e.Graphics.DrawLine(pen, p1, p2);
            }
        }

    }
}
