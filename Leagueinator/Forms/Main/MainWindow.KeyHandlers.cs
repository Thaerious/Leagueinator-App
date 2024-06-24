using Leagueinator.Controls;
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
            string prevName = textBox.Text;
            RenameDialog dialog = new RenameDialog(prevName);

            if (dialog.ShowDialog() == true) {
                bool prevNameExists = this.EventRow.League.PlayerTable.HasRow(prevName);
                if (!prevNameExists) return;

                string newName = dialog.NewName;
                var row = this.EventRow.League.PlayerTable.GetRow(prevName);
                row.Name = newName;

                textBox.Text = newName;
            }
        }
    }
}
