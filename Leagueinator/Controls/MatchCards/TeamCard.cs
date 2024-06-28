using Leagueinator.Extensions;
using Leagueinator.Model.Tables;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace Leagueinator.Controls {
    public class TeamCard : Border {
        private TeamRow? _teamRow;

        public TeamCard() {
            this.AllowDrop = true;
            this.Drop += this.HndDrop;
            this.PreviewMouseDown += this.HndPreMouseDown;
            this.Loaded += this.TextBox_Loaded;
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e) {
            foreach (PlayerTextBox textBox in this.Descendants<PlayerTextBox>()) {
                textBox.AllowDrop = true;
                textBox.DragEnter += this.HndDragEnter;
                textBox.PreviewDragOver += this.HndPreviewDragOver;
            }
        }

        private void HndDragEnter(object sender, DragEventArgs e) {
            e.Effects = DragDropEffects.Copy;
        }

        private void HndPreviewDragOver(object sender, DragEventArgs e) {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;

        }

        public static readonly DependencyProperty TeamIndexProperty = DependencyProperty.Register(
            "TeamIndex",
            typeof(int),
            typeof(TeamStackPanel),
            new PropertyMetadata(default(int))
        );

        public int TeamIndex {
            get { return (int)this.GetValue(TeamIndexProperty); }
            set { this.SetValue(TeamIndexProperty, value); }
        }

        public TeamRow? TeamRow {
            get => this._teamRow;
            set {
                this._teamRow = value;
                this.Clear();
                if (this.TeamRow == null) return;

                this.DataContext = value;
                foreach (MemberRow memberRow in this.TeamRow.Members) {
                    this.AddName(memberRow.Player);
                }
            }
        }

        private void HndPreMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.Source is TextBox) return;
            if (e.Source is CheckBox) return;
            e.Handled = true;
            DataObject dataObject = new DataObject(DataFormats.Serializable, this);
            DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Move);
        }

        private void HndDrop(object sender, DragEventArgs e) {
            e.Handled = true;
            TeamCard srcCard = (TeamCard)e.Data.GetData(DataFormats.Serializable);
            TeamRow? srcRow = srcCard.TeamRow ?? throw new NullReferenceException();

            List<string> srcNames = [];
            while (srcRow.Members.Count > 0) {
                srcNames.Add(srcRow.Members[0]!.Player);
                srcRow.Members[0]!.Remove();
            }

            List<string> destNames = [];
            while (this.TeamRow!.Members.Count > 0) {
                destNames.Add(this.TeamRow.Members[0]!.Player);
                this.TeamRow!.Members[0]!.Remove();
            }

            srcCard.Clear();
            this.Clear();

            foreach (string name in srcNames) {
                this.TeamRow.Members.Add(name);
                this.AddName(name);
            }
            foreach (string name in destNames) {
                srcRow.Members.Add(name);
                srcCard.AddName(name);
            }
        }


        public void AddName(string name) {
            foreach (PlayerTextBox textBox in this.Descendants<PlayerTextBox>()) {
                if (textBox.Text.IsEmpty()) {
                    textBox.Text = name;
                    return;
                }
            }
            throw new IndexOutOfRangeException("No available text boxes.");
        }

        internal void RemoveName(string name) {
            foreach (PlayerTextBox textBox in this.Descendants<PlayerTextBox>()) {
                if (textBox.Text == name) textBox.Text = "";
            }
        }

        public void Clear() {
            foreach (PlayerTextBox textBox in this.Descendants<PlayerTextBox>()) {
                textBox.Clear();
            }
        }
    }
}
