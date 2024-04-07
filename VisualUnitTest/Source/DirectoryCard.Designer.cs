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
            this.Label = new Label();
            this.ToolTip = new ToolTip(this.components);
            this.pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
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
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            this.pictureBox1.Location = new Point(337, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(45, 42);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // CurrentDirectoryCard
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            this.BackgroundImageLayout = ImageLayout.None;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Label);
            this.Name = "DirectoryCard";
            this.Size = new Size(400, 55);
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label Label;
        private ToolTip ToolTip;
        private PictureBox pictureBox1;
    }
}
