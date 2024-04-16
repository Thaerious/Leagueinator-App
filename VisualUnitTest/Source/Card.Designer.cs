namespace Leagueinator.VisualUnitTest {
    partial class Card {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.LabelDisplayText = new Label();
            this.ToolTip = new ToolTip(this.components);
            this.SuspendLayout();
            // 
            // LabelDisplayText
            // 
            this.LabelDisplayText.AutoSize = true;
            this.LabelDisplayText.BackColor = SystemColors.Control;
            this.LabelDisplayText.Font = new Font("Consolas", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.LabelDisplayText.Location = new Point(3, 12);
            this.LabelDisplayText.Name = "LabelDisplayText";
            this.LabelDisplayText.Size = new Size(105, 33);
            this.LabelDisplayText.TabIndex = 0;
            this.LabelDisplayText.Text = "label1";
            // 
            // Card
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            this.BackgroundImageLayout = ImageLayout.None;
            this.Controls.Add(this.LabelDisplayText);
            this.Name = "Card";
            this.Size = new Size(400, 55);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private ToolTip ToolTip;
        internal Label LabelDisplayText;
    }
}
