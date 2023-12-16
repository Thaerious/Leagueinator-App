
using Leagueinator.Utility;
using System.Drawing.Printing;


namespace Leagueinator.App.Forms.Main {
    public class MatchCardPrinter : IDisposable {
        int rowHeight = 35;
        int tableWidth = 300;

        Brush lightGrayBrush = new SolidBrush(Color.LightGray);
        Pen boldBlackPen = new Pen(Color.Black, 3);
        Pen blackPen = new Pen(Color.Black, 2);
        Pen fineBlackPen = new Pen(Color.Black, 1);
        Pen grayPen = new Pen(Color.Gray, 2);
        Font font = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
        Font boldFont = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);

        private readonly LeagueEvent lEvent;
        private readonly Round round;
        private readonly int roundIndex;
        private int matchIndex = -1;
        private Match? match = null;

        public MatchCardPrinter(LeagueEvent lEvent, Round currentRound, int currentRoundIndex) {
            this.lEvent = lEvent;
            this.round = currentRound;
            this.roundIndex = currentRoundIndex;
            this.match = this.AdvanceMatch();
        }

        private Match? AdvanceMatch() {
            while (++this.matchIndex < this.round.Settings.LaneCount) {
                if (this.round.Matches[this.matchIndex].Players.Count > 0) {
                    return this.round.Matches[this.matchIndex];
                }
            }
            return null;
        }

        StringFormat centered = new StringFormat {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public void HndPrint(object sender, PrintPageEventArgs e) {
            e.HasMorePages = this.DrawNextPage(e.Graphics);
        }

        private Rectangle DrawTitle(Graphics g, Rectangle loc, int round, int lane) {
            Rectangle[] split = loc.SplitHorz(2);
            g.DrawString($"Round {round + 1}", this.boldFont, Brushes.Black, split[0], this.centered);
            g.DrawString($"Lane {lane + 1}", this.boldFont, Brushes.Black, split[1], this.centered);
            return loc;
        }

        private Rectangle DrawInfo(Graphics g, Rectangle loc, string[] names1, string[] names2) {
            Rectangle[] split = loc.SplitHorz(45, 10, 45);

            this.DrawNames(g, split[0], names1);
            g.DrawString("VS", this.font, Brushes.Black, split[1], this.centered);
            this.DrawNames(g, split[2], names2);

            g.DrawRectangle(this.boldBlackPen, loc);
            return loc;
        }

        private Rectangle DrawNames(Graphics g, Rectangle loc, string[] names) {
            Rectangle[] split = loc.SplitVert(names.Length).Shrink(10);

            for (int i = 0; i < names.Length; i++) {
                g.DrawString(names[i], this.font, Brushes.Black, split[i], this.centered);
                g.DrawRectangle(this.blackPen, split[i]);
            }

            return loc;
        }

        private Rectangle DrawTable(Graphics g, Rectangle loc, int numEnds) {
            Rectangle[] split = loc.SplitVert(numEnds + 1);
            this.DrawHeader(g, split[0]);

            for (int i = 0; i < numEnds; i++) {
                this.DrawRow(g, split[i + 1], i + 1);
            }

            g.DrawRectangle(this.boldBlackPen, loc);

            return loc;
        }

        private Rectangle DrawHeader(Graphics g, Rectangle loc) {
            string[] strings = new string[5] { "Shots", "Total", "End", "Shots", "Total" };
            Rectangle[] split = loc.SplitHorz(5);

            g.FillRectangle(this.lightGrayBrush, loc);

            for (int i = 0; i < strings.Length; i++) {
                g.DrawString(strings[i], this.font, Brushes.Black, split[i], this.centered);
                g.DrawRectangle(this.blackPen, split[i]);
            }

            return loc;
        }

        private Rectangle DrawRow(Graphics g, Rectangle loc, int lane) {
            Rectangle[] split = loc.SplitHorz(5);

            g.FillRectangle(this.lightGrayBrush, split[2]);

            foreach (Rectangle r in split) {
                g.DrawRectangle(this.fineBlackPen, r);
            }

            g.DrawString(lane.ToString(), this.font, Brushes.Black, split[2], this.centered);

            return loc;
        }

        private Rectangle DrawCard(Graphics g, Point offset, Match match, int lane, int roundIDX) {
            string[] n1 = match.Teams[0].Players.Values.Select(pi => pi.Name).ToArray();
            string[] n2 = match.Teams[1].Players.Values.Select(pi => pi.Name).ToArray();
            int numEnds = match.Settings.NumberOfEnds;

            var rect1 = this.DrawTitle(g, new Rectangle(offset.X, offset.Y, this.tableWidth, 40), roundIDX, lane);
            var rect2 = this.DrawInfo(g, rect1.Below(this.rowHeight * match.Settings.TeamSize), n1, n2);
            var rect3 = this.DrawTable(g, rect2.Below(this.rowHeight * (numEnds + 1)).MoveDown(5), numEnds);

            var rect4 = rect3.Below(50);
            g.DrawString(this.lEvent.Name, this.font, Brushes.Black, rect4, this.centered);

            Rectangle area = RectangleExtensions.Merge(rect1, rect2, rect3, rect4);

            return area;
        }

        /// <summary>
        /// Main entry point for printing.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="round"></param>
        /// <param name="roundIDX"></param>
        /// <returns>True if there are more pages to print, otherwise false</returns>
        private bool DrawNextPage(Graphics g) {
            int countCardsPrinted = 0;
            var offset = new Point(50, 50); // location of next card

            //OnDraw each card on the current page.
            while (this.match != null) {
                var area = this.DrawCard(g, offset, this.match, this.matchIndex, this.roundIndex);
                this.match = this.AdvanceMatch();
                countCardsPrinted++;

                if (countCardsPrinted % 2 == 1) {
                    // Switch from left to right side of page
                    offset.X += area.Width + 100;
                }
                else {
                    // Move down one card length
                    offset.X = 50;
                    offset.Y += area.Height + 50;
                }

                if (offset.Y + area.Height > 1100) return true;
            }

            return false;
        }

        public void Dispose() {
            this.lightGrayBrush.Dispose();
            this.blackPen.Dispose();
            this.grayPen.Dispose();
            this.font.Dispose();
        }
    }
}
