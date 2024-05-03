using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Printer;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Printing;


//using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using PrintDialog = System.Windows.Controls.PrintDialog;
using Size = System.Windows.Size;

namespace Leagueinator.Forms {
    /// <summary>
    /// Interaction logic for PrinterForm.xaml
    /// </summary>
    public partial class PrinterForm : Window {
        private Size Dims = new(400, 600);

        public PrinterForm() {
            InitializeComponent();
            this.SizeChanged += this.HndSizeChanged;
            this.InnerCanvas.Pages = LoadDefault();
        }

        /// <summary>
        /// Attempt to fit the width of the inner to the width of the outer.
        /// The width will be equal to the outer width.
        /// The height will be proportional to the width keeping the ratio of Dims.Width : Dim.Height.
        /// This is used to determine if the inner nees to pin to width or height.
        /// </summary>
        /// <returns></returns>
        private void HndSizeChanged(object sender, SizeChangedEventArgs e) {
            double outerHeight = this.OuterCanvas.ActualHeight;
            double outerWidth = this.OuterCanvas.ActualWidth;
            double height = (outerWidth / this.Dims.Width) * this.Dims.Height;
            double width = (outerHeight / this.Dims.Height) * this.Dims.Width;

            if (height <= outerHeight) {
                // Pin To Width                               
                Canvas.SetTop(this.InnerCanvas, (outerHeight - height) / 2);
                Canvas.SetLeft(this.InnerCanvas, 0);
                this.InnerCanvas.Width = outerWidth;
                this.InnerCanvas.Height = height;
            }
            else {
                // Pin To Height
                Canvas.SetTop(this.InnerCanvas, 0);
                Canvas.SetLeft(this.InnerCanvas, (outerWidth - width) / 2);
                this.InnerCanvas.Width = width;
                this.InnerCanvas.Height = outerHeight;
            }

            //var scalex = this.inner.Width / this.inner.dimX;
            //var scaley = this.inner.Height / this.inner.dimY;

            this.InnerCanvas.SetPage(0);
            Debug.WriteLine($"{this.InnerCanvas.Width} {this.InnerCanvas.Height}");
        }

        private static List<RenderNode> LoadDefault() {
            LoadedStyles styles = Assembly.GetExecutingAssembly().LoadStyleResource("Leagueinator.Assets.default.style");
            Element docroot = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.default.xml");
            styles.AssignTo(docroot);
            return new Flex().DoLayout(docroot);
        }

        private void HndPrintClick(object sender, RoutedEventArgs e) {
            PrintDialog dialog = new PrintDialog();

            //PrintDocument printDocument = dialog.PrintDocument;
            //printDocument.PrintPage += new PrintPageEventHandler(PrintPage);

            if (dialog.ShowDialog() == true) {
                new Image();

                Debug.WriteLine("dialog.ShowDialog()");
                //dialog.PrintVisual(this.InnerCanvas.Children[0], "Printing an Image");
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs e) {
        }
    }
}
