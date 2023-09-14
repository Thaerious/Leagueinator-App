using Leagueinator.Model;
using Leagueinator.Utility;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace Leagueinator.App.Forms.Main {
    public partial class FormMain {
        private void SetupFileDialog(FileDialog dialog) {
            //dialog.InitialDirectory = Properties.Settings.Default.save_dir; TODO
            dialog.Filter = "league files (*.league)|*.league|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;
        }

        private void LoadFile(string filename) {
            //try {
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    using (FileStream stream = new FileStream(filename, FileMode.Open)) {
            //        this.League = (League)formatter.Deserialize(stream);
            //    }
            //    this.filename = filename;
            //    IsSaved.Singleton.Value = true;
            //}
            //catch (Exception ex) {
            //    MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    Debug.WriteLine(ex.Message);
            //    Debug.WriteLine(ex.StackTrace);
            //}
        }

        private void SaveAs(string filename) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate)) {                
                JsonSerializer.Serialize(stream, this.League);
                this.filename = filename;
                IsSaved.Singleton.Value = true;
            }
        }

        private void File_New(object sender, EventArgs e) {
            this.League = new League();
        }

        private void File_Close(object sender, EventArgs e) {
            this.Text = "Leagueinator";
            this.League = null;
        }

        private void Menu_File_Exit(object sender, EventArgs e) {
            this.Close();
        }
        private void File_Load(object sender, EventArgs e) {
            using (OpenFileDialog dialog = new OpenFileDialog()) {
                this.SetupFileDialog(dialog);

                if (dialog.ShowDialog() == DialogResult.OK) {
                    this.LoadFile(dialog.FileName);
                }
            }
        }

        private void File_SaveAs(object sender, EventArgs e) {
            using (SaveFileDialog dialog = new SaveFileDialog()) {
                this.SetupFileDialog(dialog);

                if (dialog.ShowDialog() == DialogResult.OK) {
                    this.SaveAs(dialog.FileName);
                }
            }
        }

        private void File_Save(object sender, EventArgs e) {
            if (this.filename.IsEmpty()) {
                this.File_SaveAs(sender, e);
            }
            else {
                this.SaveAs(this.filename);
            }
        }
        private void File_Print(object sender, EventArgs e) {
            //var round = this.editEventPanel.CurrentRound;
            //if (round == null) return;
            ////ScoreCardPrinter.Print(round);

            //int currentRoundIndex = this.editEventPanel.LeagueEvent.Rounds.IndexOf(this.editEventPanel.CurrentRound);
            //var mcp = new MatchCardPrinter(round, currentRoundIndex);

            //if (this.printDialog.ShowDialog() == DialogResult.OK) {
            //    this.printDocument.PrintPage += mcp.HndPrint;
            //    this.printDocument.Print();
            //}
        }

        private void Help_About(object sender, EventArgs e) {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Debug.WriteLine(version);
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
            string msg = $"Version\n{version}\n({buildDate})";
            MessageBox.Show(msg, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
