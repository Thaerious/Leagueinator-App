using System;
using System.Collections.Generic;

namespace Leagueinator.VisualUnitTest {
    public partial class DirectoryCard : Card {

        public new EventHandler Click = delegate { };

        public readonly List<Card> Cards = [];

        public DirectoryCard(string directory) {
            InitializeComponent();
            this.Directory = directory;
            this.Label.BackColor = Color.Transparent;
            this.Label.Click += (s, e) => this.Click.Invoke(this, e);
            base.Click += (s, e) => this.Click.Invoke(this, e);            
            this.ToolTip.SetToolTip(this, "No description provided.");
        }

        public string Directory { get; }
    }
}
