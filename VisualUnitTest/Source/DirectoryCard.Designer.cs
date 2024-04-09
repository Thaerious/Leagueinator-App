namespace Leagueinator.VisualUnitTest {
    partial class DirectoryCard {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryCard));
            this.ToolTip = new ToolTip(this.components);
            this.label1 = new Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Image = (Image)resources.GetObject("label1.Image");
            this.label1.Location = new Point(321, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = RightToLeft.Yes;
            this.label1.Size = new Size(76, 55);
            this.label1.TabIndex = 1;
            // 
            // DirectoryCard
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            this.Controls.Add(this.label1);
            this.Name = "DirectoryCard";
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private ToolTip ToolTip;
        private Label label1;
    }
}
