﻿using Leagueinator.CSSParser;
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

                LoadedStyles styles = StyleLoader.Load(styleString);
                Element root = XMLLoader.Load(xmlString);
                styles.ApplyTo(root);

                this.printerCanvas.RootElement = root;
                root.Style.DoLayout(root);

                this.printerCanvas.Invalidate();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine(ex);
            }
        }

        private void EleOnDraw(Graphics g, Element element, int page) {
            Debug.WriteLine($"Drawing {element.Identifier} {element.Style.Page}");
            Debug.WriteLine($" - content {element.ContentRect}");
            Debug.WriteLine($" - border {element.BorderRect}");
            Debug.WriteLine($" - outer {element.OuterRect}");
        }

        private void ToolPrintXML_Click(object sender, EventArgs e) {
            Debug.WriteLine(this.printerCanvas.RootElement.ToXML());
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
            Debug.WriteLine(this.printerCanvas.RootElement.ToXML(
                (element, xml) => {
                    xml.InnerText(element.Style.ToString());
                }
            ));
        }

        private void ToolPrintLocXML(object sender, EventArgs e) {
            Debug.WriteLine(this.printerCanvas.RootElement.ToXML(
                (element, xml) => {
                    xml.InnerText(element.ContentRect.ToString());
                }
            ));
        }

        private void butPrevClick(object sender, EventArgs e) {
            int page = int.Parse(this.lblPage.Text);
            if (--page < 0) page = 0;
            this.lblPage.Text = $"{page}";
            this.printerCanvas.Page = page;
            this.printerCanvas.Invalidate();
        }

        private void butNextClick(object sender, EventArgs e) {
            int page = int.Parse(this.lblPage.Text);
            this.lblPage.Text = $"{++page}";
            this.printerCanvas.Page = page;
            this.printerCanvas.Invalidate();
        }
    }
}
