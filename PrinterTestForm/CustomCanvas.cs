using Leagueinator.Printer;
using System.Diagnostics;
using System.Reflection;

namespace PrinterTestForm {
    public partial class CustomCanvas : UserControl {
        public CustomCanvas() {
            InitializeComponent();
        }

        private PrinterElement printerRoot;

        public PrinterElement ContainerElement { get; private set; }

        public void SetupPrintElements() {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string[] resourceNames = assembly.GetManifestResourceNames();
            string xmlResource = "PrinterTestForm.xml.layout.xml";
            string ssResource = "PrinterTestForm.xml.style.ss";

            if (xmlResource is null) throw new NullReferenceException(xmlResource);
            if (ssResource is null) throw new NullReferenceException(ssResource);

            using Stream? xmlStream = assembly.GetManifestResourceStream(xmlResource) ?? throw new NullReferenceException();
            using StreamReader xmlReader = new StreamReader(xmlStream);

            using Stream? ssStream = assembly.GetManifestResourceStream(ssResource) ?? throw new NullReferenceException();
            using StreamReader ssReader = new StreamReader(ssStream);

            string xmlString = xmlReader.ReadToEnd();
            string ssString = ssReader.ReadToEnd();

            XMLLoader xmlLoader;
            try {
                xmlLoader = new(xmlString, ssString);
                this.printerRoot = xmlLoader.Root;
                this.ContainerElement = this.printerRoot.Children.QuerySelector(".container")!;
            }
            catch (Exception ex) {
                Debug.Write(ex.ToString());
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            this.printerRoot.Style.Width = this.Width;
            this.printerRoot.Style.Height = this.Height;

            base.OnPaint(e);
            printerRoot.Update();
            Debug.WriteLine(this.printerRoot.InnerRect);
            printerRoot.Draw(e.Graphics);

            Pen pen = new(Color.LightGray) {
                DashPattern = new float[] { 1, 2 }
            };

            for (int x = 0; x < this.printerRoot.InnerRect.Width; x += 25) {
                var p1 = new PointF(x, 0);
                var p2 = new PointF(x, this.printerRoot.InnerRect.Bottom);
                e.Graphics.DrawLine(pen, p1, p2);
            }
            for (int y = 0; y < this.printerRoot.InnerRect.Height; y += 25) {
                var p1 = new PointF(0, y);
                var p2 = new PointF(this.printerRoot.InnerRect.Right, y);
                e.Graphics.DrawLine(pen, p1, p2);
            }
        }
    }
}
