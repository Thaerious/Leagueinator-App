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
            if (e.Key == Key.F2) {
                if (this.EventRow is null) return;

                if (Keyboard.FocusedElement is PlayerTextBox textBox) {
                    this.ClearFocus();
                    string oldName = textBox.Text;
                    RenameDialog dialog = new RenameDialog(oldName);

                    if (dialog.ShowDialog() == true) {
                        bool oldNameExists = this.EventRow.League.PlayerTable.HasRow(oldName);
                        if (!oldNameExists) return;

                        string newName = dialog.NewName;
                        var row = this.EventRow.League.PlayerTable.GetRow(oldName);
                        row.Name = newName;

                        textBox.Text = newName;
                    }
                }
            }
        }
    }
}
