using Leagueinator.Printer.Elements;
using System.ComponentModel;
using System.Diagnostics;

namespace Leagueinator.Printer.Components {

    class InnerCanvas : Panel {

        public InnerCanvas(PrinterCanvas outer) {
            this.outer = outer;
            this.DoubleBuffered = true;
        }

        public float dimX = 0f;
        public float dimY = 0f;

        [Category("Inner")]
        public int GridSize { get; set; } = 0;

        [Category("Inner")]
        public int SubGridSize { get; set; } = 0;

        [Category("Inner")]
        public bool ToBack { get; set; } = false;

        public SizeF GridScale {
            get; set;
        } = new(1f, 1f);

        public int Page { get; set; } = 0;

        private Element? _rootElement = null;
        private PrinterCanvas outer;

        public Element? RootElement {
            get => this._rootElement;
            set {
                this._rootElement = value;
                if (this._rootElement == null) return;
                this.outer.SetDims(this._rootElement?.Style.Width, this._rootElement?.Style.Height);
            }
        }

        public TimeSpan RepaintTime { get; private set; }

        protected override void OnPaint(PaintEventArgs e) {
            if (this.RootElement is null) return;

            var stopwatch = new Stopwatch();

            base.OnPaint(e);
            e.Graphics.Clear(Color.White);
            e.Graphics.ScaleTransform(this.GridScale.Width, this.GridScale.Height);

            if (this.GridSize > 0 && this.ToBack) this.DrawGrids(e.Graphics);

            stopwatch.Start();
            this.RootElement?.Draw(e.Graphics, this.Page);
            stopwatch.Stop();

            if (this.GridSize > 0 && !this.ToBack) this.DrawGrids(e.Graphics);

            this.outer.InvokeRepaintTimer(stopwatch.Elapsed.TotalMilliseconds);
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
            var top = (int)(this.dimY);
            var right = (int)(this.dimX);

            for (int x = 0; x < this.dimX; x += size) {
                var p1 = new Point(x, 0);
                var p2 = new Point(x, top);
                g.DrawLine(pen, p1, p2);
            }
            for (int y = 0; y < this.dimY; y += size) {
                var p1 = new Point(0, y);
                var p2 = new Point(right, y);
                g.DrawLine(pen, p1, p2);
            }
        }
    }

    [DesignerCategory("")]
    public class PrinterCanvas : Panel {
        public delegate void RepaintTimer(double ms);
        public event RepaintTimer OnRepaintTime = delegate { };

        private InnerCanvas inner;

        public PrinterCanvas() {
            this.inner = new InnerCanvas(this);
            this.Controls.Add(this.inner);
            this.DoubleBuffered = true;
        }

        internal void InvokeRepaintTimer(double ms) {
            this.OnRepaintTime.Invoke(ms);
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        private void ResetDims() {
            this.SetDims(this.inner.dimX, this.inner.dimY);
        }

        public void SetDims(float x, float y) {
            this.inner.Dock = DockStyle.None;
            this.inner.dimX = x;
            this.inner.dimY = y;

            if (this.FitWidth().Height <= this.Height) {
                this.PinWidth();
            }
            else {
                this.PinHeight();
            }

            var scalex = this.inner.Width / this.inner.dimX;
            var scaley = this.inner.Height / this.inner.dimY;
            this.inner.GridScale = new(scalex, scaley);

            this.Invalidate(true);
        }

        private SizeF FitWidth() {
            return new(
                this.Width,
                (int)((this.Width / this.inner.dimX) * this.inner.dimY)
            );
        }

        /// <summary>
        /// Make the inner width the same as the outer (this) width.
        /// </summary>
        private void PinWidth() {
            this.inner.Width = this.Width;
            this.inner.Height = (int)((this.Width / this.inner.dimX) * this.inner.dimY);
            var top = (this.Height - this.inner.Height) / 2;
            this.inner.Location = new Point(0, top);
        }

        /// <summary>
        /// Make the inner height the same as the outer (this) height.
        /// </summary>
        private void PinHeight() {
            this.inner.Height = this.Height;
            this.inner.Width = (int)((double)this.Height * (this.inner.dimX / this.inner.dimY));
            var left = (this.Width - this.inner.Width) / 2;
            this.inner.Location = new Point(left, 0);
        }

        [Category("Inner")]
        public int GridSize {
            get => this.inner.GridSize;
            set => this.inner.GridSize = value;
        }

        [Category("Inner")]
        public int SubGridSize {
            get => this.inner.SubGridSize;
            set => this.inner.SubGridSize = value;
        }


        [Category("Inner")]
        public bool ToBack {
            get => this.inner.ToBack;
            set => this.inner.ToBack = value;
        }

        [Category("Inner")]
        public BorderStyle InnerBorder {
            get => this.inner.BorderStyle;
            set => this.inner.BorderStyle = value;
        }

        public int Page {
            get => this.inner.Page;
            set => this.inner.Page = value;
        }

        public Element? RootElement {
            get => this.inner.RootElement;
            set => this.inner.RootElement = value;
        }
        public TimeSpan RepaintTime { get => this.inner.RepaintTime; }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            this.ResetDims();

            if (this.RootElement != null) {
                this.inner.dimX = this.RootElement.Style.Width;
                this.inner.dimY = this.RootElement.Style.Height;
                this.RootElement.Style.DoLayout();
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
    }
}
