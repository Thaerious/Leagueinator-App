using Leagueinator.PrinterElement;
using System.Diagnostics;
using System.Drawing.Printing;

namespace TestForm {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Menu_Print(object sender, EventArgs e) {
            this.printDocument.DefaultPageSettings.Landscape = true;
            this.printDocument.PrintPage += this.HndPrint;
            this.printPreviewDialog.Document = this.printDocument;
            this.printPreviewDialog.ShowDialog();
        }

        public void HndPrint(object sender, PrintPageEventArgs e) {
            if (e.Graphics == null) throw new NullReferenceException();
            e.HasMorePages = this.DrawNextPage(e.Graphics);
        }

        public bool DrawNextPage(Graphics g) {
            Debug.WriteLine(printDocument.DefaultPageSettings.PaperSize);
            var pageWidth = this.printDocument.DefaultPageSettings.PaperSize.Height;
            var pageHeight = this.printDocument.DefaultPageSettings.PaperSize.Width;
            var printer = new Printer(pageWidth, pageHeight);

            printer.Style.Flex_Direction = Flex_Direction.row;
            printer.Style.Justify_Content = Justify_Content.center;
            printer.Style.Align_Items = Align_Items.center;
            printer.Style.Display = Display.flex;

            printer.OnDraw = p => {
                p.DrawRectangle(new Pen(Color.Black, 5));
                p.FillRectangle(Color.SkyBlue);
            };

            var container = printer.Add(new PointF(500, 200));
            //container.Style.Flex_Direction = Flex_Direction.row;
            container.Style.Justify_Content = Justify_Content.space_evenly;
            container.Style.Align_Items = Align_Items.center;
            //container.Style.Display = Display.flex;
            container.OnDraw = p => {
                p.DrawRectangle(new Pen(Color.Black, 5));
                p.FillRectangle(Color.Purple);
            };

            container.Add(new PrintBox("1"));
            container.Add(new PrintBox("2"));
            container.Add(new PrintBox("3"));
            container.Add(new PrintBox("4"));

            container.Children[2].Translate(0, 25);

            printer.Run(g);
            return false;
        }
    }

    public class PrintBox : Printer {
        string text = "";
        Random random = new Random();

        public PrintBox(string text) {
            this.text = text;
            this.Style.Pen = new Pen(Color.Black, 5);
            this.OnDraw = p => this.Draw();
            //this.Size = new SizeF(random.Next(1, 5) * 50, random.Next(1, 5) * 50);            
            this.Size = new SizeF(100, 100);
        }

        public void Draw() {
            Debug.WriteLine(this.ScreenRect);
            this.FillRectangle(Color.Orange);
            this.DrawRectangle();
            this.DrawString(text);
        }
    }
}
