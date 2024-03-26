using Leagueinator.Model;

namespace Leagueinator.Forms {
    public partial class FormEventResults : Form {
        private League League;

        public FormEventResults(League league) {
            this.League = league;
            this.InitializeComponent();
        }

        private void HndPrintClick(object sender, EventArgs e) {

        }
    }
}
