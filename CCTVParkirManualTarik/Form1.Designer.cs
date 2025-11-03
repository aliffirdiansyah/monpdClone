namespace CCTVParkirManualTarik
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            date1 = new DateTimePicker();
            date2 = new DateTimePicker();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            consoleLog = new TextBox();
            btnTarik = new Button();
            SuspendLayout();
            // 
            // date1
            // 
            date1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            date1.Format = DateTimePickerFormat.Short;
            date1.Location = new Point(36, 72);
            date1.Name = "date1";
            date1.Size = new Size(200, 23);
            date1.TabIndex = 0;
            // 
            // date2
            // 
            date2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            date2.Format = DateTimePickerFormat.Short;
            date2.Location = new Point(316, 72);
            date2.Name = "date2";
            date2.Size = new Size(200, 23);
            date2.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(36, 54);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 2;
            label1.Text = "tgl awal";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(267, 78);
            label2.Name = "label2";
            label2.Size = new Size(24, 15);
            label2.TabIndex = 3;
            label2.Text = "s/d";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(316, 54);
            label3.Name = "label3";
            label3.Size = new Size(50, 15);
            label3.TabIndex = 4;
            label3.Text = "tgl akhir";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label4.Location = new Point(218, 9);
            label4.Name = "label4";
            label4.Size = new Size(305, 25);
            label4.TabIndex = 5;
            label4.Text = "TARIKAN MANUAL CCTV PARKIR";
            // 
            // consoleLog
            // 
            consoleLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            consoleLog.BackColor = SystemColors.InfoText;
            consoleLog.ForeColor = Color.Lime;
            consoleLog.Location = new Point(36, 132);
            consoleLog.Multiline = true;
            consoleLog.Name = "consoleLog";
            consoleLog.ScrollBars = ScrollBars.Both;
            consoleLog.Size = new Size(726, 288);
            consoleLog.TabIndex = 6;
            // 
            // btnTarik
            // 
            btnTarik.BackColor = Color.SteelBlue;
            btnTarik.BackgroundImageLayout = ImageLayout.None;
            btnTarik.Cursor = Cursors.Hand;
            btnTarik.FlatAppearance.BorderSize = 0;
            btnTarik.FlatAppearance.MouseDownBackColor = Color.DodgerBlue;
            btnTarik.ForeColor = Color.White;
            btnTarik.Location = new Point(552, 72);
            btnTarik.Name = "btnTarik";
            btnTarik.Size = new Size(75, 23);
            btnTarik.TabIndex = 7;
            btnTarik.Text = "TARIK";
            btnTarik.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnTarik);
            Controls.Add(consoleLog);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(date2);
            Controls.Add(date1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker date1;
        private DateTimePicker date2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox consoleLog;
        private Button btnTarik;
    }
}
