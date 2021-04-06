namespace Cheese
{
    partial class Main
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.VerLabel = new System.Windows.Forms.Label();
            this.chkBox_LoopTimes = new System.Windows.Forms.CheckBox();
            this.Txt_LoopTimes = new System.Windows.Forms.TextBox();
            this.Label_LoopCounter = new System.Windows.Forms.Label();
            this.Picbox_CurrentStatus = new System.Windows.Forms.PictureBox();
            this.PIC_ComPortStatus = new System.Windows.Forms.PictureBox();
            this.BTN_Stop = new System.Windows.Forms.Button();
            this.BTN_Pause = new System.Windows.Forms.Button();
            this.BTN_StartTest = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PIC_NetworkStatus = new System.Windows.Forms.PictureBox();
            this.cboxCameraList = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PIC_Arduino = new System.Windows.Forms.PictureBox();
            this.button_Schedule = new System.Windows.Forms.Button();
            this.button_Camera = new System.Windows.Forms.Button();
            this.button_AcOn = new System.Windows.Forms.Button();
            this.button_AcOff = new System.Windows.Forms.Button();
            this.Txt_LoopCounter = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picBox_preview = new System.Windows.Forms.PictureBox();
            this.videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer();
            this.button_Snapshot = new System.Windows.Forms.Button();
            this.videoSourcePlayer2 = new AForge.Controls.VideoSourcePlayer();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picbox_CurrentStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_ComPortStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_NetworkStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_Arduino)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_preview)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1194, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::Cheese.ImageResource.open_file;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(85, 24);
            this.toolStripButton1.Text = "Open File";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::Cheese.ImageResource.tools;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(71, 24);
            this.toolStripButton2.Text = "Setting";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = global::Cheese.ImageResource.save_file;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(80, 24);
            this.toolStripButton3.Text = "Save File";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13,
            this.Column14});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1172, 480);
            this.dataGridView1.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "CMD";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Times";
            this.Column2.Name = "Column2";
            this.Column2.Width = 50;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Interval";
            this.Column3.Name = "Column3";
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "PIN";
            this.Column4.Name = "Column4";
            this.Column4.Width = 50;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Function/CRC_mode";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Sub-function";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Output String";
            this.Column7.Name = "Column7";
            this.Column7.Width = 200;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "AC/USB_Switch";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Delay(ms)";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.HeaderText = "CMD Description";
            this.Column10.Name = "Column10";
            this.Column10.Width = 150;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Reply String";
            this.Column11.Name = "Column11";
            this.Column11.Width = 200;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Result_1";
            this.Column12.Name = "Column12";
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Result_2";
            this.Column13.Name = "Column13";
            // 
            // Column14
            // 
            this.Column14.HeaderText = "Judge";
            this.Column14.Name = "Column14";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(173, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "ComPort Status";
            // 
            // VerLabel
            // 
            this.VerLabel.AutoSize = true;
            this.VerLabel.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VerLabel.ForeColor = System.Drawing.Color.Coral;
            this.VerLabel.Location = new System.Drawing.Point(886, 640);
            this.VerLabel.Name = "VerLabel";
            this.VerLabel.Size = new System.Drawing.Size(96, 29);
            this.VerLabel.TabIndex = 7;
            this.VerLabel.Text = "Version:";
            // 
            // chkBox_LoopTimes
            // 
            this.chkBox_LoopTimes.AutoSize = true;
            this.chkBox_LoopTimes.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBox_LoopTimes.Location = new System.Drawing.Point(400, 574);
            this.chkBox_LoopTimes.Name = "chkBox_LoopTimes";
            this.chkBox_LoopTimes.Size = new System.Drawing.Size(118, 27);
            this.chkBox_LoopTimes.TabIndex = 9;
            this.chkBox_LoopTimes.Text = "Loop Times";
            this.chkBox_LoopTimes.UseVisualStyleBackColor = true;
            this.chkBox_LoopTimes.CheckedChanged += new System.EventHandler(this.chkBox_Loop_CheckedChanged);
            // 
            // Txt_LoopTimes
            // 
            this.Txt_LoopTimes.Enabled = false;
            this.Txt_LoopTimes.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_LoopTimes.Location = new System.Drawing.Point(558, 574);
            this.Txt_LoopTimes.Name = "Txt_LoopTimes";
            this.Txt_LoopTimes.Size = new System.Drawing.Size(79, 26);
            this.Txt_LoopTimes.TabIndex = 10;
            this.Txt_LoopTimes.Text = "1";
            this.Txt_LoopTimes.TextChanged += new System.EventHandler(this.Txt_LoopTimes_TextChanged);
            // 
            // Label_LoopCounter
            // 
            this.Label_LoopCounter.AutoSize = true;
            this.Label_LoopCounter.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_LoopCounter.Location = new System.Drawing.Point(416, 615);
            this.Label_LoopCounter.Name = "Label_LoopCounter";
            this.Label_LoopCounter.Size = new System.Drawing.Size(136, 23);
            this.Label_LoopCounter.TabIndex = 11;
            this.Label_LoopCounter.Text = "Loop Remaining";
            // 
            // Picbox_CurrentStatus
            // 
            this.Picbox_CurrentStatus.Location = new System.Drawing.Point(656, 574);
            this.Picbox_CurrentStatus.Name = "Picbox_CurrentStatus";
            this.Picbox_CurrentStatus.Size = new System.Drawing.Size(197, 49);
            this.Picbox_CurrentStatus.TabIndex = 8;
            this.Picbox_CurrentStatus.TabStop = false;
            // 
            // PIC_ComPortStatus
            // 
            this.PIC_ComPortStatus.Image = global::Cheese.ImageResource.BlackLED;
            this.PIC_ComPortStatus.Location = new System.Drawing.Point(153, 43);
            this.PIC_ComPortStatus.Name = "PIC_ComPortStatus";
            this.PIC_ComPortStatus.Size = new System.Drawing.Size(20, 20);
            this.PIC_ComPortStatus.TabIndex = 5;
            this.PIC_ComPortStatus.TabStop = false;
            // 
            // BTN_Stop
            // 
            this.BTN_Stop.Enabled = false;
            this.BTN_Stop.Image = global::Cheese.ImageResource.stop;
            this.BTN_Stop.Location = new System.Drawing.Point(1087, 583);
            this.BTN_Stop.Name = "BTN_Stop";
            this.BTN_Stop.Size = new System.Drawing.Size(95, 40);
            this.BTN_Stop.TabIndex = 4;
            this.BTN_Stop.UseVisualStyleBackColor = true;
            this.BTN_Stop.Click += new System.EventHandler(this.BTN_Stop_Click);
            // 
            // BTN_Pause
            // 
            this.BTN_Pause.Enabled = false;
            this.BTN_Pause.Image = global::Cheese.ImageResource.pause_button;
            this.BTN_Pause.Location = new System.Drawing.Point(986, 583);
            this.BTN_Pause.Name = "BTN_Pause";
            this.BTN_Pause.Size = new System.Drawing.Size(95, 40);
            this.BTN_Pause.TabIndex = 3;
            this.BTN_Pause.UseVisualStyleBackColor = true;
            this.BTN_Pause.Click += new System.EventHandler(this.BTN_Pause_Click);
            // 
            // BTN_StartTest
            // 
            this.BTN_StartTest.Image = global::Cheese.ImageResource.play_button;
            this.BTN_StartTest.Location = new System.Drawing.Point(885, 583);
            this.BTN_StartTest.Name = "BTN_StartTest";
            this.BTN_StartTest.Size = new System.Drawing.Size(95, 40);
            this.BTN_StartTest.TabIndex = 2;
            this.BTN_StartTest.UseVisualStyleBackColor = true;
            this.BTN_StartTest.Click += new System.EventHandler(this.BTN_StartTest_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(323, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "Network Status";
            // 
            // PIC_NetworkStatus
            // 
            this.PIC_NetworkStatus.Image = global::Cheese.ImageResource.BlackLED;
            this.PIC_NetworkStatus.Location = new System.Drawing.Point(304, 43);
            this.PIC_NetworkStatus.Name = "PIC_NetworkStatus";
            this.PIC_NetworkStatus.Size = new System.Drawing.Size(20, 20);
            this.PIC_NetworkStatus.TabIndex = 13;
            this.PIC_NetworkStatus.TabStop = false;
            // 
            // cboxCameraList
            // 
            this.cboxCameraList.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxCameraList.FormattingEnabled = true;
            this.cboxCameraList.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cboxCameraList.Location = new System.Drawing.Point(947, 37);
            this.cboxCameraList.Name = "cboxCameraList";
            this.cboxCameraList.Size = new System.Drawing.Size(228, 27);
            this.cboxCameraList.TabIndex = 28;
            this.cboxCameraList.SelectedIndexChanged += new System.EventHandler(this.cboxCameraList_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(808, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 23);
            this.label7.TabIndex = 27;
            this.label7.Text = "Camera Device:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 19);
            this.label3.TabIndex = 30;
            this.label3.Text = "Arduino Status";
            // 
            // PIC_Arduino
            // 
            this.PIC_Arduino.Image = global::Cheese.ImageResource.BlackLED;
            this.PIC_Arduino.Location = new System.Drawing.Point(11, 44);
            this.PIC_Arduino.Name = "PIC_Arduino";
            this.PIC_Arduino.Size = new System.Drawing.Size(20, 20);
            this.PIC_Arduino.TabIndex = 29;
            this.PIC_Arduino.TabStop = false;
            // 
            // button_Schedule
            // 
            this.button_Schedule.Enabled = false;
            this.button_Schedule.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Schedule.Location = new System.Drawing.Point(12, 623);
            this.button_Schedule.Name = "button_Schedule";
            this.button_Schedule.Size = new System.Drawing.Size(109, 43);
            this.button_Schedule.TabIndex = 33;
            this.button_Schedule.Text = "Schedule";
            this.button_Schedule.UseVisualStyleBackColor = true;
            this.button_Schedule.Click += new System.EventHandler(this.button_Schedule_Click);
            // 
            // button_Camera
            // 
            this.button_Camera.Enabled = false;
            this.button_Camera.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Camera.Location = new System.Drawing.Point(12, 574);
            this.button_Camera.Name = "button_Camera";
            this.button_Camera.Size = new System.Drawing.Size(109, 43);
            this.button_Camera.TabIndex = 34;
            this.button_Camera.Text = "Camera";
            this.button_Camera.UseVisualStyleBackColor = true;
            this.button_Camera.Click += new System.EventHandler(this.button_Camera_Click);
            // 
            // button_AcOn
            // 
            this.button_AcOn.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_AcOn.Location = new System.Drawing.Point(242, 574);
            this.button_AcOn.Name = "button_AcOn";
            this.button_AcOn.Size = new System.Drawing.Size(109, 43);
            this.button_AcOn.TabIndex = 35;
            this.button_AcOn.Text = "AC On";
            this.button_AcOn.UseVisualStyleBackColor = true;
            this.button_AcOn.Click += new System.EventHandler(this.button_AcOn_Click);
            // 
            // button_AcOff
            // 
            this.button_AcOff.Enabled = false;
            this.button_AcOff.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_AcOff.Location = new System.Drawing.Point(242, 623);
            this.button_AcOff.Name = "button_AcOff";
            this.button_AcOff.Size = new System.Drawing.Size(109, 43);
            this.button_AcOff.TabIndex = 36;
            this.button_AcOff.Text = "AC off";
            this.button_AcOff.UseVisualStyleBackColor = true;
            this.button_AcOff.Click += new System.EventHandler(this.button_AcOff_Click);
            // 
            // Txt_LoopCounter
            // 
            this.Txt_LoopCounter.Enabled = false;
            this.Txt_LoopCounter.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_LoopCounter.Location = new System.Drawing.Point(558, 615);
            this.Txt_LoopCounter.Name = "Txt_LoopCounter";
            this.Txt_LoopCounter.Size = new System.Drawing.Size(79, 26);
            this.Txt_LoopCounter.TabIndex = 32;
            this.Txt_LoopCounter.Text = "1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.videoSourcePlayer2);
            this.panel1.Controls.Add(this.picBox_preview);
            this.panel1.Controls.Add(this.videoSourcePlayer1);
            this.panel1.Location = new System.Drawing.Point(12, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1180, 480);
            this.panel1.TabIndex = 37;
            // 
            // picBox_preview
            // 
            this.picBox_preview.Location = new System.Drawing.Point(603, 3);
            this.picBox_preview.Name = "picBox_preview";
            this.picBox_preview.Size = new System.Drawing.Size(272, 153);
            this.picBox_preview.TabIndex = 1;
            this.picBox_preview.TabStop = false;
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.Location = new System.Drawing.Point(3, 3);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(560, 315);
            this.videoSourcePlayer1.TabIndex = 0;
            this.videoSourcePlayer1.Text = "videoSourcePlayer1";
            this.videoSourcePlayer1.VideoSource = null;
            // 
            // button_Snapshot
            // 
            this.button_Snapshot.Enabled = false;
            this.button_Snapshot.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Snapshot.Location = new System.Drawing.Point(127, 574);
            this.button_Snapshot.Name = "button_Snapshot";
            this.button_Snapshot.Size = new System.Drawing.Size(109, 43);
            this.button_Snapshot.TabIndex = 38;
            this.button_Snapshot.Text = "Snapshot";
            this.button_Snapshot.UseVisualStyleBackColor = true;
            this.button_Snapshot.Click += new System.EventHandler(this.button_Snapshot_Click);
            // 
            // videoSourcePlayer2
            // 
            this.videoSourcePlayer2.Location = new System.Drawing.Point(603, 162);
            this.videoSourcePlayer2.Name = "videoSourcePlayer2";
            this.videoSourcePlayer2.Size = new System.Drawing.Size(560, 315);
            this.videoSourcePlayer2.TabIndex = 2;
            this.videoSourcePlayer2.Text = "videoSourcePlayer2";
            this.videoSourcePlayer2.VideoSource = null;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 692);
            this.Controls.Add(this.button_Snapshot);
            this.Controls.Add(this.button_AcOff);
            this.Controls.Add(this.button_AcOn);
            this.Controls.Add(this.button_Camera);
            this.Controls.Add(this.button_Schedule);
            this.Controls.Add(this.Txt_LoopCounter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PIC_Arduino);
            this.Controls.Add(this.cboxCameraList);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.PIC_NetworkStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Label_LoopCounter);
            this.Controls.Add(this.Txt_LoopTimes);
            this.Controls.Add(this.chkBox_LoopTimes);
            this.Controls.Add(this.Picbox_CurrentStatus);
            this.Controls.Add(this.VerLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PIC_ComPortStatus);
            this.Controls.Add(this.BTN_Stop);
            this.Controls.Add(this.BTN_Pause);
            this.Controls.Add(this.BTN_StartTest);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Cheese";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picbox_CurrentStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_ComPortStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_NetworkStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_Arduino)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BTN_StartTest;
        private System.Windows.Forms.Button BTN_Pause;
        private System.Windows.Forms.Button BTN_Stop;
        private System.Windows.Forms.PictureBox PIC_ComPortStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label VerLabel;
        private System.Windows.Forms.PictureBox Picbox_CurrentStatus;
        private System.Windows.Forms.CheckBox chkBox_LoopTimes;
        private System.Windows.Forms.TextBox Txt_LoopTimes;
        private System.Windows.Forms.Label Label_LoopCounter;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox PIC_NetworkStatus;
        private System.Windows.Forms.ComboBox cboxCameraList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox PIC_Arduino;
        private System.Windows.Forms.Button button_Schedule;
        private System.Windows.Forms.Button button_Camera;
        private System.Windows.Forms.Button button_AcOn;
        private System.Windows.Forms.Button button_AcOff;
        private System.Windows.Forms.TextBox Txt_LoopCounter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picBox_preview;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1;
        private System.Windows.Forms.Button button_Snapshot;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer2;
    }
}

