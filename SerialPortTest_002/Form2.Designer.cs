namespace SerialPortTest_002
{
    partial class Form2
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
            this.label5.Location = new System.Drawing.Point(7, 155);
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
            this.ComboBox_StopBit.Location = new System.Drawing.Point(89, 155);
            this.ComboBox_StopBit.Name = "ComboBox_StopBit";
            this.ComboBox_StopBit.Size = new System.Drawing.Size(93, 24);
            this.ComboBox_StopBit.TabIndex = 9;
            // 
            // BTN_Connect
            // 
            this.BTN_Connect.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BTN_Connect.Location = new System.Drawing.Point(357, 138);
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
            this.textBox_PortNum.Location = new System.Drawing.Point(325, 56);
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
            this.label_Port.Location = new System.Drawing.Point(204, 59);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(113, 23);
            this.label_Port.TabIndex = 18;
            this.label_Port.Text = "Port Number";
            // 
            // textBox_IPAddr
            // 
            this.textBox_IPAddr.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_IPAddr.Location = new System.Drawing.Point(291, 18);
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
            this.label_IPAddr.Location = new System.Drawing.Point(204, 21);
            this.label_IPAddr.Name = "label_IPAddr";
            this.label_IPAddr.Size = new System.Drawing.Size(81, 23);
            this.label_IPAddr.TabIndex = 16;
            this.label_IPAddr.Text = "Server IP";
            // 
            // textBox_MailAddress
            // 
            this.textBox_MailAddress.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MailAddress.Location = new System.Drawing.Point(256, 93);
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
            this.label6.Location = new System.Drawing.Point(204, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 23);
            this.label6.TabIndex = 22;
            this.label6.Text = "Mail";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 189);
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
            this.Name = "Form2";
            this.Text = "ComPort Setting";
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
    }
}