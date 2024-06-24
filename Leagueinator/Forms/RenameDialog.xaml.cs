using System.Windows;
using System.Windows.Input;

namespace Leagueinator.Forms {
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window {
        public string NewName { get; private set; } = "";

        public RenameDialog(string currentName) {
            InitializeComponent();
            txtNewName.Text = currentName;
            txtNewName.SelectAll();
            txtNewName.Focus();

            this.txtNewName.KeyDown += this.HndKeyDown;
        }

        private void HndKeyDown(object sender, System.Windows.RoutedEventArgs e) {
            if (e is not KeyEventArgs keyArgs) return;

            if (keyArgs.Key == Key.Enter) {
                this.ButOk.Focus();
            }
        }

        private void HndOkButtonClick(object sender, RoutedEventArgs e) {
            // Set the NewName property to the text in the textbox.
            NewName = txtNewName.Text;
            DialogResult = true;
            Close();
        }

        private void HndCancelButtonClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }
    }
}
