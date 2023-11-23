using Leagueinator.Printer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevPrint {
    internal class CanvasPane : Panel {
        private PrinterElement? document;

        public CanvasPane() {
            this.SetupPrintElements();
        }

        public void SetupPrintElements() {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string[] resourceNames = assembly.GetManifestResourceNames();
            string xmlResource = "DevPrint.assets.layout.xml";
            string ssResource = "DevPrint.assets.style.ss";
            string template = "DevPrint.assets.template.xml";

            if (xmlResource is null) throw new NullReferenceException(xmlResource);
            if (ssResource is null) throw new NullReferenceException(ssResource);
            if (template is null) throw new NullReferenceException(template);

            using Stream? xmlStream = assembly.GetManifestResourceStream(xmlResource) ?? throw new NullReferenceException();
            using StreamReader xmlReader = new StreamReader(xmlStream);

            using Stream? ssStream = assembly.GetManifestResourceStream(ssResource) ?? throw new NullReferenceException();
            using StreamReader ssReader = new StreamReader(ssStream);

            string xmlString = xmlReader.ReadToEnd();
            string ssString = ssReader.ReadToEnd();

            XMLLoader xmlLoader;
            try {
                xmlLoader = new(xmlString, ssString);
                this.document = xmlLoader.Root;
            }
            catch (Exception ex) {
                Debug.Write(ex.ToString());
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            Debug.WriteLine($"OnPaint {this.Width} {this.Height}");
            base.OnPaint(e);
        }
    }
}
