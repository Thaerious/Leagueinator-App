using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Leagueinator.VisualUnitTest {
    public partial class DirectoryCard : Card {

        public new EventHandler Click = delegate { };

        public readonly List<Card> Cards = [];

        private Bitmap folderImage;

        public DirectoryCard(string directory) : base(){
            InitializeComponent();
            this.Directory = directory;

            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream("VisualUnitTest.Assets.folder.png")!;
            foreach(String s in assembly.GetManifestResourceNames()) Debug.WriteLine(s);
            folderImage = new Bitmap(stream);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);            
            e.Graphics.DrawImage(folderImage, new Point(0, 0));
        }

        public string Directory { get; }
    }
}
