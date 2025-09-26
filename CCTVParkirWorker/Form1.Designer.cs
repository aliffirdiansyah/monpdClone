namespace CCTVParkirWorker
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridView1 = new DataGridView();
            Id = new DataGridViewTextBoxColumn();
            Nop = new DataGridViewTextBoxColumn();
            Nama = new DataGridViewTextBoxColumn();
            Alamat = new DataGridViewTextBoxColumn();
            Uptb = new DataGridViewTextBoxColumn();
            CctvId = new DataGridViewTextBoxColumn();
            AccessPoint = new DataGridViewTextBoxColumn();
            Mode = new DataGridViewTextBoxColumn();
            LastConnected = new DataGridViewTextBoxColumn();
            Action = new DataGridViewButtonColumn();
            Status = new DataGridViewTextBoxColumn();
            Error = new DataGridViewTextBoxColumn();
            Log = new DataGridViewTextBoxColumn();
            btnStartAll = new Button();
            btnStopAll = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Id, Nop, Nama, Alamat, Uptb, CctvId, AccessPoint, Mode, LastConnected, Action, Status, Error, Log });
            dataGridView1.Location = new Point(12, 77);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(776, 361);
            dataGridView1.TabIndex = 0;
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.Name = "Id";
            // 
            // Nop
            // 
            Nop.HeaderText = "Nop";
            Nop.Name = "Nop";
            // 
            // Nama
            // 
            Nama.HeaderText = "Nama";
            Nama.Name = "Nama";
            // 
            // Alamat
            // 
            Alamat.HeaderText = "Alamat";
            Alamat.Name = "Alamat";
            // 
            // Uptb
            // 
            Uptb.HeaderText = "Uptb";
            Uptb.Name = "Uptb";
            // 
            // CctvId
            // 
            CctvId.HeaderText = "Cctv Id";
            CctvId.Name = "CctvId";
            // 
            // AccessPoint
            // 
            AccessPoint.HeaderText = "Access Point";
            AccessPoint.Name = "AccessPoint";
            // 
            // Mode
            // 
            Mode.HeaderText = "Mode";
            Mode.Name = "Mode";
            // 
            // LastConnected
            // 
            LastConnected.HeaderText = "Last Connected";
            LastConnected.Name = "LastConnected";
            // 
            // Action
            // 
            Action.HeaderText = "Action";
            Action.Name = "Action";
            Action.Resizable = DataGridViewTriState.True;
            Action.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Status
            // 
            dataGridViewCellStyle2.BackColor = Color.Red;
            Status.DefaultCellStyle = dataGridViewCellStyle2;
            Status.HeaderText = "Status";
            Status.Name = "Status";
            // 
            // Error
            // 
            Error.HeaderText = "Err Message";
            Error.Name = "Error";
            // 
            // Log
            // 
            Log.HeaderText = "Log";
            Log.Name = "Log";
            // 
            // btnStartAll
            // 
            btnStartAll.Location = new Point(32, 28);
            btnStartAll.Name = "btnStartAll";
            btnStartAll.Size = new Size(90, 23);
            btnStartAll.TabIndex = 1;
            btnStartAll.Text = "Start All";
            btnStartAll.UseVisualStyleBackColor = true;
            // 
            // btnStopAll
            // 
            btnStopAll.Location = new Point(682, 28);
            btnStopAll.Name = "btnStopAll";
            btnStopAll.Size = new Size(90, 23);
            btnStopAll.TabIndex = 2;
            btnStopAll.Text = "Stop All";
            btnStopAll.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnStopAll);
            Controls.Add(btnStartAll);
            Controls.Add(dataGridView1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Nop;
        private DataGridViewTextBoxColumn Nama;
        private DataGridViewTextBoxColumn Alamat;
        private DataGridViewTextBoxColumn Uptb;
        private DataGridViewTextBoxColumn CctvId;
        private DataGridViewTextBoxColumn AccessPoint;
        private DataGridViewTextBoxColumn Mode;
        private DataGridViewTextBoxColumn LastConnected;
        private DataGridViewButtonColumn Action;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn Error;
        private DataGridViewTextBoxColumn Log;
        private Button btnStartAll;
        private Button btnStopAll;
    }
}
