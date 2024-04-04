using Leagueinator.CSSParser;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Diagnostics;

namespace Leagueinator.VisualUnitTest {
    public partial class Main : Form {

        private int lastSelectedTest = -1;

        public Main() {
            InitializeComponent();

            this.ListBoxFile.SelectedIndexChanged += this.HndChangeSelectedTest;
        }

        private void HndChangeSelectedTest(object? sender, EventArgs e) {
            string testName = (string)this.ListBoxFile.Items[ListBoxFile.SelectedIndex];

            if (lastSelectedTest != -1) SaveTest(
                (string)this.ListBoxFile.Items[lastSelectedTest]
            );

            this.lastSelectedTest = ListBoxFile.SelectedIndex;
            this.LoadTest();
        }

        private void SaveTest(string? name = null) {
            name ??= (string)this.ListBoxFile.Items[ListBoxFile.SelectedIndex];

            string xmlFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.xml");
            string styleFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.style");

            File.WriteAllText(xmlFile, this.RichTextXML.Text);
            File.WriteAllText(styleFile, this.RichTextStyle.Text);
        }

        private void LoadTest(string? name = null) {
            name ??= (string)this.ListBoxFile.Items[ListBoxFile.SelectedIndex];

            string xmlFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.xml");
            string styleFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.style");

            this.RichTextXML.Text = File.ReadAllText(xmlFile);
            this.RichTextStyle.Text = File.ReadAllText(styleFile);
        }

        private void HndMenuCloseClick(object sender, EventArgs e) {
            this.SaveTest();
            this.Close();
        }

        private void HndMenuLoadClick(object sender, EventArgs e) {
            if (this.FolderDialog.ShowDialog() == DialogResult.OK) {
                this.LoadDirPath();
            };
        }
        private void HndMenuAddTest(object sender, EventArgs e) {
            if (this.FolderDialog.SelectedPath.IsEmpty()) return;
            Debug.WriteLine(this.FolderDialog.SelectedPath);

            using InputNameDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK) {
                string xmlPath = Path.Join(this.FolderDialog.SelectedPath, $"{dialog.TestName}.xml");
                string stylePath = Path.Join(this.FolderDialog.SelectedPath, $"{dialog.TestName}.style");
                File.CreateText(xmlPath);
                File.CreateText(stylePath);
                this.ListBoxFile.Items.Add(dialog.TestName);
            }
        }

        private void LoadDirPath() {
            this.ListBoxFile.Items.Clear();
            string[] files = Directory.GetFiles(this.FolderDialog.SelectedPath, "*.xml");

            foreach (string file in files) {
                var filename = Path.GetFileNameWithoutExtension(file);
                this.ListBoxFile.Items.Add(filename);
            }

            if (files.Length > 0) this.ListBoxFile.SelectedIndex = 0;
        }

        private void DrawActual() {
            try {
                LoadedStyles styles = StyleLoader.Load(this.RichTextStyle.Text);
                Element root = XMLLoader.Load(this.RichTextXML.Text);
                styles.ApplyTo(root);

                Flex flex = new();

                (int pages, RenderNode renderNode) = flex.DoLayout(root);
                this.CanvasActual.RenderNode = renderNode;
                this.CanvasActual.Invalidate(true);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex);
            }
        }

        private void HndMenuRunSelected(object sender, EventArgs e) {
            DrawActual();
        }
    }
}
