using Leagueinator.Printer;
using System.Diagnostics;

namespace PrinterTestForm {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            this.canvas.SetupPrintElements();
            Debug.Write($"{this.canvas.GetHashCode().ToString("X")} {this.canvas.Width}, {this.canvas.Height}");
        }

        private void Menu_Direction_Row(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Flex_Direction = Flex_Direction.Row;
            this.Refresh();
        }

        private void Menu_Directon_RowReverse(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Flex_Direction = Flex_Direction.Row_reverse;
            this.Refresh();
        }

        private void Menu_Directon_Column(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Flex_Direction = Flex_Direction.Column;
            this.Refresh();
        }

        private void Menu_Directon_ColumnReverse(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Flex_Direction = Flex_Direction.Column_reverse;
            this.Refresh();
        }

        private void Menu_Justify_FlexStart(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Justify_Content = Justify_Content.Flex_start;
            this.Refresh();
        }
        private void Menu_Justify_FlexEnd(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Justify_Content = Justify_Content.Flex_end;
            this.Refresh();
        }
        private void Menu_Justify_Center(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Justify_Content = Justify_Content.Center;
            this.Refresh();
        }
        private void Menu_Justify_SpaceEvenly(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Justify_Content = Justify_Content.Space_evenly;
            this.Refresh();
        }
        private void Menu_Justify_SpaceBetween(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Justify_Content = Justify_Content.Space_between;
            this.Refresh();
        }
        private void Menu_Justify_SpaceAround(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Justify_Content = Justify_Content.Space_around;
            this.Refresh();
        }
        private void Menu_Align_Start(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Align_Items = Align_Items.Flex_start;
            this.Refresh();
        }

        private void Menu_Align_End(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Align_Items = Align_Items.Flex_end;
            this.Refresh();
        }

        private void Menu_Align_Center(object sender, EventArgs e) {
            this.canvas.ContainerElement.Style.Align_Items = Align_Items.Center;
            this.Refresh();
        }
    }
}
