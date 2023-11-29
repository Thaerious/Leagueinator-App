using Leagueinator.Model;
using Leagueinator.Printer;
using Leagueinator_Model.Model.Tables;
using Printer.Printer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace DevPrint {
    internal class CanvasPane : Panel {

        private LeagueEvent _leagueEvent;
        public LeagueEvent LeagueEvent { 
            get => _leagueEvent;
            set {
                this._leagueEvent = value;
                this.ApplyEvent();
            }
        }

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

        private void ApplyEvent() {
            var container = this.document.Children[".container"][0];
            var leagueData = this.LeagueEvent.ToDataSet();

            container.Children.Clear();

            var eventTable = new EventTable(leagueData.Tables["event"] ?? throw new NullReferenceException());
            var teamTable = new TeamTable(leagueData.Tables["team"] ?? throw new NullReferenceException());

            foreach (int id in teamTable.AllIDs()) {
                var template = this.template.Clone();
                container.AddChild(template);

                foreach (string name in teamTable.GetNames(id)) {
                    var nameElement = new TextElement(name);
                    template.Children.QuerySelector(".players").AddChild(nameElement);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            this.document.Update();
            this.document.Draw(e.Graphics);
        }
    }
}
