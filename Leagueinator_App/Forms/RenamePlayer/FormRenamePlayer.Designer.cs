namespace Leagueinator.App.Forms.RenamePlayer {
    partial class FormRenamePlayer {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.butOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(60, 81);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(207, 35);
            this.txtName.TabIndex = 14;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtName_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(86, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 29);
            this.label3.TabIndex = 13;
            this.label3.Text = "_player TagName";
            // 
            // butOK
            // 
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butOK.Location = new System.Drawing.Point(88, 155);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(150, 50);
            this.butOK.TabIndex = 15;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            // 
            // FormAddPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 244);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Text = "FormAddPlayer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button butOK;
    }
}
