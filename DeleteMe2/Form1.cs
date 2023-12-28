using TutorialApp2;

namespace DeleteMe2 {
    public partial class Form1 : Form {
        private MatchCard matchCard;

        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.flowLayoutPanel1.SuspendLayout();

            var button = new Button() {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                AutoSize = true,
            };

            this.flowLayoutPanel1.Controls.Add(new Button());

            this.flowLayoutPanel1.ResumeLayout(true);
        }
    }
}
