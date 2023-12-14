namespace Leagueinator.App.Forms.AddEvent {
    partial class FormAddEvent {
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
            label2 = new Label();
            label1 = new Label();
            butOK = new Button();
            spinLaneCount = new NumericUpDown();
            spinTeamSize = new NumericUpDown();
            label3 = new Label();
            txtName = new TextBox();
            dateTimePicker = new DateTimePicker();
            butCancel = new Button();
            label4 = new Label();
            spinNumEnds = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)spinLaneCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinTeamSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinNumEnds).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(140, 145);
            label2.Name = "label2";
            label2.Size = new Size(148, 25);
            label2.TabIndex = 10;
            label2.Text = "Number of Lanes";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(161, 80);
            label1.Name = "label1";
            label1.Size = new Size(89, 25);
            label1.TabIndex = 9;
            label1.Text = "Team Size";
            // 
            // butOK
            // 
            butOK.DialogResult = DialogResult.OK;
            butOK.Location = new Point(35, 369);
            butOK.Margin = new Padding(3, 4, 3, 4);
            butOK.Name = "butOK";
            butOK.Size = new Size(164, 56);
            butOK.TabIndex = 8;
            butOK.Text = "OK";
            butOK.UseVisualStyleBackColor = true;
            // 
            // spinLaneCount
            // 
            spinLaneCount.Location = new Point(183, 174);
            spinLaneCount.Margin = new Padding(3, 4, 3, 4);
            spinLaneCount.Maximum = new decimal(new int[] { 32, 0, 0, 0 });
            spinLaneCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinLaneCount.Name = "spinLaneCount";
            spinLaneCount.Size = new Size(51, 31);
            spinLaneCount.TabIndex = 7;
            spinLaneCount.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // spinTeamSize
            // 
            spinTeamSize.Location = new Point(183, 109);
            spinTeamSize.Margin = new Padding(3, 4, 3, 4);
            spinTeamSize.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
            spinTeamSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinTeamSize.Name = "spinTeamSize";
            spinTeamSize.Size = new Size(51, 31);
            spinTeamSize.TabIndex = 6;
            spinTeamSize.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(160, 11);
            label3.Name = "label3";
            label3.Size = new Size(107, 25);
            label3.TabIndex = 11;
            label3.Text = "Event TagName";
            // 
            // txtName
            // 
            txtName.Location = new Point(99, 40);
            txtName.Margin = new Padding(3, 4, 3, 4);
            txtName.Name = "txtName";
            txtName.Size = new Size(230, 31);
            txtName.TabIndex = 12;
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(98, 292);
            dateTimePicker.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(222, 31);
            dateTimePicker.TabIndex = 13;
            // 
            // butCancel
            // 
            butCancel.DialogResult = DialogResult.Cancel;
            butCancel.Location = new Point(205, 369);
            butCancel.Margin = new Padding(3, 4, 3, 4);
            butCancel.Name = "butCancel";
            butCancel.Size = new Size(169, 58);
            butCancel.TabIndex = 14;
            butCancel.Text = "Cancel";
            butCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(140, 211);
            label4.Name = "label4";
            label4.Size = new Size(142, 25);
            label4.TabIndex = 19;
            label4.Text = "Number of Ends";
            // 
            // spinNumEnds
            // 
            spinNumEnds.Location = new Point(183, 240);
            spinNumEnds.Margin = new Padding(3, 4, 3, 4);
            spinNumEnds.Maximum = new decimal(new int[] { 32, 0, 0, 0 });
            spinNumEnds.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinNumEnds.Name = "spinNumEnds";
            spinNumEnds.Size = new Size(51, 31);
            spinNumEnds.TabIndex = 18;
            spinNumEnds.Value = new decimal(new int[] { 8, 0, 0, 0 });
            // 
            // FormAddEvent
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(426, 462);
            this.Controls.Add(label4);
            this.Controls.Add(spinNumEnds);
            this.Controls.Add(butCancel);
            this.Controls.Add(dateTimePicker);
            this.Controls.Add(txtName);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(butOK);
            this.Controls.Add(spinLaneCount);
            this.Controls.Add(spinTeamSize);
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = "FormAddEvent";
            this.Text = "AddEventForm";
            ((System.ComponentModel.ISupportInitialize)spinLaneCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinTeamSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinNumEnds).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Button butOK;
        private NumericUpDown spinLaneCount;
        private NumericUpDown spinTeamSize;
        private Label label3;
        private TextBox txtName;
        private DateTimePicker dateTimePicker;
        private Button butCancel;
        private Label label4;
        private NumericUpDown spinNumEnds;
    }
}
