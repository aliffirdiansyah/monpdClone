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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dataGridView1 = new DataGridView();
            No = new DataGridViewTextBoxColumn();
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
            tabControl1 = new TabControl();
            TarikanCctvJasnita = new TabPage();
            label2 = new Label();
            TarikanLogJasnita = new TabPage();
            dataGridView2 = new DataGridView();
            NoLog = new DataGridViewTextBoxColumn();
            IdLog = new DataGridViewTextBoxColumn();
            NopLog = new DataGridViewTextBoxColumn();
            NamaLog = new DataGridViewTextBoxColumn();
            AlamatLog = new DataGridViewTextBoxColumn();
            UptbLog = new DataGridViewTextBoxColumn();
            CctvIdLog = new DataGridViewTextBoxColumn();
            AccessPointLog = new DataGridViewTextBoxColumn();
            ModeLog = new DataGridViewTextBoxColumn();
            LastConnectedLog = new DataGridViewTextBoxColumn();
            ActionLog = new DataGridViewButtonColumn();
            StatusLog = new DataGridViewTextBoxColumn();
            ErrorLog = new DataGridViewTextBoxColumn();
            LogLog = new DataGridViewTextBoxColumn();
            label1 = new Label();
            btnStopAllLogJasnita = new Button();
            btnStartAllLogJasnita = new Button();
            TarikanCctvTelkom = new TabPage();
            label3 = new Label();
            btnStartAllTelkom = new Button();
            btnStopAllTelkom = new Button();
            dataGridView3 = new DataGridView();
            NoTelkom = new DataGridViewTextBoxColumn();
            IdTelkom = new DataGridViewTextBoxColumn();
            NopTelkom = new DataGridViewTextBoxColumn();
            NamaTelkom = new DataGridViewTextBoxColumn();
            AlamatTelkom = new DataGridViewTextBoxColumn();
            UptbTelkom = new DataGridViewTextBoxColumn();
            CctvIdTelkom = new DataGridViewTextBoxColumn();
            ModeTelkom = new DataGridViewTextBoxColumn();
            LastConnectedTelkom = new DataGridViewTextBoxColumn();
            ActionTelkom = new DataGridViewButtonColumn();
            StatusTelkom = new DataGridViewTextBoxColumn();
            ErrorTelkom = new DataGridViewTextBoxColumn();
            LogTelkom = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabControl1.SuspendLayout();
            TarikanCctvJasnita.SuspendLayout();
            TarikanLogJasnita.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            TarikanCctvTelkom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { No, Id, Nop, Nama, Alamat, Uptb, CctvId, AccessPoint, Mode, LastConnected, Action, Status, Error, Log });
            dataGridView1.Location = new Point(3, 62);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(746, 330);
            dataGridView1.TabIndex = 0;
            // 
            // No
            // 
            No.HeaderText = "No";
            No.Name = "No";
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
            dataGridViewCellStyle1.BackColor = Color.Red;
            Status.DefaultCellStyle = dataGridViewCellStyle1;
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
            btnStartAll.Location = new Point(15, 20);
            btnStartAll.Name = "btnStartAll";
            btnStartAll.Size = new Size(90, 23);
            btnStartAll.TabIndex = 1;
            btnStartAll.Text = "Start All";
            btnStartAll.UseVisualStyleBackColor = true;
            // 
            // btnStopAll
            // 
            btnStopAll.Location = new Point(111, 20);
            btnStopAll.Name = "btnStopAll";
            btnStopAll.Size = new Size(90, 23);
            btnStopAll.TabIndex = 2;
            btnStopAll.Text = "Stop All";
            btnStopAll.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(TarikanCctvJasnita);
            tabControl1.Controls.Add(TarikanLogJasnita);
            tabControl1.Controls.Add(TarikanCctvTelkom);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(760, 426);
            tabControl1.TabIndex = 3;
            // 
            // TarikanCctvJasnita
            // 
            TarikanCctvJasnita.Controls.Add(label2);
            TarikanCctvJasnita.Controls.Add(dataGridView1);
            TarikanCctvJasnita.Controls.Add(btnStopAll);
            TarikanCctvJasnita.Controls.Add(btnStartAll);
            TarikanCctvJasnita.Location = new Point(4, 24);
            TarikanCctvJasnita.Name = "TarikanCctvJasnita";
            TarikanCctvJasnita.Padding = new Padding(3);
            TarikanCctvJasnita.Size = new Size(752, 398);
            TarikanCctvJasnita.TabIndex = 0;
            TarikanCctvJasnita.Text = "TARIKAN CCTV JASNITA";
            TarikanCctvJasnita.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label2.Location = new Point(275, 16);
            label2.Name = "label2";
            label2.Size = new Size(146, 25);
            label2.TabIndex = 4;
            label2.Text = "TARIKAN CCTV";
            // 
            // TarikanLogJasnita
            // 
            TarikanLogJasnita.Controls.Add(dataGridView2);
            TarikanLogJasnita.Controls.Add(label1);
            TarikanLogJasnita.Controls.Add(btnStopAllLogJasnita);
            TarikanLogJasnita.Controls.Add(btnStartAllLogJasnita);
            TarikanLogJasnita.Location = new Point(4, 24);
            TarikanLogJasnita.Name = "TarikanLogJasnita";
            TarikanLogJasnita.Padding = new Padding(3);
            TarikanLogJasnita.Size = new Size(752, 398);
            TarikanLogJasnita.TabIndex = 1;
            TarikanLogJasnita.Text = "TARIKAN LOG JASNITA";
            TarikanLogJasnita.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            dataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { NoLog, IdLog, NopLog, NamaLog, AlamatLog, UptbLog, CctvIdLog, AccessPointLog, ModeLog, LastConnectedLog, ActionLog, StatusLog, ErrorLog, LogLog });
            dataGridView2.Location = new Point(3, 65);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(746, 327);
            dataGridView2.TabIndex = 4;
            // 
            // NoLog
            // 
            NoLog.HeaderText = "No";
            NoLog.Name = "NoLog";
            // 
            // IdLog
            // 
            IdLog.HeaderText = "Id";
            IdLog.Name = "IdLog";
            // 
            // NopLog
            // 
            NopLog.HeaderText = "Nop";
            NopLog.Name = "NopLog";
            // 
            // NamaLog
            // 
            NamaLog.HeaderText = "Nama";
            NamaLog.Name = "NamaLog";
            // 
            // AlamatLog
            // 
            AlamatLog.HeaderText = "Alamat";
            AlamatLog.Name = "AlamatLog";
            // 
            // UptbLog
            // 
            UptbLog.HeaderText = "Uptb";
            UptbLog.Name = "UptbLog";
            // 
            // CctvIdLog
            // 
            CctvIdLog.HeaderText = "Cctv Id";
            CctvIdLog.Name = "CctvIdLog";
            // 
            // AccessPointLog
            // 
            AccessPointLog.HeaderText = "Access Point";
            AccessPointLog.Name = "AccessPointLog";
            // 
            // ModeLog
            // 
            ModeLog.HeaderText = "Mode";
            ModeLog.Name = "ModeLog";
            // 
            // LastConnectedLog
            // 
            LastConnectedLog.HeaderText = "Last Connected";
            LastConnectedLog.Name = "LastConnectedLog";
            // 
            // ActionLog
            // 
            ActionLog.HeaderText = "Action";
            ActionLog.Name = "ActionLog";
            ActionLog.Resizable = DataGridViewTriState.True;
            ActionLog.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // StatusLog
            // 
            dataGridViewCellStyle2.BackColor = Color.Red;
            StatusLog.DefaultCellStyle = dataGridViewCellStyle2;
            StatusLog.HeaderText = "Status";
            StatusLog.Name = "StatusLog";
            // 
            // ErrorLog
            // 
            ErrorLog.HeaderText = "Err Message";
            ErrorLog.Name = "ErrorLog";
            // 
            // LogLog
            // 
            LogLog.HeaderText = "Log";
            LogLog.Name = "LogLog";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label1.Location = new Point(307, 19);
            label1.Name = "label1";
            label1.Size = new Size(136, 25);
            label1.TabIndex = 3;
            label1.Text = "TARIKAN LOG";
            // 
            // btnStopAllLogJasnita
            // 
            btnStopAllLogJasnita.Location = new Point(105, 23);
            btnStopAllLogJasnita.Name = "btnStopAllLogJasnita";
            btnStopAllLogJasnita.Size = new Size(75, 23);
            btnStopAllLogJasnita.TabIndex = 1;
            btnStopAllLogJasnita.Text = "Stop All";
            btnStopAllLogJasnita.UseVisualStyleBackColor = true;
            // 
            // btnStartAllLogJasnita
            // 
            btnStartAllLogJasnita.Location = new Point(24, 23);
            btnStartAllLogJasnita.Name = "btnStartAllLogJasnita";
            btnStartAllLogJasnita.Size = new Size(75, 23);
            btnStartAllLogJasnita.TabIndex = 0;
            btnStartAllLogJasnita.Text = "Start All";
            btnStartAllLogJasnita.UseVisualStyleBackColor = true;
            // 
            // TarikanCctvTelkom
            // 
            TarikanCctvTelkom.Controls.Add(dataGridView3);
            TarikanCctvTelkom.Controls.Add(btnStopAllTelkom);
            TarikanCctvTelkom.Controls.Add(btnStartAllTelkom);
            TarikanCctvTelkom.Controls.Add(label3);
            TarikanCctvTelkom.Location = new Point(4, 24);
            TarikanCctvTelkom.Name = "TarikanCctvTelkom";
            TarikanCctvTelkom.Padding = new Padding(3);
            TarikanCctvTelkom.Size = new Size(752, 398);
            TarikanCctvTelkom.TabIndex = 2;
            TarikanCctvTelkom.Text = "TARIKAN CCTV TELKOM";
            TarikanCctvTelkom.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label3.Location = new Point(258, 25);
            label3.Name = "label3";
            label3.Size = new Size(225, 25);
            label3.TabIndex = 4;
            label3.Text = "TARIKAN CCTV TELKOM";
            // 
            // btnStartAllTelkom
            // 
            btnStartAllTelkom.Location = new Point(34, 46);
            btnStartAllTelkom.Name = "btnStartAllTelkom";
            btnStartAllTelkom.Size = new Size(75, 23);
            btnStartAllTelkom.TabIndex = 5;
            btnStartAllTelkom.Text = "Start All";
            btnStartAllTelkom.UseVisualStyleBackColor = true;
            // 
            // btnStopAllTelkom
            // 
            btnStopAllTelkom.Location = new Point(115, 46);
            btnStopAllTelkom.Name = "btnStopAllTelkom";
            btnStopAllTelkom.Size = new Size(75, 23);
            btnStopAllTelkom.TabIndex = 6;
            btnStopAllTelkom.Text = "Stop All";
            btnStopAllTelkom.UseVisualStyleBackColor = true;
            // 
            // dataGridView3
            // 
            dataGridView3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { NoTelkom, IdTelkom, NopTelkom, NamaTelkom, AlamatTelkom, UptbTelkom, CctvIdTelkom, ModeTelkom, LastConnectedTelkom, ActionTelkom, StatusTelkom, ErrorTelkom, LogTelkom });
            dataGridView3.Location = new Point(3, 75);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.Size = new Size(746, 327);
            dataGridView3.TabIndex = 7;
            // 
            // NoTelkom
            // 
            NoTelkom.HeaderText = "No";
            NoTelkom.Name = "NoTelkom";
            // 
            // IdTelkom
            // 
            IdTelkom.HeaderText = "Id";
            IdTelkom.Name = "IdTelkom";
            // 
            // NopTelkom
            // 
            NopTelkom.HeaderText = "Nop";
            NopTelkom.Name = "NopTelkom";
            // 
            // NamaTelkom
            // 
            NamaTelkom.HeaderText = "Nama";
            NamaTelkom.Name = "NamaTelkom";
            // 
            // AlamatTelkom
            // 
            AlamatTelkom.HeaderText = "Alamat";
            AlamatTelkom.Name = "AlamatTelkom";
            // 
            // UptbTelkom
            // 
            UptbTelkom.HeaderText = "Uptb";
            UptbTelkom.Name = "UptbTelkom";
            // 
            // CctvIdTelkom
            // 
            CctvIdTelkom.HeaderText = "Cctv Id";
            CctvIdTelkom.Name = "CctvIdTelkom";
            // 
            // ModeTelkom
            // 
            ModeTelkom.HeaderText = "Mode";
            ModeTelkom.Name = "ModeTelkom";
            // 
            // LastConnectedTelkom
            // 
            LastConnectedTelkom.HeaderText = "Last Connected";
            LastConnectedTelkom.Name = "LastConnectedTelkom";
            // 
            // ActionTelkom
            // 
            ActionTelkom.HeaderText = "Action";
            ActionTelkom.Name = "ActionTelkom";
            ActionTelkom.Resizable = DataGridViewTriState.True;
            ActionTelkom.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // StatusTelkom
            // 
            dataGridViewCellStyle3.BackColor = Color.Red;
            StatusTelkom.DefaultCellStyle = dataGridViewCellStyle3;
            StatusTelkom.HeaderText = "Status";
            StatusTelkom.Name = "StatusTelkom";
            // 
            // ErrorTelkom
            // 
            ErrorTelkom.HeaderText = "Err Message";
            ErrorTelkom.Name = "ErrorTelkom";
            // 
            // LogTelkom
            // 
            LogTelkom.HeaderText = "Log";
            LogTelkom.Name = "LogTelkom";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabControl1.ResumeLayout(false);
            TarikanCctvJasnita.ResumeLayout(false);
            TarikanCctvJasnita.PerformLayout();
            TarikanLogJasnita.ResumeLayout(false);
            TarikanLogJasnita.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            TarikanCctvTelkom.ResumeLayout(false);
            TarikanCctvTelkom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button btnStartAll;
        private Button btnStopAll;
        private DataGridViewTextBoxColumn No;
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
        private TabControl tabControl1;
        private TabPage TarikanCctvJasnita;
        private TabPage TarikanLogJasnita;
        private Button btnStopAllLogJasnita;
        private Button btnStartAllLogJasnita;
        private Label label1;
        private Label label2;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn NoLog;
        private DataGridViewTextBoxColumn IdLog;
        private DataGridViewTextBoxColumn NopLog;
        private DataGridViewTextBoxColumn NamaLog;
        private DataGridViewTextBoxColumn AlamatLog;
        private DataGridViewTextBoxColumn UptbLog;
        private DataGridViewTextBoxColumn CctvIdLog;
        private DataGridViewTextBoxColumn AccessPointLog;
        private DataGridViewTextBoxColumn ModeLog;
        private DataGridViewTextBoxColumn LastConnectedLog;
        private DataGridViewButtonColumn ActionLog;
        private DataGridViewTextBoxColumn StatusLog;
        private DataGridViewTextBoxColumn ErrorLog;
        private DataGridViewTextBoxColumn LogLog;
        private TabPage TarikanCctvTelkom;
        private DataGridView dataGridView3;
        private DataGridViewTextBoxColumn NoTelkom;
        private DataGridViewTextBoxColumn IdTelkom;
        private DataGridViewTextBoxColumn NopTelkom;
        private DataGridViewTextBoxColumn NamaTelkom;
        private DataGridViewTextBoxColumn AlamatTelkom;
        private DataGridViewTextBoxColumn UptbTelkom;
        private DataGridViewTextBoxColumn CctvIdTelkom;
        private DataGridViewTextBoxColumn ModeTelkom;
        private DataGridViewTextBoxColumn LastConnectedTelkom;
        private DataGridViewButtonColumn ActionTelkom;
        private DataGridViewTextBoxColumn StatusTelkom;
        private DataGridViewTextBoxColumn ErrorTelkom;
        private DataGridViewTextBoxColumn LogTelkom;
        private Button btnStopAllTelkom;
        private Button btnStartAllTelkom;
        private Label label3;
    }
}
