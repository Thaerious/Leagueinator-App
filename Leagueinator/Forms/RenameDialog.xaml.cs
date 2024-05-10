using Leagueinator.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
