using Leagueinator.Printer;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace Leagueinator.PrinterComponents
{
    public partial class PrinterCanvas : UserControl{

        private PrinterElement? _rootElement = null;
        public PrinterElement? RootElement { 
            get => this._rootElement;
            set {
                this._rootElement = value;

                if (this._rootElement != null) {
                    this._rootElement.ContainerProvider = new ContentRectProvider(() => new(0, 0, this.Width, this.Height));
                }
            }
        }

        [Category("Grid")]
        public int GridSize { get; set; } = 0;

        [Category("Grid")]
        public int SubGridSize { get; set; } = 0;


        [Category("Grid")]
        public bool ToBack { get; set; } = false;


        public PrinterCanvas() {
            this.InitializeComponent();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            this.RootElement?.Update();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);                        
            this.RootElement?.Draw(e.Graphics);
        }

        private void DrawGrids(Graphics g) {
            if (this.SubGridSize > 0f) {
                Pen pen = new Pen(Color.LightGray, 1) {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };
                this.DrawGrid(g, pen, this.SubGridSize);
            }
            if (this.GridSize > 0f) {
                this.DrawGrid(g, new Pen(Color.Gray, 1), this.GridSize);
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
