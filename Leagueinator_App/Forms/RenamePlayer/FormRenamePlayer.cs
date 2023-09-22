using System.Windows.Forms;

namespace Leagueinator.App.Forms.RenamePlayer{
    public partial class FormRenamePlayer : Form {

        public string PlayerName => this.txtName.Text;

        public FormRenamePlayer() {
            this.InitializeComponent();
        }

        private void TxtName_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                this.butOK.PerformClick();
            }
        }
    }
}
