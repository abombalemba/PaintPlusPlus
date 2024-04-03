
namespace KPFU_2_sem_programming_PaintPlusPlus {
    partial class InputText {
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
            this.formInputTextLabel1 = new System.Windows.Forms.Label();
            this.formInputTextTextBox = new System.Windows.Forms.RichTextBox();
            this.formInputTextButton = new System.Windows.Forms.Button();
            this.formInputTextTextSize = new System.Windows.Forms.ComboBox();
            this.formInputTextLabel2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // formInputTextLabel1
            // 
            this.formInputTextLabel1.AutoSize = true;
            this.formInputTextLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.formInputTextLabel1.Location = new System.Drawing.Point(15, 15);
            this.formInputTextLabel1.Name = "formInputTextLabel1";
            this.formInputTextLabel1.Size = new System.Drawing.Size(103, 17);
            this.formInputTextLabel1.TabIndex = 0;
            this.formInputTextLabel1.Text = "Введите текст";
            // 
            // formInputTextTextBox
            // 
            this.formInputTextTextBox.Location = new System.Drawing.Point(12, 35);
            this.formInputTextTextBox.Name = "formInputTextTextBox";
            this.formInputTextTextBox.Size = new System.Drawing.Size(458, 49);
            this.formInputTextTextBox.TabIndex = 1;
            this.formInputTextTextBox.Text = "";
            // 
            // formInputTextButton
            // 
            this.formInputTextButton.Location = new System.Drawing.Point(395, 111);
            this.formInputTextButton.Name = "formInputTextButton";
            this.formInputTextButton.Size = new System.Drawing.Size(75, 30);
            this.formInputTextButton.TabIndex = 2;
            this.formInputTextButton.Text = "Далее";
            this.formInputTextButton.UseVisualStyleBackColor = true;
            this.formInputTextButton.Click += new System.EventHandler(this.formInputTextButton_Click);
            // 
            // formInputTextTextSize
            // 
            this.formInputTextTextSize.FormattingEnabled = true;
            this.formInputTextTextSize.Items.AddRange(new object[] {
            "8",
            "10",
            "12",
            "14",
            "16",
            "18",
            "20",
            "24",
            "28",
            "32",
            "26",
            "40",
            "44",
            "50"});
            this.formInputTextTextSize.Location = new System.Drawing.Point(18, 115);
            this.formInputTextTextSize.Name = "formInputTextTextSize";
            this.formInputTextTextSize.Size = new System.Drawing.Size(75, 24);
            this.formInputTextTextSize.TabIndex = 4;
            // 
            // formInputTextLabel2
            // 
            this.formInputTextLabel2.AutoSize = true;
            this.formInputTextLabel2.Location = new System.Drawing.Point(15, 87);
            this.formInputTextLabel2.Name = "formInputTextLabel2";
            this.formInputTextLabel2.Size = new System.Drawing.Size(174, 17);
            this.formInputTextLabel2.TabIndex = 5;
            this.formInputTextLabel2.Text = "Выберите размер текста";
            // 
            // InputText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 153);
            this.Controls.Add(this.formInputTextLabel2);
            this.Controls.Add(this.formInputTextTextSize);
            this.Controls.Add(this.formInputTextButton);
            this.Controls.Add(this.formInputTextTextBox);
            this.Controls.Add(this.formInputTextLabel1);
            this.Name = "InputText";
            this.Text = "Текст";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label formInputTextLabel1;
        private System.Windows.Forms.Button formInputTextButton;
        internal System.Windows.Forms.RichTextBox formInputTextTextBox;
        private System.Windows.Forms.Label formInputTextLabel2;
        internal System.Windows.Forms.ComboBox formInputTextTextSize;
    }
}