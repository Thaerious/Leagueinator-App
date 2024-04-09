﻿using System.Diagnostics;

namespace Leagueinator.VisualUnitTest {
    public partial class DirectoryCard : Card {
        public delegate void CardCopyEvent(TestCard from, DirectoryCard to);
        public event CardCopyEvent OnCopy = delegate { };

        public DirectoryCard(string directory) : base() {
            InitializeComponent();
            this.DirPath = directory;
            this.AllowDrop = true;

            this.DragEnter += this.HndDragEnter;
            this.DragDrop += this.HndDragDrop;
        }

        private void HndDragDrop(object? sender, DragEventArgs e) {
            if (e.Data is null) return;
            TestCard? data = (TestCard?)e.Data.GetData(typeof(TestCard));            
            if (data == null) return;
            Debug.WriteLine($"DirectoryCard.HndDragDrop {data.Text}");
            this.OnCopy.Invoke(data, this);            
        }

        private void HndDragEnter(object? sender, DragEventArgs e) {
            if (e.Data is null) return;

            if (e.Data.GetDataPresent(typeof(TestCard))) {
                e.Effect = DragDropEffects.Copy;
            }
            else {
                e.Effect = DragDropEffects.None;
            }
        }

        public List<Card> Cards {
            get {
                List<Card> cards = [];
                foreach (string directory in Directory.GetDirectories(this.DirPath)) {
                    cards.Add(new DirectoryCard(directory) { Text = Path.GetFileNameWithoutExtension(directory) });
                }

                string[] files = Directory.GetFiles(this.DirPath, "*.xml");

                foreach (string file in files) {
                    TestCard card = new(this, Path.GetFileNameWithoutExtension(file));
                    cards.Add(card);
                }

                return cards;
            }
        }

        public string DirPath { get; }
    }
}
