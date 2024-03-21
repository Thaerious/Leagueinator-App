using Leagueinator.CSSParser;
using Leagueinator.Printer;
using Leagueinator.PrinterComponents;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Reflection;

namespace PrinterTestForm {
    public partial class PrinterLayoutDesigner : Form {
        private readonly string roamingPath;
        private string? xmlPath = null;
        private string? stylePath = null;

        private string FilePath {
            set {
                if (value is null) {
                    xmlPath = null;
                    stylePath = null;
                }
                else {
                    xmlPath = value;
                    stylePath = value + ".style";
                }
                this.UpdateTitle();
            }
        }

        private bool _isSaved = true;
        private bool IsSaved {
            get => _isSaved;
            set {
                _isSaved = value;
                this.UpdateTitle();
            }
        }

        private void UpdateTitle() {
            if (xmlPath is not null) {
                if (this.IsSaved) {
                    this.Text = $"{this.Name} [{xmlPath}]";
                }
                else {
                    this.Text = $"{this.Name} [{xmlPath}] *";
                }
            }
            else {
                this.Text = $"{this.Name}";
            }            
        }

        public PrinterLayoutDesigner() {
            this.InitializeComponent();
            this.printerCanvas.OnRepaintTime += (double ms) => {
                this.lblTimer.Text = ms + " ms";
            };

            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this.roamingPath = Path.Combine(roamingPath, this.Name, "data.txt");

            if (!Directory.Exists(Path.Combine(roamingPath, this.Name))) {
                Directory.CreateDirectory(Path.Combine(roamingPath, this.Name));
            }

            if (File.Exists(this.roamingPath)) {
                this.FilePath = File.ReadAllText(this.roamingPath);
                this.LoadFilePath();
            }
            else {
                this.MenuNewClick(null, null);
            }

            this.MenuRefreshClick(this, new EventArgs());
        }

        private void TXT_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '\t') {
                int tabSize = 4; // Set your desired tab size
                e.Handled = true; // Prevent the default tab behavior

                TextBox? textBox = sender as TextBox;
                if (textBox != null) {
                    int selectionStart = textBox.SelectionStart;
                    textBox.Text = textBox.Text.Insert(selectionStart, new string(' ', tabSize));
                    textBox.SelectionStart = selectionStart + tabSize;
                }
            }
            this.IsSaved = false;
        }


        private void ButPrevClick(object sender, EventArgs e) {
            int page = int.Parse(this.lblPage.Text);
            if (--page < 0) page = 0;
            this.lblPage.Text = $"{page}";
            this.printerCanvas.Page = page;
            this.printerCanvas.Invalidate(true);
        }

        private void ButNextClick(object sender, EventArgs e) {
            int page = int.Parse(this.lblPage.Text);
            this.lblPage.Text = $"{++page}";
            this.printerCanvas.Page = page;
            this.printerCanvas.Invalidate(true);
        }

        private void MenuLoadClick(object sender, EventArgs e) {
            using (OpenFileDialog dialog = new OpenFileDialog()) {
                dialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                dialog.Title = "Load Layout and Style";
                if (dialog.ShowDialog() == DialogResult.OK) {
                    this.FilePath = dialog.FileName;
                    this.LoadFilePath();
                    File.WriteAllText(this.roamingPath, dialog.FileName);
                }
            }
        }

        private void LoadFilePath() {
            this.txtXML.Text = File.ReadAllText(this.xmlPath);
            this.txtStyle.Text = File.ReadAllText(this.stylePath);
            this.MenuRefreshClick(null, null);
            
            this.saveToolStripMenuItem1.Enabled = true;
            this.IsSaved = true;
        }

        private void MenuSaveAsClick(object sender, EventArgs e) {
            using SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            dialog.Title = "Save Layout and Style";

            if (dialog.ShowDialog() == DialogResult.OK) {
                this.FilePath = dialog.FileName;
                this.MenuRefreshClick(null, null);
                File.WriteAllText(this.xmlPath!, this.txtXML.Text);
                File.WriteAllText(this.stylePath!, this.txtStyle.Text);
                this.saveToolStripMenuItem1.Enabled = true;
                File.WriteAllText(this.roamingPath, dialog.FileName);
                this.IsSaved = true;
            }            
        }

        private void MenuPrintClick(object sender, EventArgs e) {
            if (this.printerCanvas.RootElement == null) return;

            using PrintDialog dialog = new PrintDialog();
            ElementPrintHandler printHandler = new ElementPrintHandler(this.printerCanvas.RootElement);
            dialog.Document = new ElementPrintHandler(this.printerCanvas.RootElement);

            if (dialog.ShowDialog() == DialogResult.OK) {
                printHandler.Print();
            }
        }

        private void MenuPreviewClick(object sender, EventArgs e) {
            if (this.printerCanvas.RootElement == null) return;

            using PrintPreviewDialog dialog = new PrintPreviewDialog();
            PrintDocument printDocument = new PrintDocument();
            dialog.Document = new ElementPrintHandler(this.printerCanvas.RootElement);
        }

        private void MenuRefreshClick(object sender, EventArgs e) {
            try {
                string xmlString = this.txtXML.Text;
                string styleString = this.txtStyle.Text;
                  
                LoadedStyles styles = StyleLoader.Load(styleString);
                Element root = XMLLoader.Load(xmlString);
                styles.ApplyTo(root);

                this.printerCanvas.RootElement = root;
                root.Style.DoLayout(root);

                this.printerCanvas.Invalidate(true);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine(ex);
            }
        }

        private void MenuSaveClick(object sender, EventArgs e) {
            File.WriteAllText(this.xmlPath, this.txtXML.Text);
            File.WriteAllText(this.stylePath, this.txtStyle.Text);
            this.MenuRefreshClick(null, null);
            this.IsSaved = true;
        }

        private void MenuNewClick(object sender, EventArgs e) {
            this.txtXML.Text = "<root>\n</root>";

            this.txtStyle.Text =
                "root {" +
                "\tWidth: 850px;\n" +
                "\tHeight: 1100px;\n" +
                "}";

            this.FilePath = null;
        }
    }
}
