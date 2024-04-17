using Leagueinator.Model;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Printer.Source;
using System.Reflection;

namespace Leagueinator.Forms {
    public partial class FormEventResults : Form {
        private League League;
        private Element root;
        private RenderNode RenderNode;
        private int Pages;

        public FormEventResults(League league) {
            this.League = league;
            this.InitializeComponent();
            this.root = 
                Assembly.GetExecutingAssembly().
                LoadResources("Leagueinator.Assets.EventForm.xml", "Leagueinator.Assets.EventForm.style");

            (this.Pages, this.RenderNode) = Flex.Layout(root);
            this.Canvas.RenderNode = this.RenderNode;
        }


        private void HndPrintClick(object sender, EventArgs e) {

        }
    }
}
