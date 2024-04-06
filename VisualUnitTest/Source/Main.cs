﻿using Leagueinator.CSSParser;
using Leagueinator.Printer.Elements;
using Leagueinator.Printer.Styles;
using Leagueinator.Printer;
using Leagueinator.Utility;
using VisualUnitTest.Source;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Leagueinator.VisualUnitTest {
    public partial class Main : Form {
        private TestCard? activeTestCard;
        TestCard? ActiveTestCard {
            get => activeTestCard;
            set {
                if (this.activeTestCard is not null) {
                    this.activeTestCard.BackColor = SystemColors.Control;
                }
                if (value is not null) {
                    value.BackColor = SystemColors.ActiveCaption;
                }
                activeTestCard = value;
            }
        }

        public string ActiveDir {
            get => this.FolderDialog.SelectedPath;
        }

        public bool IsReady {
            get {
                if (this.FolderDialog.SelectedPath.IsEmpty()) return false;
                if (this.ActiveTestCard is null) return false;
                return true;
            }
        }

        public Main() {
            InitializeComponent();
            this.PanelExpected.Paint += this.HndPanelPaint;
        }

        private void HndPanelPaint(object? sender, PaintEventArgs e) {
            e.Graphics.Clear(Color.White);
            if (!this.IsReady) return;
            Paths paths = new(this.ActiveDir, this.ActiveTestCard!.TestName);
            if (!File.Exists(paths.BMP)) return;
            Bitmap expected = LoadBitmap(paths.BMP);
            e.Graphics.DrawImage(expected, 0, 0);
            expected.Dispose();
        }

        private void AddTestCard(string text) {
            TestCard card = new TestCard {
                TestName = text
            };

            this.FlowPanelTestCards.Controls.Add(card);
            card.Click += this.HndCardClick;

            this.SetupCard(card);
        }

        private void SetupCard(TestCard card) {
            Paths paths = new(this.ActiveDir, card.TestName);

            if (File.Exists(paths.BMP)) card.Status = Status.UNTESTED;
            else card.Status = Status.NO_TEST;

            card.ButtonFail.Click += (s, e) => {
                if (File.Exists(paths.BMP)) File.Delete(paths.BMP);
                card.Status = Status.FAIL;
                this.PanelExpected.Invalidate();
            };

            card.ButtonPass.Click += (s, e) => {
                Bitmap bmp = this.CreateBitmapFromCard(card);
                bmp.Save(paths.BMP, System.Drawing.Imaging.ImageFormat.Bmp);
                Debug.WriteLine($"Save bitmap {paths.BMP}");
                this.LoadTextBoxes();
                this.PanelExpected.Invalidate();
                card.Status = Status.PASS;
            };

            try {
                var desc = XMLLoader.Load(File.ReadAllText(paths.XML))["desc"];
                if (desc.Count > 0) {
                    if (desc[0].InnerText is not null) card.ToolTipText = desc[0].InnerText!;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK);
            }
        }

        private void HndCardClick(object? sender, EventArgs? _ = null) {
            if (this.ActiveTestCard != null) this.SaveTest();
            this.ActiveTestCard = sender as TestCard;
            this.LoadTextBoxes();
            this.DrawActual();
            this.PanelExpected.Invalidate();
        }

        /// <summary>
        /// Save the contents of the xml and style text boxes.
        /// The filename is either provided by the name field or the
        /// active text card.
        /// </summary>
        /// <param name="name"></param>
        private void SaveTest(string? name = null) {
            if (!this.IsReady) return;
            Paths paths = new(this.ActiveDir, name ?? ActiveTestCard!.TestName);

            File.WriteAllText(paths.XML, this.RichTextXML.Text);
            File.WriteAllText(paths.Style, this.RichTextStyle.Text);
        }

        /// <summary>
        /// Load the named test into the xml and style text boxes.
        /// If the name is omitted get the name from the active test card.
        /// </summary>
        /// <param name="name"></param>
        private void LoadTextBoxes(string? name = null) {
            if (!this.IsReady) return;
            Paths paths = new(this.ActiveDir, name ?? ActiveTestCard!.TestName);

            this.RichTextXML.Text = File.ReadAllText(paths.XML);
            this.RichTextStyle.Text = File.ReadAllText(paths.Style);
        }

        private void HndMenuCloseClick(object sender, EventArgs e) {
            this.SaveTest();
            this.Close();
        }

        private void HndMenuLoadClick(object sender, EventArgs e) {
            if (this.FolderDialog.ShowDialog() == DialogResult.OK) {
                this.LoadDirectory();
            };
        }

        private void HndMenuAddTest(object sender, EventArgs e) {
            if (this.FolderDialog.SelectedPath.IsEmpty()) return;

            using InputNameDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK) {
                Paths paths = new(this.ActiveDir, dialog.TestName);
                File.CreateText(paths.XML).Close();
                File.CreateText(paths.Style).Close();
                this.AddTestCard(dialog.TestName);
            }
        }

        private void LoadDirectory() {
            this.FlowPanelTestCards.Controls.Clear();
            string[] files = Directory.GetFiles(this.FolderDialog.SelectedPath, "*.xml");

            foreach (string file in files) {
                var filename = Path.GetFileNameWithoutExtension(file);
                this.AddTestCard(filename);
            }

            if (files.Length > 0) {
                this.HndCardClick(this.FlowPanelTestCards.Controls[0], null);
            }

            this.Text = this.FolderDialog.SelectedPath;
        }

        private void DrawActual() {
            try {
                LoadedStyles styles = StyleLoader.Load(this.RichTextStyle.Text);
                Element root = XMLLoader.Load(this.RichTextXML.Text)["root"][0];
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

        private static Bitmap LoadBitmap(string imagePath) {
            using FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            using MemoryStream ms = new MemoryStream(bytes);
            return new Bitmap(ms);
        }

        async private void HndMenuRunSelected(object sender, EventArgs e) {
            if (!this.IsReady) return;
            Paths paths = new(this.ActiveDir, this.ActiveTestCard!.TestName);
            this.ActiveTestCard.Status = await DoTest(paths);
        }

        private void HndMenuRenameTest(object sender, EventArgs e) {
            if (!this.IsReady) return;

            using InputNameDialog dialog = new();
            if (dialog.ShowDialog(ActiveTestCard!.TestName) == DialogResult.OK) {
                Paths pathsFrom = new(this.ActiveDir, ActiveTestCard!.TestName);
                Paths pathsTo = new(this.ActiveDir, dialog.TestName);

                if (File.Exists(pathsTo.XML)) {
                    MessageBox.Show("Test Already Exists", "Warning", MessageBoxButtons.OK);
                    return;
                }

                this.ActiveTestCard.TestName = dialog.TestName;

                File.Move(pathsFrom.XML, pathsTo.XML);
                File.Move(pathsFrom.Style, pathsTo.Style);
                if (File.Exists(pathsFrom.BMP)) File.Move(pathsFrom.BMP, pathsTo.BMP);
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
                paths = new(this.ActiveDir, $"copy ({i++}) of {this.ActiveTestCard!.TestName}");
            } while (File.Exists(paths.XML));

            File.WriteAllText(paths.XML, this.RichTextXML.Text);
            File.WriteAllText(paths.Style, this.RichTextStyle.Text);

            this.AddTestCard(paths.TestName);
        }

        private void HndAcceptActual(object sender, EventArgs e) {
            if (!this.IsReady) return;

            Paths paths = new(this.ActiveDir, this.ActiveTestCard!.TestName);
            using Bitmap bmp = CreateBitmapFromSource(paths);
            bmp.Save(paths.BMP, System.Drawing.Imaging.ImageFormat.Bmp);
            this.LoadTextBoxes();
            this.PanelExpected.Invalidate();
        }

        /// <summary>
        /// Create a new bitmap from a test card.
        /// Remember to dispose the bitmap after use.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        private Bitmap CreateBitmapFromCard(TestCard card) {
            if (this.FolderDialog.SelectedPath.IsEmpty()) throw new InvalidOperationException("Directory Not Loaded");
            return CreateBitmapFromSource(new Paths(this.ActiveDir, card.TestName));
        }

        private static Bitmap CreateBitmapFromSource(Paths paths) {
            LoadedStyles styles = StyleLoader.Load(File.ReadAllText(paths.Style));
            Element root = XMLLoader.Load(File.ReadAllText(paths.XML))["root"][0];
            styles.ApplyTo(root);
            Flex flex = new();
            (int pages, RenderNode renderNode) = flex.DoLayout(root);

            if ((int)renderNode.Size.Width <= 0) throw new InvalidOperationException();
            if ((int)renderNode.Size.Height <= 0) throw new InvalidOperationException();

            Bitmap bitmap = new Bitmap((int)renderNode.Size.Width, (int)renderNode.Size.Height);
            Size size = new((int)renderNode.Size.Width, (int)renderNode.Size.Height);

            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
            renderNode.Draw(graphics, 0);

            return bitmap;
        }

        private void HndMenuDelete(object sender, EventArgs e) {
            if (!this.IsReady) return;
            Paths paths = new(this.ActiveDir, this.ActiveTestCard!.TestName);

            if (File.Exists(paths.XML)) File.Delete(paths.XML);
            if (File.Exists(paths.Style)) File.Delete(paths.Style);
            if (File.Exists(paths.BMP)) File.Delete(paths.BMP);

            this.FlowPanelTestCards.Controls.Remove(this.ActiveTestCard);
        }

        public static async Task<Status> DoTest(Paths paths) {
            Status result = await Task.Run(() => {
                if (!File.Exists(paths.BMP)) return Status.NO_TEST;

                try {
                    using Bitmap actual = CreateBitmapFromSource(paths);
                    using Bitmap expected = new Bitmap(paths.BMP);

                    if (AreBitmapsIdentical(actual, expected)) return Status.PASS;
                    else return Status.FAIL;
                }
                catch {
                    return Status.FAIL;
                }
            });

            return result;
        }

        async private void HndMenuRunAll(object sender, EventArgs e) {
            // Ensure all test cards are marked as pending before starting.
            Action setStatusPending = () => {
                foreach (TestCard card in FlowPanelTestCards.Controls) {
                    card.Status = Status.PENDING;
                }
            };

            // Run the status update on the UI thread.
            Invoke(setStatusPending);

            // Invalidate the control to refresh the UI.
            Invalidate();

            foreach (TestCard card in FlowPanelTestCards.Controls) {
                card.Status = await DoTest(new Paths(this.ActiveDir, card.TestName));
            }

            // Consider refreshing UI or notifying the user upon completion.
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

        private void HndMenuClearResults(object sender, EventArgs e) {
            foreach (TestCard card in this.FlowPanelTestCards.Controls) card.Status = Status.UNTESTED;
        }

        private void HndMenuAutoFormat(object sender, EventArgs e) {
            Element root = XMLLoader.Load(this.RichTextXML.Text);
            this.RichTextXML.Text = root.ToXML().ToString();
            Debug.WriteLine(root.ToXML().ToString());
        }
    }

    public readonly struct Paths(string path, string name) {
        public readonly string XML = Path.Join(path, $"{name}.xml");
        public readonly string Style = Path.Join(path, $"{name}.style");
        public readonly string BMP = Path.Join(path, $"{name}.bmp");
        public readonly string TestName = name;
    }
}
