using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Printer;
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
            this.LabelDisplayText.BackColor = Color.Transparent;

            this.MouseDown += HndDragStart;
            this.LabelDisplayText.MouseDown += HndDragStart;

            this.ButtonFail.Click += (s, e) => {
                this.Status = Status.NO_TEST;
            };

            this.ButtonPass.Click += (s, e) => {
                this.Status = Status.PASS;
            };

            if (!File.Exists(this.Paths.BMP(0))) {
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

        public void DeleteBitmaps() {
            this.AllBMPPaths(path => File.Delete(path));
        }

        public List<Bitmap> CreateBitmaps() {
            List<Bitmap> list = [];

            string xmlText = File.ReadAllText(this.Paths.XML);
            LoadedStyles styles = LoadedStyles.LoadFromFile(this.Paths.Style);
            Element root = XMLLoader.Load(xmlText)["root"][0];
            styles.AssignTo(root);
            Flex flex = new();
            List<RenderNode> pages = flex.DoLayout(root);

            foreach (RenderNode page in pages) {
                if ((int)page.Size.Width <= 0) throw new InvalidOperationException();
                if ((int)page.Size.Height <= 0) throw new InvalidOperationException();

                Bitmap bitmap = new Bitmap((int)page.Size.Width, (int)page.Size.Height);
                Size size = new((int)page.Size.Width, (int)page.Size.Height);

                using Graphics graphics = Graphics.FromImage(bitmap);
                graphics.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
                page.Draw(graphics);

                list.Add(bitmap);
            }

            return list;
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

        public void AllBMPPaths(Action<string, int> action) {
            int i = 0;
            string path = Path.Join(this.Paths.Dir, $"{this.Paths.TestName}.0.bmp");
            while (File.Exists(path)) {
                action(path, i);
                path = Path.Join(this.Paths.Dir, $"{this.Paths.TestName}.{++i}.bmp");
            }
        }

        public void AllBMPPaths(Action<string> action) {
            int i = 0;
            string path = Path.Join(this.Paths.Dir, $"{this.Paths.TestName}.0.bmp");
            while (File.Exists(path)) {
                action(path);
                path = Path.Join(this.Paths.Dir, $"{this.Paths.TestName}.{++i}.bmp");
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
            this.AllBMPPaths((path, i) => File.Move(path, pathsTo.BMP(i)));
            this.Text = newName;
        }

        internal void DeleteFiles() {
            if (File.Exists(this.Paths.XML)) File.Delete(this.Paths.XML);
            if (File.Exists(this.Paths.Style)) File.Delete(this.Paths.Style);
            this.AllBMPPaths(path => File.Delete(path));
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
            this.AllBMPPaths((path, i) => File.Move(path, to.BMP(i)));
            if (File.Exists(this.Paths.Status)) File.Move(this.Paths.Status, to.Status);

            this._parentCard = dirCard;
        }

        public Status DoTest() {
            List<Bitmap> actual = this.CreateBitmaps();
            if (actual.Count == 0) return Status.NO_TEST;

            for (int i = 0; i < actual.Count; i++) {
                string path = this.Paths.BMP(i);
                if (!File.Exists(path)) {
                    return Status.FAIL;
                }
                using Bitmap expected = new Bitmap(path);

                if (!AreBitmapsIdentical(actual[i], expected)) return Status.FAIL;
            }

            return Status.PASS;
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
