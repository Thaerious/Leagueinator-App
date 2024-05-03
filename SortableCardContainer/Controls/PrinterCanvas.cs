using Leagueinator.Printer.Styles;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using Image = System.Windows.Controls.Image;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace Leagueinator.Controls {
    public class PrinterCanvas : Canvas {

        public List<RenderNode> Pages {
            set => this.Bitmaps = CreateBitmaps(value);
        }

        public void SetPage(int index) {
            this.Children.Clear();
            BitmapSource bitmapSoure = ConvertBitmapToBitmapSource(this.Bitmaps[index]);

            Image image = new Image {
                Source = bitmapSoure,
                Width = this.ActualWidth,
                Height = this.ActualHeight
            };

            Debug.WriteLine($"{image.ActualWidth} {image.ActualHeight}");
            Debug.WriteLine($"{image.Width} {image.Height}");
            this.Children.Add(image);
        }

        private List<Bitmap> Bitmaps { get => this._bitmaps; set => this._bitmaps = value; }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);
        }

        public static List<Bitmap> CreateBitmaps(List<RenderNode> pages) {
            List<Bitmap> list = [];

            int i = 0;
            foreach (RenderNode page in pages) {
                Debug.WriteLine($"Page #{i++} {page.Size.Width} {page.Size.Height}");
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
