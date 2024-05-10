using System;
using System.Windows.Forms;

namespace KPFU_2_sem_programming_PaintPlusPlus {
    public partial class InputText : Form {
        public InputText() {
            InitializeComponent();
        }

        private void formInputTextButton_Click(object sender, EventArgs e) {
            if (formInputTextTextBox.Text.Length > 0 && formInputTextTextSize.Text.Length > 0) {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
