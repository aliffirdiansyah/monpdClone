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
            btnTarik = new Button();
            consoleLog = new RichTextBox();
            dataListBox = new CheckedListBox();
            btnRefresh = new Button();
            btnSelectAll = new Button();
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
            // consoleLog
            // 
            consoleLog.BackColor = Color.Black;
            consoleLog.Location = new Point(289, 112);
            consoleLog.Name = "consoleLog";
            consoleLog.Size = new Size(464, 326);
            consoleLog.TabIndex = 8;
            consoleLog.Text = "";
            // 
            // dataListBox
            // 
            dataListBox.CheckOnClick = true;
            dataListBox.FormattingEnabled = true;
            dataListBox.HorizontalScrollbar = true;
            dataListBox.Location = new Point(36, 148);
            dataListBox.Name = "dataListBox";
            dataListBox.ScrollAlwaysVisible = true;
            dataListBox.Size = new Size(247, 292);
            dataListBox.TabIndex = 9;
            // 
            // btnRefresh
            // 
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Location = new Point(208, 119);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 10;
            btnRefresh.Text = "REFRESH";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Cursor = Cursors.Hand;
            btnSelectAll.Location = new Point(36, 119);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(95, 23);
            btnSelectAll.TabIndex = 11;
            btnSelectAll.Text = "SELECT ALL";
            btnSelectAll.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSelectAll);
            Controls.Add(btnRefresh);
            Controls.Add(dataListBox);
            Controls.Add(consoleLog);
            Controls.Add(btnTarik);
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
        private Button btnTarik;
        private RichTextBox consoleLog;
        private CheckedListBox dataListBox;
        private Button btnRefresh;
        private Button btnSelectAll;
    }
}
