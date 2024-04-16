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
            this.labelImage = new Label();
            this.labelCount = new Label();
            this.SuspendLayout();
            // 
            // labelImage
            // 
            this.labelImage.Image = (Image)resources.GetObject("labelImage.Image");
            this.labelImage.Location = new Point(269, 0);
            this.labelImage.Name = "labelImage";
            this.labelImage.RightToLeft = RightToLeft.Yes;
            this.labelImage.Size = new Size(76, 55);
            this.labelImage.TabIndex = 1;
            // 
            // labelCount
            // 
            this.labelCount.BackColor = Color.Transparent;
            this.labelCount.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelCount.Location = new Point(351, 1);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new Size(46, 50);
            this.labelCount.TabIndex = 2;
            this.labelCount.Text = "0";
            this.labelCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // DirectoryCard
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Control;
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.labelImage);
            this.Name = "DirectoryCard";
            this.Controls.SetChildIndex(this.labelImage, 0);
            this.Controls.SetChildIndex(this.labelCount, 0);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private ToolTip ToolTip;
        private Label labelImage;
        private Label labelCount;
    }
}
