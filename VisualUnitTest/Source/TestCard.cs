using Leagueinator.CSSParser;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Diagnostics;

namespace Leagueinator.VisualUnitTest {
    public enum Status { PENDING, PASS, FAIL, UNTESTED, NO_TEST, NOT_SET }

    public partial class TestCard : Card {
        
        public DirectoryCard ParentCard => this._parentCard;

        public Paths Paths {
            get => new(this.ParentCard.DirPath, this.Text);
        }

        public TestCard(DirectoryCard parent, string testName) : base() {
            InitializeComponent();
            this._parentCard = parent;
            this.Text = testName;
            this.Label.BackColor = Color.Transparent;

            this.MouseDown += HndDragStart;
            this.Label.MouseDown += HndDragStart;

            this.ButtonFail.Click += (s, e) => {
                this.Status = Status.NO_TEST;
            };

            this.ButtonPass.Click += (s, e) => {
                this.Status = Status.PASS;
            };

            if (!File.Exists(this.Paths.BMP)) {
                this.Status = Status.NO_TEST;
            }
            else if (File.Exists(this.Paths.Status)) {
                var statusText = File.ReadAllText(this.Paths.Status);
                this.Status = (Status)Enum.Parse(typeof(Status), statusText);
            }
            else {
                this.Status = Status.NO_TEST;
            }

            if (!File.Exists(this.Paths.XML)) File.WriteAllText(this.Paths.XML, "<root>\n</root>");
            if (!File.Exists(this.Paths.Style)) File.Create(this.Paths.Style).Close();
        }

        private void HndDragStart(object? sender, MouseEventArgs e) {
            this.Click.Invoke(this, e);
            this.DoDragDrop(this, DragDropEffects.Copy);
        }

        public void DeleteBitmap() {
            if (File.Exists(this.Paths.BMP)) File.Delete(this.Paths.BMP);
        }

        public bool CreateBitmap(out Bitmap? bitmap) {
            bitmap = null;

            string styleText = File.ReadAllText(this.Paths.Style);
            string xmlText = File.ReadAllText(this.Paths.XML);

            if (styleText.IsEmpty()) return false;
            if (xmlText.IsEmpty()) return false;

            LoadedStyles styles = StyleLoader.Load(styleText);
            Element root = XMLLoader.Load(xmlText)["root"][0];
            styles.ApplyTo(root);
            Flex flex = new();
            (int pages, RenderNode renderNode) = flex.DoLayout(root);

            if ((int)renderNode.Size.Width <= 0) throw new InvalidOperationException();
            if ((int)renderNode.Size.Height <= 0) throw new InvalidOperationException();

            bitmap = new Bitmap((int)renderNode.Size.Width, (int)renderNode.Size.Height);
            Size size = new((int)renderNode.Size.Width, (int)renderNode.Size.Height);

            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
            renderNode.Draw(graphics, 0);

            return true;
        }

        public Status Status {
            get => _status;
            set {
                File.WriteAllText(this.Paths.Status, value.ToString());
                _status = value;

                this.ButtonPass.Text = "✓";
                this.ButtonFail.Text = "✗";

                switch (value) {
                    case Status.PASS:
                        this.ButtonPass.BackColor = Color.Lime;
                        this.ButtonFail.BackColor = SystemColors.Control;
                        this.ButtonFail.Enabled = true;
                        break;
                    case Status.FAIL:
                        this.ButtonPass.BackColor = SystemColors.Control;
                        this.ButtonFail.BackColor = Color.DeepPink;
                        this.ButtonFail.Enabled = true;
                        break;
                    case Status.UNTESTED:
                    case Status.NOT_SET:
                        this.ButtonPass.BackColor = SystemColors.Control;
                        this.ButtonFail.BackColor = SystemColors.Control;
                        this.ButtonFail.Enabled = true;
                        break;
                    case Status.NO_TEST:
                        this.ButtonPass.BackColor = Color.SkyBlue;
                        this.ButtonPass.Text = "?";
                        this.ButtonFail.BackColor = SystemColors.ControlDark;
                        this.ButtonFail.Enabled = false;
                        break;
                    case Status.PENDING:
                        this.ButtonPass.BackColor = Color.SkyBlue;
                        this.ButtonPass.Text = "?";
                        this.ButtonFail.BackColor = Color.SkyBlue;
                        this.ButtonFail.Text = "?";
                        break;
                }
            }
        }

        public void Rename(string newName) {
            Paths pathsTo = new(this.Paths.Dir, newName);

            if (File.Exists(pathsTo.XML)) {
                MessageBox.Show("Test Already Exists", "Warning", MessageBoxButtons.OK);
                return;
            }

            File.Move(this.Paths.XML, pathsTo.XML);
            File.Move(this.Paths.Style, pathsTo.Style);
            if (File.Exists(this.Paths.BMP)) File.Move(this.Paths.BMP, pathsTo.BMP);

            this.Text = newName;
        }

        internal void DeleteFiles() {
            if (File.Exists(this.Paths.XML)) File.Delete(this.Paths.XML);
            if (File.Exists(this.Paths.Style)) File.Delete(this.Paths.Style);
            if (File.Exists(this.Paths.BMP)) File.Delete(this.Paths.BMP);
            if (File.Exists(this.Paths.Status)) File.Delete(this.Paths.Status);
        }

        internal void MoveFiles(DirectoryCard dirCard) {
            if (File.Exists(Path.Join(dirCard.DirPath, this.Text + ".xml"))) {
                MessageBox.Show("Test Already Exists", "Warning", MessageBoxButtons.OK);
                return;
            }

            Paths to = new(dirCard.DirPath, this.Text);

            if (File.Exists(this.Paths.XML)) File.Move(this.Paths.XML, to.XML);
            if (File.Exists(this.Paths.Style)) File.Move(this.Paths.Style, to.Style);
            if (File.Exists(this.Paths.BMP)) File.Move(this.Paths.BMP, to.BMP);
            if (File.Exists(this.Paths.Status)) File.Move(this.Paths.Status, to.Status);

            this._parentCard = dirCard;
        }

        public Status DoTest() {
            try {
                if (!File.Exists(this.Paths.BMP)) {
                    return Status.NO_TEST;
                }
                else if (this.CreateBitmap(out Bitmap? actual)) {
                    using Bitmap expected = new Bitmap(this.Paths.BMP);

                    if (AreBitmapsIdentical(actual, expected)) {
                        return Status.PASS;
                    }
                    else {
                        return Status.FAIL;
                    }
                }
                else {
                    return Status.FAIL;
                }
            }
            catch {
                return Status.FAIL;
            }
        }

        public static bool AreBitmapsIdentical(Bitmap bmp1, Bitmap bmp2) {
            // Check if the dimensions are different
            if (bmp1.Width != bmp2.Width || bmp1.Height != bmp2.Height) {
                return false;
            }

            // Compare pixel data
            for (int y = 0; y < bmp1.Height; y++) {
                for (int x = 0; x < bmp1.Width; x++) {
                    // If at least one pixel does not match, the bitmaps are not identical
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y)) {
                        return false;
                    }
                }
            }

            // If all pixels match, the bitmaps are identical
            return true;
        }

        private Status _status = Status.NOT_SET;
        private DirectoryCard _parentCard;
    }
}
