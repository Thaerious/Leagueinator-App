using Leagueinator.Printer;
using System.Diagnostics;
using System.Reflection;

namespace PrinterTestForm {
    public partial class PrinterCanvas : UserControl {
        public PrinterElement? RootElement { get; set; }

        public PrinterCanvas() {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            if (this.RootElement is not null) {
                this.RootElement.Style.Width = this.Width;
                this.RootElement.Style.Height = this.Height;
                this.RootElement.Update();
                this.RootElement.Draw(e.Graphics);
            }
        }
    }
}
