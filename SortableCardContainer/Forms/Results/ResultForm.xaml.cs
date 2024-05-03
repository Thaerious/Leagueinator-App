﻿using Leagueinator.Printer.Styles;
using Leagueinator.Controls;
using System.Windows;
using System.Windows.Controls;
using PrintDialog = System.Windows.Controls.PrintDialog;
using Size = System.Windows.Size;
using System.Printing;
using Leagueinator.Model.Tables;

namespace Leagueinator.Forms {
    /// <summary>
    /// Interaction logic for PrinterForm.xaml
    /// </summary>
    public partial class PrinterForm : Window {
        private readonly Size Dims = new(850, 1100);

        /// <summary>
        /// Create a new printer form for the specified eventRow.
        /// </summary>
        /// <param name="eventRow"></param>
        public PrinterForm(EventRow eventRow) {
            InitializeComponent();
            this.SizeChanged += this.HndSizeChanged;
            this.InnerCanvas.Pages = new Flex().DoLayout(ResultBuilder.BuildElement(eventRow));
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

            this.InnerCanvas.SetPage(0);
        }

        /// <summary>
        /// Show print dialog and handle print result.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HndPrintClick(object sender, RoutedEventArgs e) {
            PrintDialog dialog = new PrintDialog();

            PrintCapabilities capabilities = dialog.PrintQueue.GetPrintCapabilities(dialog.PrintTicket);
            double printableWidth = capabilities.PageImageableArea.ExtentWidth;
            double printableHeight = capabilities.PageImageableArea.ExtentHeight;

            if (dialog.ShowDialog() == true) {
                for (int i = 0; i < this.InnerCanvas.Bitmaps.Count; i++) {
                    Image image = new() {
                        Source = PrinterImage.ConvertBitmapToBitmapSource(this.InnerCanvas.Bitmaps[i]),
                        Width = printableWidth,
                        Height = printableHeight
                    };
                    dialog.PrintVisual(image, "Printing Results");
                }
            }
        }
    }
}