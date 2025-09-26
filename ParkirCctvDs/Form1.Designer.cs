namespace ParkirCctvDs
{
    partial class ParkirCctv
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
            cbWilayah = new ComboBox();
            textBox1 = new TextBox();
            wil = new Label();
            interval = new Label();
            btnView = new Button();
            dataGridView1 = new DataGridView();
            CheckBox = new DataGridViewCheckBoxColumn();
            Nop = new DataGridViewTextBoxColumn();
            Nama = new DataGridViewTextBoxColumn();
            Uptb = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            LastRun = new DataGridViewTextBoxColumn();
            Message = new DataGridViewTextBoxColumn();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // cbWilayah
            // 
            cbWilayah.FormattingEnabled = true;
            cbWilayah.Location = new Point(121, 49);
            cbWilayah.Name = "cbWilayah";
            cbWilayah.Size = new Size(194, 23);
            cbWilayah.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(443, 49);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 1;
            // 
            // wil
            // 
            wil.AutoSize = true;
            wil.Location = new Point(66, 52);
            wil.Name = "wil";
            wil.Size = new Size(49, 15);
            wil.TabIndex = 2;
            wil.Text = "Wilayah";
            // 
            // interval
            // 
            interval.AutoSize = true;
            interval.Location = new Point(341, 52);
            interval.Name = "interval";
            interval.Size = new Size(96, 15);
            interval.TabIndex = 3;
            interval.Text = "Interval (Second)";
            // 
            // btnView
            // 
            btnView.Location = new Point(586, 49);
            btnView.Name = "btnView";
            btnView.Size = new Size(75, 23);
            btnView.TabIndex = 4;
            btnView.Text = "View";
            btnView.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { CheckBox, Nop, Nama, Uptb, Status, LastRun, Message });
            dataGridView1.Location = new Point(12, 78);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(776, 322);
            dataGridView1.TabIndex = 5;
            // 
            // CheckBox
            // 
            CheckBox.FillWeight = 35.5329933F;
            CheckBox.HeaderText = "#";
            CheckBox.Name = "CheckBox";
            // 
            // Nop
            // 
            Nop.FillWeight = 110.744492F;
            Nop.HeaderText = "Nop";
            Nop.Name = "Nop";
            // 
            // Nama
            // 
            Nama.FillWeight = 110.744492F;
            Nama.HeaderText = "Nama";
            Nama.Name = "Nama";
            // 
            // Uptb
            // 
            Uptb.FillWeight = 110.744492F;
            Uptb.HeaderText = "Uptb";
            Uptb.Name = "Uptb";
            // 
            // Status
            // 
            Status.FillWeight = 110.744492F;
            Status.HeaderText = "Status";
            Status.Name = "Status";
            // 
            // LastRun
            // 
            LastRun.FillWeight = 110.744492F;
            LastRun.HeaderText = "Last Run";
            LastRun.Name = "LastRun";
            // 
            // Message
            // 
            Message.FillWeight = 110.744492F;
            Message.HeaderText = "Message";
            Message.Name = "Message";
            // 
            // button1
            // 
            button1.Location = new Point(713, 415);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 6;
            button1.Text = "START";
            button1.UseVisualStyleBackColor = true;
            // 
            // ParkirCctv
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Controls.Add(btnView);
            Controls.Add(interval);
            Controls.Add(wil);
            Controls.Add(textBox1);
            Controls.Add(cbWilayah);
            Name = "ParkirCctv";
            Text = "ParkirCctv";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbWilayah;
        private TextBox textBox1;
        private Label wil;
        private Label interval;
        private Button btnView;
        private DataGridView dataGridView1;
        private DataGridViewCheckBoxColumn CheckBox;
        private DataGridViewTextBoxColumn Nop;
        private DataGridViewTextBoxColumn Nama;
        private DataGridViewTextBoxColumn Uptb;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn LastRun;
        private DataGridViewTextBoxColumn Message;
        private Button button1;
    }
}
