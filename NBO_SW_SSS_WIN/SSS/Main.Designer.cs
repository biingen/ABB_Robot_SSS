namespace SSS
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
            this.label1 = new System.Windows.Forms.Label();
            this.VerLabel = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.Txt_LoopCount = new System.Windows.Forms.TextBox();
            this.Label_LoopCounter = new System.Windows.Forms.Label();
            this.Picbox_CurrentStyatus = new System.Windows.Forms.PictureBox();
            this.PIC_ComPortStatus = new System.Windows.Forms.PictureBox();
            this.BTN_Stop = new System.Windows.Forms.Button();
            this.BTN_Pause = new System.Windows.Forms.Button();
            this.BTN_StartTest = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PIC_NetworkStatus = new System.Windows.Forms.PictureBox();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picbox_CurrentStyatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_ComPortStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_NetworkStatus)).BeginInit();
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
            this.toolStrip1.Size = new System.Drawing.Size(1625, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::SSS.ImageResource.open_file;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(99, 24);
            this.toolStripButton1.Text = "Open File";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::SSS.ImageResource.tools;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(83, 24);
            this.toolStripButton2.Text = "Setting";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = global::SSS.ImageResource.save_file;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(94, 24);
            this.toolStripButton3.Text = "Save File";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column10,
            this.Column2,
            this.Column4,
            this.Column3,
            this.Column5,
            this.Column6,
            this.Column9,
            this.Column7,
            this.Column8});
            this.dataGridView1.Location = new System.Drawing.Point(16, 35);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1596, 712);
            this.dataGridView1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(11, 772);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "ComPort Status";
            // 
            // VerLabel
            // 
            this.VerLabel.AutoSize = true;
            this.VerLabel.Font = new System.Drawing.Font("Microsoft JhengHei", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.VerLabel.Location = new System.Drawing.Point(1116, 856);
            this.VerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VerLabel.Name = "VerLabel";
            this.VerLabel.Size = new System.Drawing.Size(121, 35);
            this.VerLabel.TabIndex = 7;
            this.VerLabel.Text = "Version:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkBox1.Location = new System.Drawing.Point(381, 772);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(83, 29);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Loop";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Txt_LoopCount
            // 
            this.Txt_LoopCount.Enabled = false;
            this.Txt_LoopCount.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Txt_LoopCount.Location = new System.Drawing.Point(381, 806);
            this.Txt_LoopCount.Margin = new System.Windows.Forms.Padding(4);
            this.Txt_LoopCount.Name = "Txt_LoopCount";
            this.Txt_LoopCount.Size = new System.Drawing.Size(119, 34);
            this.Txt_LoopCount.TabIndex = 10;
            this.Txt_LoopCount.Text = "1";
            // 
            // Label_LoopCounter
            // 
            this.Label_LoopCounter.AutoSize = true;
            this.Label_LoopCounter.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Label_LoopCounter.Location = new System.Drawing.Point(200, 856);
            this.Label_LoopCounter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_LoopCounter.Name = "Label_LoopCounter";
            this.Label_LoopCounter.Size = new System.Drawing.Size(0, 25);
            this.Label_LoopCounter.TabIndex = 11;
            // 
            // Picbox_CurrentStyatus
            // 
            this.Picbox_CurrentStyatus.Location = new System.Drawing.Point(579, 772);
            this.Picbox_CurrentStyatus.Margin = new System.Windows.Forms.Padding(4);
            this.Picbox_CurrentStyatus.Name = "Picbox_CurrentStyatus";
            this.Picbox_CurrentStyatus.Size = new System.Drawing.Size(213, 75);
            this.Picbox_CurrentStyatus.TabIndex = 8;
            this.Picbox_CurrentStyatus.TabStop = false;
            // 
            // PIC_ComPortStatus
            // 
            this.PIC_ComPortStatus.Image = global::SSS.ImageResource.BlackLED;
            this.PIC_ComPortStatus.Location = new System.Drawing.Point(41, 802);
            this.PIC_ComPortStatus.Margin = new System.Windows.Forms.Padding(4);
            this.PIC_ComPortStatus.Name = "PIC_ComPortStatus";
            this.PIC_ComPortStatus.Size = new System.Drawing.Size(44, 44);
            this.PIC_ComPortStatus.TabIndex = 5;
            this.PIC_ComPortStatus.TabStop = false;
            // 
            // BTN_Stop
            // 
            this.BTN_Stop.Enabled = false;
            this.BTN_Stop.Image = global::SSS.ImageResource.stop;
            this.BTN_Stop.Location = new System.Drawing.Point(1115, 786);
            this.BTN_Stop.Margin = new System.Windows.Forms.Padding(4);
            this.BTN_Stop.Name = "BTN_Stop";
            this.BTN_Stop.Size = new System.Drawing.Size(127, 50);
            this.BTN_Stop.TabIndex = 4;
            this.BTN_Stop.UseVisualStyleBackColor = true;
            this.BTN_Stop.Click += new System.EventHandler(this.BTN_Stop_Click);
            // 
            // BTN_Pause
            // 
            this.BTN_Pause.Enabled = false;
            this.BTN_Pause.Image = global::SSS.ImageResource.pause_button;
            this.BTN_Pause.Location = new System.Drawing.Point(980, 786);
            this.BTN_Pause.Margin = new System.Windows.Forms.Padding(4);
            this.BTN_Pause.Name = "BTN_Pause";
            this.BTN_Pause.Size = new System.Drawing.Size(127, 50);
            this.BTN_Pause.TabIndex = 3;
            this.BTN_Pause.UseVisualStyleBackColor = true;
            this.BTN_Pause.Click += new System.EventHandler(this.BTN_Pause_Click);
            // 
            // BTN_StartTest
            // 
            this.BTN_StartTest.Image = global::SSS.ImageResource.play_button;
            this.BTN_StartTest.Location = new System.Drawing.Point(845, 786);
            this.BTN_StartTest.Margin = new System.Windows.Forms.Padding(4);
            this.BTN_StartTest.Name = "BTN_StartTest";
            this.BTN_StartTest.Size = new System.Drawing.Size(127, 50);
            this.BTN_StartTest.TabIndex = 2;
            this.BTN_StartTest.UseVisualStyleBackColor = true;
            this.BTN_StartTest.Click += new System.EventHandler(this.BTN_StartTest_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(192, 772);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 25);
            this.label2.TabIndex = 12;
            this.label2.Text = "Network Status";
            // 
            // PIC_NetworkStatus
            // 
            this.PIC_NetworkStatus.Image = global::SSS.ImageResource.BlackLED;
            this.PIC_NetworkStatus.Location = new System.Drawing.Point(217, 804);
            this.PIC_NetworkStatus.Margin = new System.Windows.Forms.Padding(4);
            this.PIC_NetworkStatus.Name = "PIC_NetworkStatus";
            this.PIC_NetworkStatus.Size = new System.Drawing.Size(44, 44);
            this.PIC_NetworkStatus.TabIndex = 13;
            this.PIC_NetworkStatus.TabStop = false;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "CMD";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "CMD Description";
            this.Column10.Name = "Column10";
            this.Column10.Width = 200;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Out String";
            this.Column2.Name = "Column2";
            this.Column2.Width = 200;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Delay(ms)";
            this.Column4.Name = "Column4";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "CRC Field";
            this.Column3.Name = "Column3";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Reply String";
            this.Column5.Name = "Column5";
            this.Column5.Width = 200;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Result_1";
            this.Column6.Name = "Column6";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Result_2";
            this.Column9.Name = "Column9";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Judge ";
            this.Column7.Name = "Column7";
            this.Column7.Width = 150;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Judge Criterion";
            this.Column8.Name = "Column8";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1625, 911);
            this.Controls.Add(this.PIC_NetworkStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Label_LoopCounter);
            this.Controls.Add(this.Txt_LoopCount);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.Picbox_CurrentStyatus);
            this.Controls.Add(this.VerLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PIC_ComPortStatus);
            this.Controls.Add(this.BTN_Stop);
            this.Controls.Add(this.BTN_Pause);
            this.Controls.Add(this.BTN_StartTest);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "SSS";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picbox_CurrentStyatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_ComPortStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_NetworkStatus)).EndInit();
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
        private System.Windows.Forms.PictureBox Picbox_CurrentStyatus;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox Txt_LoopCount;
        private System.Windows.Forms.Label Label_LoopCounter;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox PIC_NetworkStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}

