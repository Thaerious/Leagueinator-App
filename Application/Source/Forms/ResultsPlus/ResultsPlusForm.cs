using Leagueinator.Model.Tables;
using Leagueinator.Model.Views;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Utility;
using Printer;
using System.Diagnostics;
using System.Reflection;

namespace Leagueinator.Forms.ResultsPlus {
    public partial class ResultsPlusForm : Form {
        private EventRow EventRow;
        private Element root;
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

        public ResultsPlusForm(EventRow eventRow) {
            this.EventRow = eventRow;
            this.InitializeComponent();
            this.SetupNavigationButtons();
            this.root = this.InitializeXML();
            this.pages = Flex.Layout(this.root);
            this.Page = 0;
        }

        private Element InitializeXML() {
            LoadedStyles styles = Assembly.GetExecutingAssembly().LoadStyleResource("Leagueinator.Assets.EventScoreForm.style");
            Element docroot = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.EventScoreForm.xml");
            Element teamXML = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.TeamScore.xml");
            Element rowXML  = Assembly.GetExecutingAssembly().LoadXMLResource<Element>("Leagueinator.Assets.ScoreRow.xml");

            var allTeams = this.EventRow.AllTeams();

            foreach (var pair in allTeams) {
                Element currentTeamXML = teamXML.Clone();
                foreach (string name in pair.Key.Players) {
                    currentTeamXML["names"][0].AddChild(
                        new Element {
                            TagName = "Name",
                            InnerText = name
                        }
                    );
                }

                foreach (Match match in pair.Value) {
                    Element row = rowXML.Clone();
                    row["index"][0].InnerText = match.Round.ToString();
                    row["lane"][0].InnerText = match.Lane.ToString();
                    row["bowls_for"][0].InnerText = match.Bowls.ToString();
                    row["ends_played"][0].InnerText = match.Ends.ToString();
                    row["tie"][0].InnerText = match.Tie.ToString();

                    currentTeamXML["rounds"][0].AddChild(row);
                }

                docroot["page"][0].AddChild(currentTeamXML);
            }

            styles.AssignTo(docroot);
            return docroot;
        }

        private void SetupNavigationButtons() {
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

