using TutorialApp2;

namespace DeleteMe2 {
    public partial class Form1 : Form {
        private MatchCard matchCard;

        public Form1() {
            InitializeComponent();
            var mock = new Mock();
            this.matchCard = new MatchCard(mock.LeagueEvents[0].Rounds[0].Matches[0]);
            this.matchCard.Size = new Size(500, 300);
            matchCard.Location = new Point(200, 50);
            this.Controls.Add(matchCard);
        }

        private void button1_Click(object sender, EventArgs e) {
            this.flowLayoutPanel1.SuspendLayout();

            this.flowLayoutPanel1.Controls.Add(new Button());

            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.matchCard.flowLeft.AddTextBox();
        }
    }
}
