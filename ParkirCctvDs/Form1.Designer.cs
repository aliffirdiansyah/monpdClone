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
            comboBox1 = new ComboBox();
            textBox1 = new TextBox();
            wilayahCb = new Label();
            interval = new Label();
            btnView = new Button();
            dataGridView1 = new DataGridView();
            button1 = new Button();
            CheckBox = new DataGridViewCheckBoxColumn();
            Nop = new DataGridViewTextBoxColumn();
            Nama = new DataGridViewTextBoxColumn();
            Uptb = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            LastRun = new DataGridViewTextBoxColumn();
            Message = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(121, 49);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(194, 23);
            comboBox1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(443, 49);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 1;
            // 
            // wilayahCb
            // 
            wilayahCb.AutoSize = true;
            wilayahCb.Location = new Point(66, 52);
            wilayahCb.Name = "wilayahCb";
            wilayahCb.Size = new Size(49, 15);
            wilayahCb.TabIndex = 2;
            wilayahCb.Text = "Wilayah";
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
            // button1
            // 
            button1.Location = new Point(713, 415);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 6;
            button1.Text = "START";
            button1.UseVisualStyleBackColor = true;
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
            // ParkirCctv
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Controls.Add(btnView);
            Controls.Add(interval);
            Controls.Add(wilayahCb);
            Controls.Add(textBox1);
            Controls.Add(comboBox1);
            Name = "ParkirCctv";
            Text = "ParkirCctv";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private TextBox textBox1;
        private Label wilayahCb;
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
