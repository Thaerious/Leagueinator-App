using Leagueinator.Controls;
using System.Windows.Input;

namespace Leagueinator.Forms.Main {
    public partial class MainWindow {
        private void HndKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.F2) {
                if (this.EventRow is null) return;
                if (Keyboard.FocusedElement is PlayerTextBox textBox) {
                    string oldName = textBox.Text;
                    RenameDialog dialog = new RenameDialog(oldName);

                    if (dialog.ShowDialog() == true) {
                        string newName = dialog.NewName;
                        var row = this.EventRow.League.PlayerTable.GetRow(oldName);
                        row.Name = "";
                        row.Name = newName;

                        textBox.Text = newName;
                    }
                }
            }
        }
    }
}
