
using System.Diagnostics;
using System.Windows.Forms;

namespace Leagueinator.VisualUnitTest {
    internal class CustomRichTextBox : RichTextBox {
        public CustomRichTextBox() : base() {
            this.KeyDown += HndKeyDown;
        }

        private void HndKeyDown(object? sender, KeyEventArgs e) {
            if (sender is null) return;
            RichTextBox richTextBox = (sender as RichTextBox)!;

            if (e.KeyCode == Keys.Tab) {
                if (richTextBox.SelectionLength == 0 && !e.Shift) return;

                e.SuppressKeyPress = true; // Prevent the default tab behavior
                if (richTextBox == null) return;

                // Calculate original selection range
                int selectionStart = richTextBox.SelectionStart;
                int selectionLength = richTextBox.SelectionLength;
                int selectionEnd = selectionStart + selectionLength;

                // Calculate start and end lines of the selection
                int startLine = richTextBox.GetLineFromCharIndex(selectionStart);
                int endLine = richTextBox.GetLineFromCharIndex(selectionEnd - 1); // Adjusted to ensure we don't jump to the next line

                if (e.Shift) {
                    // Shift+Tab pressed: Unindent the selected lines
                    for (int i = startLine; i <= endLine; i++) {
                        int lineStartIndex = richTextBox.GetFirstCharIndexFromLine(i);
                        if (richTextBox.Lines[i].StartsWith("\t")) {
                            richTextBox.Select(lineStartIndex, 1); // Select the tab character
                            richTextBox.SelectedText = ""; // Remove the tab

                            // Adjust the selection start if the first line was changed
                            if (i == startLine) {
                                selectionStart -= 1;
                            }
                            selectionLength -= 1;
                        }
                    }
                }
                else {
                    // Tab pressed: Indent the selected lines
                    for (int i = startLine; i <= endLine; i++) {
                        int lineStartIndex = richTextBox.GetFirstCharIndexFromLine(i);
                        richTextBox.Select(lineStartIndex, 0);
                        richTextBox.SelectedText = "\t";

                        // Adjust the selection start if the first line was changed
                        if (i == startLine) {
                            selectionStart += 1;
                        }
                        selectionLength += 1;
                    }
                }

                // Restore the original selection
                richTextBox.Select(selectionStart, selectionLength - 1);
            }
        }
    }
}
