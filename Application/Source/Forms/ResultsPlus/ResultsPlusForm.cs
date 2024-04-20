using Leagueinator.AssetControllers;
using Leagueinator.Model;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Utility;
using System.Diagnostics;

namespace Leagueinator.Forms.ResultsPlus {
    public partial class ResultsPlusForm : Form {
        private League League;
        private EventScoreForm root;
        private List<RenderNode> pages;

        private int _page;
        public int Page {
            get => this._page;
            set {
                if (value < 0) value = 0;
                if (value >= this.pages.Count) value = this.pages.Count - 1;
                this._page = value;
                this.LabelPage.Text = $"{value + 1}/{this.pages.Count}";
                this.Canvas.RenderNode = pages[value];
            }
        }

        public ResultsPlusForm(League league) {
            this.League = league;
            this.InitializeComponent();
            this.CustomizeComponent();

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

            this.pages = Flex.Layout(root);
            this.Page = 0;
        }

        private void CustomizeComponent() {
            this.toolContainer.Location = this.toolContainer.Parent!.Size.Center(this.toolContainer.Size);

            this.ToolPanel.Resize += (object? sender, EventArgs e) => {
                this.toolContainer.Location = this.toolContainer.Parent!.Size.Center(this.toolContainer.Size);
            };
        }

        private void HndPrintClick(object sender, EventArgs e) {

        }

        private void ButFirst_Click(object sender, EventArgs e) {
            this.Page = 0;
        }

        private void ButPrev_Click(object sender, EventArgs e) {
            this.Page--;
        }

        private void ButNext_Click(object sender, EventArgs e) {
            this.Page++;
        }

        private void ButLast_Click(object sender, EventArgs e) {
            this.Page = this.pages.Count - 1;
        }
    }
}

