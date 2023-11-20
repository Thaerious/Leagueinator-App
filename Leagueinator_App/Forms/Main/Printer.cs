using Leagueinator.Utility;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Security.Policy;
using static Leagueinator_App.PrintFormater.IPrinter;

namespace Leagueinator_App.PrintFormater {

    public enum Axis {
        VERTICAL, HORIZONTAL
    }

    public interface IPrinter {
        public delegate void DrawAction(IPrinter printer);

        public DrawAction OnDraw { set; }
        void DrawRectangle();
        void FillRectangle();
        void DrawString(string text);
        void Shrink(float amount);
        IPrinter Split(float[] sizes, Axis axis);
        IPrinter Split(int count, Axis axis);
    }

    public class PrinterList : List<Printer>, IPrinter {

        public DrawAction OnDraw {
            set {
                this.ForEach(printer => printer.OnDraw = value);
            }
        }

        public void DrawRectangle() {
            this.ForEach(printer => printer.DrawRectangle());
        }

        public void FillRectangle() {
            this.ForEach(printer => printer.FillRectangle());
            this.ForEach(printer => printer.DrawRectangle());
        }

        public void DrawString(string text) {
            this.ForEach(printer => printer.DrawString(text));
        }

        public void Shrink(float amount) {
            this.ForEach(printer => printer.Shrink(amount));
        }

        public IPrinter Split(float[] sizes, Axis axis) {
            PrinterList list = new PrinterList();
            this.ForEach(printer => {
                list.AddRange((PrinterList)printer.Split(sizes, axis));
            });
            return list;
        }

        public IPrinter Split(int count, Axis axis) {
            PrinterList list = new PrinterList();
            this.ForEach(printer => {
                list.AddRange((PrinterList)printer.Split(count, axis));
            });
            return list;
        }
    }

    public class Printer : IPrinter {
        public delegate PointF HasPointF();
        public readonly List<HasPointF> Translations = new();

        private PrinterList steps = new PrinterList();

        public Printer? Parent { get; private set; }
        public DrawAction OnDraw { get; set; } = p => { };

        public StringFormat StringFormat = new StringFormat {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public Axis FlexAxis = Axis.HORIZONTAL;

        public RectangleF ScreenRect {
            get {
                PointF loc = new PointF(0, 0);
                if (Parent != null) loc = new(Location.X + Parent.Location.X, Location.Y + Parent.Location.Y);
                return new RectangleF(loc, this.Size);
            }
        }

        public PointF Location { get {
                PointF location = new PointF(0, 0);
                foreach(HasPointF hasPoint in Translations) {
                    var point = hasPoint();
                    location = new PointF(location.X + point.X, location.Y + point.Y);
                }
                return location;
            }
        }

        public SizeF Size { get; set; } = new();

        public Font Font { get; set; } = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
        public Brush Brush { get; set; } = new SolidBrush(Color.Black);
        public Pen Pen { get; set; } = new Pen(Color.Black);

        public Graphics? Graphics { get; set; }

        public Printer(Printer source) {
            this.Font = source.Font;
            this.Brush = source.Brush;
            this.StringFormat = source.StringFormat;
            this.Size = source.Size;
            this.Pen = source.Pen;
            this.Parent = source;

            this.Translations.Add(() => source.Location);
        }

        public Printer(int x, int y, int pageWidth, int pageHeight) {
            this.Translations.Add(() => new(x, y));
            this.Size = new(pageWidth, pageHeight);
            this.Parent = null;
        }

        public void Run(Graphics g) {
            this.Graphics = g;
            this.OnDraw(this);
            foreach (Printer step in this.steps) {
                step.Run(g);
            }
        }

        public Printer Add(Printer that) {
            this.steps.Add(that);
            that.Parent = this;
            that.Graphics = this.Graphics;
            return that;
        }

        public void Add(PrinterList those) {
            foreach (var that in those) {
                this.steps.Add(that);
                that.Parent = this;
                that.Graphics = this.Graphics;
            }
        }

        public Printer AddChild(RectangleF rectangle) {
            var printer = new Printer(this) {
                Size = rectangle.Size,
            };
            printer.Translate(rectangle.X, rectangle.Y);
            this.Add(printer);
            return printer;
        }

        /**
         * AddChild a printer below the last child.
         * If there is no child printers then it is added to the top of 
         * the parent that (this).
         * 
         * The top of the new rectangle is the bottom of the last child.
         */
        public Printer AddBelow(Printer that) {
            if (this.steps.Count == 0) {
                that.Translate(() => new(0, this.ScreenRect.Top));
                Debug.WriteLine($"LocateBelow {that.ScreenRect}");
                this.Add(that);
            }
            else {
                var last = this.steps.Last();
                that.Translate(() => new(0, last.ScreenRect.Bottom));
                Debug.WriteLine($"LocateBelow {that.ScreenRect}");
                last.Add(that);
            }
            return that;
        }

        private void LocateBelow(Printer that) {
            if (this.steps.Count == 0) {
                that.Translate( () => new(0, this.ScreenRect.Top));
                Debug.WriteLine($"LocateBelow {that.ScreenRect}");
            }
            else {
                var last = this.steps.Last();
                that.Translate(() => new(0, last.ScreenRect.Bottom));
                Debug.WriteLine($"LocateBelow {that.ScreenRect}");
            }
        }

        /**
         * Create a new child with a rectangle.
         * The child area will be the same size as the parent.
         */
        public void DrawRectangle() {
            this.Graphics.DrawRectangle(this.Pen, this.ScreenRect);
        }

        public void FillRectangle() {
            this.Graphics.FillRectangle(this.Brush, this.ScreenRect);
            this.Graphics.DrawRectangle(this.Pen, this.ScreenRect);
        }

        public void DrawString(string? s) {
            this.Graphics.DrawString(s, Font, Brush, this.ScreenRect, StringFormat);
        }

        /**
         * Decrease the size of the print area.
         * Does not create a new print area.
         */
        public void Shrink(float amount) {
            //this.Location = new() {
            //    X = this.Location.X + amount,
            //    Y = this.Location.Y + amount,
            //};

            this.Size = new() { 
                Width = this.Size.Width - amount * 2,
                Height = this.Size.Height - amount * 2
            };
        }

        /**
        * Split the print area into child parts.
        */
        public IPrinter Split(float[] sizes, Axis axis) {
            PrinterList split = new();

            if (axis == Axis.HORIZONTAL) {
                float x = this.ScreenRect.Left;
                for (int i = 0; i < sizes.Length; i++) {
                    float dx = sizes[i];

                    var rect = new RectangleF() {
                        X = x,
                        Y = this.ScreenRect.Top,
                        Width = dx,
                        Height = this.ScreenRect.Height
                    };

                    this.Add(new Printer(this) {
                        Size = rect.Size,
                    }).Translate(rect.Location);
                    split.Add(this.steps.Last());

                    x += dx;
                }
            }
            else {
                float y = this.ScreenRect.Top;
                for (int i = 0; i < sizes.Length; i++) {
                    float dy = sizes[i];

                    var rect = new RectangleF() {
                        X = this.ScreenRect.Left,
                        Y = y,
                        Width = this.ScreenRect.Width,
                        Height = dy
                    };

                    this.Add(new Printer(this) {
                        Size = rect.Size,
                    }).Translate(rect.Location);
                    split.Add(this.steps.Last());

                    y += dy;
                }
            }

            return split;
        }

        /**
         * Split the print area into 'count' equal parts along the chosen 'axis'.
         * Creates an array of child print areas.
         */
        public IPrinter Split(int count, Axis axis) {
            PrinterList split = new();

            if (axis == Axis.HORIZONTAL) {
                float x = this.ScreenRect.Left;
                float dx = this.ScreenRect.Width / count;

                for (int i = 0; i < count; i++) {
                    var rect = new RectangleF() {
                        X = x + dx * i,
                        Y = this.ScreenRect.Top,
                        Width = dx,
                        Height = this.ScreenRect.Height
                    };

                    this.Add(new Printer(this) {
                        Size = rect.Size,
                    })
                    .Translate(rect.Location);
                }
            }
            else {
                float y = this.ScreenRect.Top;
                float dy = this.ScreenRect.Height / count;

                for (int i = 0; i < count; i++) {
                    var rect = new RectangleF() {
                        X = this.ScreenRect.Left,
                        Y = y + dy * i,
                        Width = this.ScreenRect.Width,
                        Height = dy
                    };

                    this.Add(new Printer(this) {
                        Size = rect.Size
                    }).Translate(rect.Location);
                }
            }

            return split.Last();
        }

        internal Printer AddChild(int x, int y, int pageWidth, int pageHeight) {
            return this.AddChild(new(x, y, pageWidth, pageHeight));
        }

        public void Translate(float x, float y) {
            this.Translations.Add(()=> new(x, y));
        }

        public void Translate(PointF p) {
            this.Translations.Add(() => new(p.X, p.Y));
        }
        public void Translate(HasPointF hasPointf) {
            this.Translations.Add(hasPointf);
        }
    }
}
