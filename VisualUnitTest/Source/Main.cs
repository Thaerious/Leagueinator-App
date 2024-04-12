using Leagueinator.CSSParser;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Printer;
using Leagueinator.Utility;
using System.Diagnostics;

namespace Leagueinator.VisualUnitTest {
    public partial class Main : Form {
        private TestCard? activeTestCard;
        private DirectoryCard? _activeDirCard;

        TestCard? ActiveTestCard {
            get => activeTestCard;
            set {
                if (this.activeTestCard is not null) {
                    this.activeTestCard.IdleColor = SystemColors.Control;
                }
                if (value is not null) {
                    value.IdleColor = SystemColors.ActiveCaption;
                }
                activeTestCard = value;
            }
        }

        public DirectoryCard? ActiveDirCard {
            get => _activeDirCard;
            set {
                if (value is null) throw new InvalidOperationException("Can not set DirectoryCard to null.");

                this.FlowPanelTestCards.Controls.Clear();

                if (_activeDirCard is not null && value.DirPath != this.FolderDialog.SelectedPath) {
                    DirectoryCard upDirCard = new(this._activeDirCard.DirPath) { Text = ".." };
                    upDirCard.Click += this.HndDirCardClick;
                    this.FlowPanelTestCards.Controls.Add(upDirCard);
                    upDirCard.OnCopy += this.HndCopyTestCard;
                }

                _activeDirCard = value;

                foreach (DirectoryCard card in value.Cards.OfType<DirectoryCard>()) {
                    card.Click += this.HndDirCardClick;
                    this.FlowPanelTestCards.Controls.Add(card);
                    card.OnCopy += this.HndCopyTestCard;
                }

                foreach (TestCard card in value.Cards.OfType<TestCard>()) {
                    this.FlowPanelTestCards.Controls.Add(card);
                    card.Click += this.HndTestCardClick;

                    card.ButtonFail.Click += (s, e) => {
                        card.DeleteBitmap();
                        this.PanelExpected.Invalidate();
                    };

                    card.ButtonPass.Click += (s, e) => {
                        if (!card.CreateBitmap(out Bitmap? bitmap)) return;
                        bitmap!.Save(card.Paths.BMP, System.Drawing.Imaging.ImageFormat.Bmp);
                        this.PanelExpected.Invalidate();
                    };
                }

                if (value.Cards.Count > 0) {
                    this.HndTestCardClick(this.FlowPanelTestCards.Controls[0], null);
                }
            }
        }

        public void HndCopyTestCard(TestCard from, DirectoryCard dir) {
            try {
                from.MoveFiles(dir);
                this.FlowPanelTestCards.Controls.Remove(from);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string ActiveDir {
            get => this.ActiveDirCard?.DirPath ?? "";
        }

        public bool IsReady {
            get {
                if (this.FolderDialog.SelectedPath.IsEmpty()) return false;
                if (this.ActiveTestCard is null) return false;
                if (this.ActiveDirCard is null) return false;
                return true;
            }
        }

        public Main() {
            InitializeComponent();
            this.PanelExpected.Paint += this.HndPanelPaint;
        }

        /// <summary>
        /// Add a new card dir the flow panel.
        /// </summary>
        /// <param name="text"></param>
        private void AddTestCard(string text) {
            if (this.ActiveDirCard is null) return;

            TestCard card = new(this.ActiveDirCard, text);
            this.ActiveDirCard!.Cards.Add(card);
            card.Click += this.HndTestCardClick;
            this.FlowPanelTestCards.Controls.Add(card);

            card.ButtonFail.Click += (s, e) => {
                card.DeleteBitmap();
                this.PanelExpected.Invalidate();
            };

            card.ButtonPass.Click += (s, e) => {
                if (!card.CreateBitmap(out Bitmap? bitmap)) return;
                bitmap!.Save(card.Paths.BMP, System.Drawing.Imaging.ImageFormat.Bmp);
                this.PanelExpected.Invalidate();
            };
        }

        private void HndPanelPaint(object? sender, PaintEventArgs e) {
            e.Graphics.Clear(Color.White);
            if (!this.IsReady) return;
            Paths paths = this.ActiveTestCard!.Paths;
            if (!File.Exists(paths.BMP)) return;
            Bitmap expected = LoadBitmap(paths.BMP);
            e.Graphics.DrawImage(expected, 0, 0);
            expected.Dispose();
        }

        private void HndTestCardClick(object? sender, EventArgs? _ = null) {
            if (this.ActiveTestCard != null) this.SaveTest();
            this.ActiveTestCard = sender as TestCard;
            this.LoadTextBoxes();
            this.DrawActual();
            this.PanelExpected.Invalidate();
        }

        private void HndDirCardClick(object? sender, EventArgs? _ = null) {
            if (sender == null) return;

            // Directly cast sender dir DirectoryCard
            DirectoryCard card = (DirectoryCard)sender;
            this.ActiveDirCard = card;
        }

        /// <summary>
        /// Save the contents of the xml and style text boxes.
        /// The filename is either provided by the name field or the
        /// active text card.
        /// </summary>
        /// <param name="name"></param>
        private void SaveTest(string? name = null) {
            if (!this.IsReady) return;
            Paths paths = this.ActiveTestCard!.Paths;
            File.WriteAllText(paths.XML, this.RichTextXML.Text);
            File.WriteAllText(paths.Style, this.RichTextStyle.Text);
        }

        /// <summary>
        /// LoadFromString the named test into the xml and style text boxes.
        /// If the name is omitted get the name from the active test card.
        /// </summary>
        /// <param name="name"></param>
        private void LoadTextBoxes(string? name = null) {
            if (!this.IsReady) return;
            Paths paths = this.ActiveTestCard!.Paths;

            this.RichTextXML.Text = File.ReadAllText(paths.XML);
            this.RichTextStyle.Text = File.ReadAllText(paths.Style);
        }

        private void HndMenuCloseClick(object sender, EventArgs e) {
            this.SaveTest();
            this.Close();
        }

        private void HndMenuLoadClick(object sender, EventArgs e) {
            if (this.FolderDialog.ShowDialog() == DialogResult.OK) {
                this.LoadFile(this.FolderDialog.SelectedPath);
            };
        }

        private void LoadFile(string path) {
            this.ActiveDirCard = new(path);
            this.Text = this.FolderDialog.SelectedPath;
            this.ActiveDirCard.OnCopy += this.HndCopyTestCard;
        }

        private void HndMenuAddTest(object sender, EventArgs e) {
            if (this.FolderDialog.SelectedPath.IsEmpty()) return;

            using InputNameDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK) {
                this.AddTestCard(dialog.TestName);
            }
        }

        private void DrawActual() {
            try {
                if (!this.IsReady) return;

                LoadedStyles styles = new LoadedStyles().LoadFromFile(this.ActiveTestCard!.Paths.Style);                
                Element root = XMLLoader.Load(this.RichTextXML.Text)["root"][0];
                styles.ApplyTo(root);

                Flex flex = new();

                (int pages, RenderNode renderNode) = flex.DoLayout(root);
                this.CanvasActual.RenderNode = renderNode;
                this.CanvasActual.Invalidate(true);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine(ex);
            }
        }

        private static Bitmap LoadBitmap(string imagePath) {
            using FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            using MemoryStream ms = new MemoryStream(bytes);
            return new Bitmap(ms);
        }

        async private void HndMenuRunSelected(object sender, EventArgs e) {
            if (!this.IsReady) return;
            this.ActiveTestCard!.Status = Status.PENDING;
            this.ActiveTestCard.Status = this.ActiveTestCard.DoTest();
        }

        private void HndMenuRenameTest(object sender, EventArgs e) {
            if (!this.IsReady) return;

            using InputNameDialog dialog = new();
            if (dialog.ShowDialog(ActiveTestCard!.Text) == DialogResult.OK) {
                this.ActiveTestCard.Rename(dialog.TestName);
            }
        }

        private void HndMenuSave(object sender, EventArgs e) {
            this.SaveTest();
            this.DrawActual();
        }

        private void HndMenuDuplicate(object sender, EventArgs e) {
            if (!this.IsReady) return;

            this.SaveTest();
            this.DrawActual();

            Paths paths;
            int i = 1;
            do {
                paths = new(this.ActiveDir, $"copy ({i++}) of {this.ActiveTestCard!.Text}");
            } while (File.Exists(paths.XML));

            File.WriteAllText(paths.XML, this.RichTextXML.Text);
            File.WriteAllText(paths.Style, this.RichTextStyle.Text);

            this.AddTestCard(paths.TestName);
        }

        private void HndMenuDelete(object sender, EventArgs e) {
            try {
                if (!this.IsReady) return;
                this.ActiveTestCard!.DeleteFiles();
                this.FlowPanelTestCards.Controls.Remove(this.ActiveTestCard);
                this.ActiveTestCard = null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine(ex);
            }
        }

        async private void HndMenuRunAll(object sender, EventArgs e) {
            // Ensure all test Cards are marked as pending before starting.
            Action setStatusPending = () => {
                foreach (TestCard card in FlowPanelTestCards.Controls.OfType<TestCard>()) {
                    card.Status = Status.PENDING;
                }
            };

            // Run the status update on the UI thread.
            Invoke(setStatusPending);

            // Invalidate the control dir refresh the UI.
            Invalidate();

            foreach (TestCard card in FlowPanelTestCards.Controls.OfType<TestCard>()) {
                card.Status = await Task.Run(() => card.DoTest());
            }

            // Consider refreshing UI or notifying the user upon completion.
        }

        private void HndMenuClearResults(object sender, EventArgs e) {
            foreach (TestCard card in FlowPanelTestCards.Controls.OfType<TestCard>()) {
                card.Status = Status.UNTESTED;
            }
        }

        private void HndMenuAutoFormat(object sender, EventArgs e) {
            Element root = XMLLoader.Load(this.RichTextXML.Text);
            this.RichTextXML.Text = root.ToXML().ToString();
        }
    }

    public readonly struct Paths(string dir, string name) {
        public readonly string Dir = dir;
        public readonly string XML = Path.Join(dir, $"{name}.xml");
        public readonly string Style = Path.Join(dir, $"{name}.style");
        public readonly string BMP = Path.Join(dir, $"{name}.bmp");
        public readonly string Status = Path.Join(dir, $"{name}.status");
        public readonly string TestName = name;
    }
}
