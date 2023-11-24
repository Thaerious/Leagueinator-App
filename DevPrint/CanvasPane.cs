using Leagueinator.Model;
using Leagueinator.Printer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace DevPrint {
    internal class CanvasPane : Panel {
        public LeagueEvent LeagueEvent { get; set; }
        private PrinterElement document = new();
        private PrinterElement template = new();
        
        public CanvasPane() {
            this.SetupPrintElements();
        }

        public void SetupPrintElements() {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string xmlResource = "DevPrint.assets.layout.xml";
            string ssResource = "DevPrint.assets.style.ss";
            string templResource = "DevPrint.assets.template.xml";            

            using Stream? xmlStream = assembly.GetManifestResourceStream(xmlResource) ?? throw new NullReferenceException();
            using StreamReader xmlReader = new StreamReader(xmlStream);

            using Stream? ssStream = assembly.GetManifestResourceStream(ssResource) ?? throw new NullReferenceException();
            using StreamReader ssReader = new StreamReader(ssStream);

            using Stream? templStream = assembly.GetManifestResourceStream(templResource) ?? throw new NullReferenceException();
            using StreamReader templReader = new StreamReader(templStream);

            string xmlString = xmlReader.ReadToEnd();
            string ssString = ssReader.ReadToEnd();
            string templString = templReader.ReadToEnd();

            try {
                this.document = XMLLoader.Load(xmlString, ssString);
                this.template = XMLLoader.Load(templString, ssString);
            }
            catch (Exception ex) {
                Debug.Write(ex.ToString());
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            Debug.WriteLine($"OnPaint {this.Width} {this.Height}");
            base.OnPaint(e);

            this.document.Update();
            this.document.Draw(e.Graphics);
        }
    }
}
