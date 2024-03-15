namespace Printer_Dev_Form {
    public partial class Form1 : Form {
        public Form1() {
            this.InitializeComponent();
            this.printDocument.PrintPage += this.PrintPage;
            this.printPreviewDialog.Document = this.printDocument;
        }

        private void Menu_Print_Click(object sender, EventArgs e) {
            this.printDialog.AllowSomePages = true;
            DialogResult result = this.printDialog.ShowDialog();

            if (result == DialogResult.OK) {
                this.printDocument.Print();
            }
        }

        private void Menu_Preview_Click(object sender, EventArgs e) {
            this.printPreviewDialog.Show();
        }

        private void PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
