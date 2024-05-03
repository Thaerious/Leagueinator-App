using Leagueinator.Printer.Styles;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Diagnostics;

namespace Leagueinator.Controls {
    public class PrinterImage : System.Windows.Controls.Image {

        public List<RenderNode> Pages {
            set => this.Bitmaps = CreateBitmaps(value);
        }

        public void SetPage(int index) {
            BitmapSource bitmapSource = ConvertBitmapToBitmapSource(this.Bitmaps[index]);
            this.Source = bitmapSource;            
        }

        public List<Bitmap> Bitmaps { get => this._bitmaps; private set => this._bitmaps = value; }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);
        }

        public static List<Bitmap> CreateBitmaps(List<RenderNode> pages) {
            List<Bitmap> list = [];

            int i = 0;
            foreach (RenderNode page in pages) {
                if ((int)page.Size.Width <= 0) throw new InvalidOperationException();
                if ((int)page.Size.Height <= 0) throw new InvalidOperationException();

                Bitmap bitmap = new Bitmap((int)page.Size.Width, (int)page.Size.Height);

                using Graphics graphics = Graphics.FromImage(bitmap);
                graphics.FillRectangle(System.Drawing.Brushes.White, 0, 0, (int)page.Size.Width, (int)page.Size.Height);
                page.Draw(graphics);

                list.Add(bitmap);
            }

            return list;
        }

        public static BitmapSource ConvertBitmapToBitmapSource(System.Drawing.Bitmap bitmap) {
            // Using GDI to create a pointer to the bitmap's data
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                bmpPt,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            // Release the HBitmap
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private List<Bitmap> _bitmaps = [];
    }
}
