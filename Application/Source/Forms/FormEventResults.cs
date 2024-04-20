using Leagueinator.AssetControllers;
using Leagueinator.Model;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using System.Diagnostics;

namespace Leagueinator.Forms {
    public partial class FormEventResults : Form {
        private League League;
        private EventScoreForm root;
    

        public FormEventResults(League league) {
            this.League = league;
            this.InitializeComponent();

            this.root = EventScoreForm.New();

            for (int i = 0; i < 10; i++) {
                var team = this.root.AddTeam();
                team.AddName("John Candy");
                team.AddName("Eugene Levy");
                team.AddRow();
                team.AddRow();
                team.AddRow();
            }

            if (this.root.Invalid) this.root.DoLayout();
            this.root.Invalid = false;

            Debug.WriteLine(this.root.Invalid);
            Debug.WriteLine(this.root["team"][0].ToXML());

            List<RenderNode> pages = Flex.Layout(root);
            this.Canvas.RenderNode = pages[0];
        }


        private void HndPrintClick(object sender, EventArgs e) {

        }
    }
}

