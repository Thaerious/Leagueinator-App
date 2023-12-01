using Leagueinator.Printer;
using System.ComponentModel;

namespace Leagueinator.PrinterComponents
{
    public partial class PrinterCanvas : UserControl{
        public RootElement DocElement { get; }

        [Category("Grid")]
        public int GridSize { get; set; } = 0;

        [Category("Grid")]
        public int SubGridSize { get; set; } = 0;

        public PrinterCanvas() {
            this.InitializeComponent();
            this.DocElement = new RootElement(() => new SizeF(this.Width, this.Height));
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            this.DocElement.Update();
            this.DocElement.Draw(e.Graphics);

            if (this.SubGridSize > 0f) {
                Pen pen = new Pen(Color.LightGray, 1) {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };
                this.DrawGrid(e.Graphics, pen, this.SubGridSize);
            }
            if (this.GridSize > 0f) {
                this.DrawGrid(e.Graphics, new Pen(Color.Gray, 1), this.GridSize);
            }
        }

        private void DrawGrid(Graphics g, Pen pen, int size) {
            for (int x = 0; x < this.Width; x += size) {
                var p1 = new Point(x, 0);
                var p2 = new Point(x, this.Height);
                g.DrawLine(pen, p1, p2);
            }
            for (int y = 0; y < this.Height; y += size) {
                var p1 = new Point(0, y);
                var p2 = new Point(this.Width, y);
                g.DrawLine(pen, p1, p2);
            }
        }
    }
}
