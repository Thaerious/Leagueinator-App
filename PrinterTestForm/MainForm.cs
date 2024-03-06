using Leagueinator.Printer;
using System.Diagnostics;

namespace PrinterTestForm
{
    public partial class MainForm : Form {
        private static readonly string AppPath = "PrinterTestForm";
        private readonly string xmlPath;
        private readonly string stylePath;

        public MainForm() {
            this.InitializeComponent();

            var menuItem = new ToolStripMenuItem {
                ShortcutKeys = Keys.Control | Keys.S // Set your desired shortcut key
            };
            menuItem.Click += new EventHandler(this.ToolRefresh_Click); // Link to button's event handler
            menuItem.Visible = false; // Hide the menu item if not needed in a menu
            this.toolStrip1.Items.Add(menuItem);

            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this.xmlPath = Path.Combine(roamingPath, AppPath, "layout.xml");
            this.stylePath = Path.Combine(roamingPath, AppPath, "style.ss");

            if (!Directory.Exists(Path.Combine(roamingPath, AppPath))) {
                Directory.CreateDirectory(Path.Combine(roamingPath, AppPath));
            }

            if (File.Exists(this.xmlPath)) {
                this.txtXML.Text = File.ReadAllText(this.xmlPath);
            }

            if (File.Exists(this.stylePath)) {
                this.txtStyle.Text = File.ReadAllText(this.stylePath);
            }

            this.ToolRefresh_Click(this, new EventArgs());
        }

        private void ToolRefresh_Click(object _, EventArgs __) {
            try {
                string xmlString = this.txtXML.Text;
                string styleString = this.txtStyle.Text;

                File.WriteAllText(this.xmlPath, xmlString);
                File.WriteAllText(this.stylePath, styleString);

                var xmlLoader = new XMLLoader();
                xmlLoader.LoadStyle(styleString);
                Element root = xmlLoader.LoadXML(xmlString);

                this.printerCanvas.RootElement = root;
                root.Style.DoSize(root);
                root.Style.DoPos(root);

                this.printerCanvas.Invalidate();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine(ex);
            }
        }

        private void ToolPrintXML_Click(object sender, EventArgs e) {
            Debug.WriteLine(this.printerCanvas.RootElement.ToXML(
                (element, xml) => {
                    xml.InnerText(element.ContainerRect.ToString());
                }
            ));
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
        }

        private void ToolPrintCSS_Click(object sender, EventArgs e) {
            var target = this.printerCanvas.RootElement["inner"][0];
            Debug.WriteLine(target);
            Debug.WriteLine(target.Style);

            var text = target["@text"][0];
            Debug.WriteLine(text);
            Debug.WriteLine(text.Style);
        }

        private void ToolPrintLocXML(object sender, EventArgs e) {
            Debug.WriteLine(this.printerCanvas.RootElement?.LocXML());
        }
    }
}
