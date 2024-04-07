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
            this.Label = new Label();
            this.ToolTip = new ToolTip(this.components);
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.BackColor = SystemColors.Control;
            this.Label.Font = new Font("Consolas", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Label.Location = new Point(3, 12);
            this.Label.Name = "Label";
            this.Label.Size = new Size(105, 33);
            this.Label.TabIndex = 0;
            this.Label.Text = "label1";
            // 
            // Card
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            this.BackgroundImageLayout = ImageLayout.None;
            this.Controls.Add(this.Label);
            this.Name = "Card";
            this.Size = new Size(400, 55);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label Label;
        private ToolTip ToolTip;
    }
}
