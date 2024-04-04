﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Leagueinator.VisualUnitTest {
    public partial class InputNameDialog : Form {
        public string TestName {
            get => this.TextName.Text;
        }

        public InputNameDialog() {
            InitializeComponent();
        }

        public DialogResult Show(string name) {
            this.TextName.Text = name;
            return this.ShowDialog();
        }

        private void HndKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.ButtonOk.PerformClick();
            }
        }
    }
}
