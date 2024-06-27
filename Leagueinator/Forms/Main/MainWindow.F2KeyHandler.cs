using Leagueinator.Controls;
using Leagueinator.Model.Tables;
using System.Windows.Input;

namespace Leagueinator.Forms.Main {
    public partial class MainWindow {

        /// <summary>
        /// Main window key-down handler to catch F2 for renaming players.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HndKeyDownRenamePlayer(object sender, KeyEventArgs e) {
            if (e.Key != Key.F2) return;
            if (this.EventRow is null) return;
            if (Keyboard.FocusedElement is not PlayerTextBox textBox) return;

            this.ClearFocus();
            string oldName = textBox.Text;
            RenameDialog dialog = new RenameDialog(oldName);

            if (dialog.ShowDialog() == true) {
                var eventMembers = this.EventRow.Members.Where(x => x.Player.Equals(oldName));
                var idlePlayers = this.EventRow.IdlePlayers.Where(x => x.Player.Equals(oldName));

                foreach (MemberRow memberRow in eventMembers) memberRow.Player = dialog.NewName;
                foreach (IdleRow idleRow in idlePlayers) idleRow.Player = dialog.NewName;
            }
        }
    }
}
