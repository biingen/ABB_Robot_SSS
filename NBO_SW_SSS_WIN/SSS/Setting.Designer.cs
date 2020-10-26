namespace SSS
{
    partial class Setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ComboBox_BaudRate = new System.Windows.Forms.ComboBox();
            this.ComboBox_ParrityBit = new System.Windows.Forms.ComboBox();
            this.ComboBox_ByteCount = new System.Windows.Forms.ComboBox();
            this.ComboBox_StopBit = new System.Windows.Forms.ComboBox();
            this.BTN_Connect = new System.Windows.Forms.Button();
            this.textBox_PortNum = new System.Windows.Forms.TextBox();
            this.label_Port = new System.Windows.Forms.Label();
            this.textBox_IPAddr = new System.Windows.Forms.TextBox();
            this.label_IPAddr = new System.Windows.Forms.Label();
            this.textBox_MailAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboCameraTypeList = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboResolutionList = new System.Windows.Forms.ComboBox();
            this.textBox_Timeout = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(7, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port Num:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(7, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud Rate:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(7, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Parrity Bit:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(7, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Byte Count:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(7, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Stop Bit:";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(89, 18);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(93, 24);
            this.comboBox1.TabIndex = 5;
            // 
            // ComboBox_BaudRate
            // 
            this.ComboBox_BaudRate.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ComboBox_BaudRate.FormattingEnabled = true;
            this.ComboBox_BaudRate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "76800",
            "115200"});
            this.ComboBox_BaudRate.Location = new System.Drawing.Point(89, 56);
            this.ComboBox_BaudRate.Name = "ComboBox_BaudRate";
            this.ComboBox_BaudRate.Size = new System.Drawing.Size(93, 24);
            this.ComboBox_BaudRate.TabIndex = 6;
            // 
            // ComboBox_ParrityBit
            // 
            this.ComboBox_ParrityBit.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ComboBox_ParrityBit.FormattingEnabled = true;
            this.ComboBox_ParrityBit.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.ComboBox_ParrityBit.Location = new System.Drawing.Point(89, 89);
            this.ComboBox_ParrityBit.Name = "ComboBox_ParrityBit";
            this.ComboBox_ParrityBit.Size = new System.Drawing.Size(93, 24);
            this.ComboBox_ParrityBit.TabIndex = 7;
            // 
            // ComboBox_ByteCount
            // 
            this.ComboBox_ByteCount.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ComboBox_ByteCount.FormattingEnabled = true;
            this.ComboBox_ByteCount.Items.AddRange(new object[] {
            "7",
            "8"});
            this.ComboBox_ByteCount.Location = new System.Drawing.Point(89, 119);
            this.ComboBox_ByteCount.Name = "ComboBox_ByteCount";
            this.ComboBox_ByteCount.Size = new System.Drawing.Size(93, 24);
            this.ComboBox_ByteCount.TabIndex = 8;
            // 
            // ComboBox_StopBit
            // 
            this.ComboBox_StopBit.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ComboBox_StopBit.FormattingEnabled = true;
            this.ComboBox_StopBit.Items.AddRange(new object[] {
            "1",
            "2"});
            this.ComboBox_StopBit.Location = new System.Drawing.Point(89, 152);
            this.ComboBox_StopBit.Name = "ComboBox_StopBit";
            this.ComboBox_StopBit.Size = new System.Drawing.Size(93, 24);
            this.ComboBox_StopBit.TabIndex = 9;
            // 
            // BTN_Connect
            // 
            this.BTN_Connect.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BTN_Connect.Location = new System.Drawing.Point(355, 189);
            this.BTN_Connect.Name = "BTN_Connect";
            this.BTN_Connect.Size = new System.Drawing.Size(107, 41);
            this.BTN_Connect.TabIndex = 10;
            this.BTN_Connect.Text = "Connect";
            this.BTN_Connect.UseVisualStyleBackColor = true;
            this.BTN_Connect.Click += new System.EventHandler(this.BTN_Connect_Click);
            // 
            // textBox_PortNum
            // 
            this.textBox_PortNum.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_PortNum.Location = new System.Drawing.Point(325, 48);
            this.textBox_PortNum.MaxLength = 5;
            this.textBox_PortNum.Name = "textBox_PortNum";
            this.textBox_PortNum.Size = new System.Drawing.Size(139, 31);
            this.textBox_PortNum.TabIndex = 19;
            this.textBox_PortNum.Text = "1025";
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.BackColor = System.Drawing.Color.Black;
            this.label_Port.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Port.ForeColor = System.Drawing.Color.White;
            this.label_Port.Location = new System.Drawing.Point(204, 51);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(113, 23);
            this.label_Port.TabIndex = 18;
            this.label_Port.Text = "Port Number";
            // 
            // textBox_IPAddr
            // 
            this.textBox_IPAddr.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_IPAddr.Location = new System.Drawing.Point(291, 13);
            this.textBox_IPAddr.MaxLength = 15;
            this.textBox_IPAddr.Name = "textBox_IPAddr";
            this.textBox_IPAddr.Size = new System.Drawing.Size(173, 31);
            this.textBox_IPAddr.TabIndex = 17;
            this.textBox_IPAddr.Text = "127.0.0.1";
            // 
            // label_IPAddr
            // 
            this.label_IPAddr.AutoSize = true;
            this.label_IPAddr.BackColor = System.Drawing.Color.Black;
            this.label_IPAddr.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_IPAddr.ForeColor = System.Drawing.Color.White;
            this.label_IPAddr.Location = new System.Drawing.Point(204, 16);
            this.label_IPAddr.Name = "label_IPAddr";
            this.label_IPAddr.Size = new System.Drawing.Size(81, 23);
            this.label_IPAddr.TabIndex = 16;
            this.label_IPAddr.Text = "Server IP";
            // 
            // textBox_MailAddress
            // 
            this.textBox_MailAddress.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MailAddress.Location = new System.Drawing.Point(256, 118);
            this.textBox_MailAddress.Name = "textBox_MailAddress";
            this.textBox_MailAddress.Size = new System.Drawing.Size(208, 31);
            this.textBox_MailAddress.TabIndex = 21;
            this.textBox_MailAddress.Text = "tpdqatest@gmail.com";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(204, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 23);
            this.label6.TabIndex = 22;
            this.label6.Text = "Mail";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Black;
            this.label7.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(10, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 23);
            this.label7.TabIndex = 23;
            this.label7.Text = "Device";
            this.label7.Visible = false;
            // 
            // cboCameraTypeList
            // 
            this.cboCameraTypeList.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboCameraTypeList.FormattingEnabled = true;
            this.cboCameraTypeList.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cboCameraTypeList.Location = new System.Drawing.Point(80, 182);
            this.cboCameraTypeList.Name = "cboCameraTypeList";
            this.cboCameraTypeList.Size = new System.Drawing.Size(190, 24);
            this.cboCameraTypeList.TabIndex = 24;
            this.cboCameraTypeList.Visible = false;
            this.cboCameraTypeList.SelectedIndexChanged += new System.EventHandler(this.cboCameraTypeList_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Black;
            this.label8.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(10, 213);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 23);
            this.label8.TabIndex = 25;
            this.label8.Text = "Resolution";
            this.label8.Visible = false;
            // 
            // cboResolutionList
            // 
            this.cboResolutionList.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cboResolutionList.FormattingEnabled = true;
            this.cboResolutionList.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cboResolutionList.Location = new System.Drawing.Point(111, 213);
            this.cboResolutionList.Name = "cboResolutionList";
            this.cboResolutionList.Size = new System.Drawing.Size(159, 24);
            this.cboResolutionList.TabIndex = 26;
            this.cboResolutionList.Visible = false;
            this.cboResolutionList.SelectedIndexChanged += new System.EventHandler(this.cboResolutionList_SelectedIndexChanged);
            // 
            // textBox_Timeout
            // 
            this.textBox_Timeout.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Timeout.Location = new System.Drawing.Point(334, 83);
            this.textBox_Timeout.MaxLength = 5;
            this.textBox_Timeout.Name = "textBox_Timeout";
            this.textBox_Timeout.Size = new System.Drawing.Size(130, 31);
            this.textBox_Timeout.TabIndex = 28;
            this.textBox_Timeout.Text = "5000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Black;
            this.label9.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(204, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 23);
            this.label9.TabIndex = 27;
            this.label9.Text = "Time-Out (ms)";
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 246);
            this.Controls.Add(this.textBox_Timeout);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cboResolutionList);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cboCameraTypeList);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_MailAddress);
            this.Controls.Add(this.textBox_PortNum);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.textBox_IPAddr);
            this.Controls.Add(this.label_IPAddr);
            this.Controls.Add(this.BTN_Connect);
            this.Controls.Add(this.ComboBox_StopBit);
            this.Controls.Add(this.ComboBox_ByteCount);
            this.Controls.Add(this.ComboBox_ParrityBit);
            this.Controls.Add(this.ComboBox_BaudRate);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setting";
            this.Text = "Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox ComboBox_BaudRate;
        private System.Windows.Forms.ComboBox ComboBox_ParrityBit;
        private System.Windows.Forms.ComboBox ComboBox_ByteCount;
        private System.Windows.Forms.ComboBox ComboBox_StopBit;
        private System.Windows.Forms.Button BTN_Connect;
        private System.Windows.Forms.TextBox textBox_PortNum;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.TextBox textBox_IPAddr;
        private System.Windows.Forms.Label label_IPAddr;
        internal System.Windows.Forms.TextBox textBox_MailAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboCameraTypeList;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboResolutionList;
        private System.Windows.Forms.TextBox textBox_Timeout;
        private System.Windows.Forms.Label label9;
    }
}