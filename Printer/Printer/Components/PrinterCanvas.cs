using Leagueinator.Printer;
using System.ComponentModel;

namespace Leagueinator.PrinterComponents {

    [DesignerCategory("")]
    public class PrinterCanvas : Panel {
        private Panel inner = new Panel();

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        public int Page { get; set; } = 0;

        public float dimX = 8.5f, dimY = 11f;
        public int dpi = 100;
        public float scaleX = 1.0f, scaleY = 1.0f;

        private Element? _rootElement = null;
        public Element? RootElement {
            get => this._rootElement;
            set {
                this._rootElement = value;
                if (this._rootElement == null) return;
                this._rootElement.ContainerRect = new(0, 0, this.Width, this.Height);
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

            if (this.RootElement != null) {
                this.RootElement.Style.DoLayout(this.RootElement);
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            e.Graphics.ScaleTransform(this.scaleX, this.scaleY);

            if (this.GridSize > 0 && this.ToBack) this.DrawGrids(e.Graphics);
            this.RootElement?.Style.Draw(e.Graphics, this.RootElement, this.Page);
            if (this.GridSize > 0 && !this.ToBack) this.DrawGrids(e.Graphics);
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

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
        }
    }
}
