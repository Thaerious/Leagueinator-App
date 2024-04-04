using Leagueinator.CSSParser;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Diagnostics;

namespace Leagueinator.VisualUnitTest {
    public partial class Main : Form {
        private int TestIndex = -1;
        private string TestName => (string)this.ListBoxFile.Items[TestIndex];
        private Bitmap? Bitmap = null;

        public Main() {
            InitializeComponent();
            this.ListBoxFile.SelectedIndexChanged += this.HndChangeSelectedTest;
            this.PanelExpected.Paint += this.HndPanelPaint;
        }

        private void HndPanelPaint(object? sender, PaintEventArgs e) {
            e.Graphics.Clear(Color.White);
            if (this.Bitmap == null) return;
            e.Graphics.DrawImage(this.Bitmap, 0, 0);
        }

        private void HndChangeSelectedTest(object? sender, EventArgs e) {
            if (this.TestIndex != -1) SaveTest(TestName);
            this.TestIndex = ListBoxFile.SelectedIndex;
            this.LoadTest();
            this.DrawActual();
            this.PanelExpected.Invalidate();
        }

        private void SaveTest(string? name = null) {
            if (name == null && TestIndex == -1) return;
            name ??= TestName;

            string xmlFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.xml");
            string styleFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.style");

            File.WriteAllText(xmlFile, this.RichTextXML.Text);
            File.WriteAllText(styleFile, this.RichTextStyle.Text);
        }

        private void LoadTest(string? name = null) {
            if (name == null && TestIndex == -1) return;
            name ??= TestName;

            string xmlFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.xml");
            string styleFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.style");
            string bmpFile = Path.Join(this.FolderDialog.SelectedPath, $"{name}.bmp");

            this.RichTextXML.Text = File.ReadAllText(xmlFile);
            this.RichTextStyle.Text = File.ReadAllText(styleFile);
            if (File.Exists(bmpFile)) this.LoadExpected(bmpFile);
            else this.ClearExpected();
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

            if (files.Length > 0) {
                this.ListBoxFile.SelectedIndex = 0;
                this.TestIndex = 0;
            }
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
            }
        }

        private void ClearExpected() {            
            this.Bitmap?.Dispose();
            this.Bitmap = null;
        }

        private void LoadExpected(string imagePath) {
            try {
                this.ClearExpected();
                this.Bitmap = new Bitmap(imagePath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HndMenuRunSelected(object sender, EventArgs e) {
            DrawActual();
        }

        private void HndMenuRenameTest(object sender, EventArgs e) {
            if (this.FolderDialog.SelectedPath.IsEmpty()) return;
            string currentName = this.TestName;

            using InputNameDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK) {
                string fromXMLPath = Path.Join(this.FolderDialog.SelectedPath, $"{this.TestName}.xml");
                string fromStylePath = Path.Join(this.FolderDialog.SelectedPath, $"{this.TestName}.style");
                string toXMLPath = Path.Join(this.FolderDialog.SelectedPath, $"{dialog.TestName}.xml");
                string toStylePath = Path.Join(this.FolderDialog.SelectedPath, $"{dialog.TestName}.style");

                if (File.Exists(toXMLPath)) {
                    MessageBox.Show("Test Already Exists", "Warning", MessageBoxButtons.OK);
                    return;
                }

                File.Move(fromXMLPath, toXMLPath);
                File.Move(fromStylePath, toStylePath);

                this.ListBoxFile.Items[this.TestIndex] = dialog.TestName;
            }
        }

        private void HndMenuSave(object sender, EventArgs e) {
            this.SaveTest();
            this.DrawActual();
        }

        private void HndMenuDuplicate(object sender, EventArgs e) {
            if (this.FolderDialog.SelectedPath.IsEmpty()) return;
            this.SaveTest();
            this.DrawActual();

            string newName = $"copy - {this.TestName}";
            string xmlPath = Path.Join(this.FolderDialog.SelectedPath, $"{newName}.xml");
            string stylePath = Path.Join(this.FolderDialog.SelectedPath, $"{newName}.style");

            if (File.Exists(xmlPath)) {
                MessageBox.Show("Test Already Exists", "Warning", MessageBoxButtons.OK);
                return;
            }

            File.WriteAllText(xmlPath, this.RichTextXML.Text);
            File.WriteAllText(stylePath, this.RichTextStyle.Text);

            this.ListBoxFile.Items.Add(newName);
        }

        private void HndAcceptActual(object sender, EventArgs e) {
            this.CreateExpected();
            this.LoadTest();
            this.PanelExpected.Invalidate();
        }

        private void CreateExpected() {
            Bitmap bmp = new Bitmap(CanvasActual.Width, CanvasActual.Height);
            CanvasActual.DrawToBitmap(bmp, new Rectangle(Point.Empty, CanvasActual.Size));
            string imagePath = Path.Join(this.FolderDialog.SelectedPath, $"{this.TestName}.bmp");
            bmp.Save(imagePath, System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}
