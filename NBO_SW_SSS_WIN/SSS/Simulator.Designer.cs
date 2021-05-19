namespace Cheese
{
    partial class Simulator
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
            this.textBox_receive = new System.Windows.Forms.TextBox();
            this.label_receive = new System.Windows.Forms.Label();
            this.label_send = new System.Windows.Forms.Label();
            this.textBox_send = new System.Windows.Forms.TextBox();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_Recv = new System.Windows.Forms.Button();
            this.button_Send = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_receive
            // 
            this.textBox_receive.Location = new System.Drawing.Point(94, 56);
            this.textBox_receive.Multiline = true;
            this.textBox_receive.Name = "textBox_receive";
            this.textBox_receive.Size = new System.Drawing.Size(645, 131);
            this.textBox_receive.TabIndex = 0;
            // 
            // label_receive
            // 
            this.label_receive.AutoSize = true;
            this.label_receive.BackColor = System.Drawing.Color.Black;
            this.label_receive.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_receive.ForeColor = System.Drawing.Color.White;
            this.label_receive.Location = new System.Drawing.Point(90, 34);
            this.label_receive.Name = "label_receive";
            this.label_receive.Size = new System.Drawing.Size(85, 19);
            this.label_receive.TabIndex = 1;
            this.label_receive.Text = "Receiving";
            // 
            // label_send
            // 
            this.label_send.AutoSize = true;
            this.label_send.BackColor = System.Drawing.Color.Black;
            this.label_send.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_send.ForeColor = System.Drawing.Color.White;
            this.label_send.Location = new System.Drawing.Point(90, 234);
            this.label_send.Name = "label_send";
            this.label_send.Size = new System.Drawing.Size(73, 19);
            this.label_send.TabIndex = 3;
            this.label_send.Text = "Sending";
            // 
            // textBox_send
            // 
            this.textBox_send.Location = new System.Drawing.Point(94, 256);
            this.textBox_send.Multiline = true;
            this.textBox_send.Name = "textBox_send";
            this.textBox_send.Size = new System.Drawing.Size(645, 131);
            this.textBox_send.TabIndex = 2;
            // 
            // button_clear
            // 
            this.button_clear.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_clear.Location = new System.Drawing.Point(674, 413);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(108, 38);
            this.button_clear.TabIndex = 4;
            this.button_clear.Text = "Clear Text";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_Recv
            // 
            this.button_Recv.BackColor = System.Drawing.Color.Blue;
            this.button_Recv.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Recv.ForeColor = System.Drawing.Color.White;
            this.button_Recv.Location = new System.Drawing.Point(631, 12);
            this.button_Recv.Name = "button_Recv";
            this.button_Recv.Size = new System.Drawing.Size(108, 38);
            this.button_Recv.TabIndex = 5;
            this.button_Recv.Text = "RECV";
            this.button_Recv.UseVisualStyleBackColor = false;
            this.button_Recv.Click += new System.EventHandler(this.button_Recv_Click);
            // 
            // button_Send
            // 
            this.button_Send.BackColor = System.Drawing.Color.Green;
            this.button_Send.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Send.ForeColor = System.Drawing.Color.White;
            this.button_Send.Location = new System.Drawing.Point(631, 212);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(108, 38);
            this.button_Send.TabIndex = 6;
            this.button_Send.Text = "SEND";
            this.button_Send.UseVisualStyleBackColor = false;
            // 
            // Simulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 457);
            this.Controls.Add(this.button_Send);
            this.Controls.Add(this.button_Recv);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.label_send);
            this.Controls.Add(this.textBox_send);
            this.Controls.Add(this.label_receive);
            this.Controls.Add(this.textBox_receive);
            this.Name = "Simulator";
            this.Text = "SIM";
            this.Load += new System.EventHandler(this.Simulator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_receive;
        private System.Windows.Forms.Label label_receive;
        private System.Windows.Forms.Label label_send;
        private System.Windows.Forms.TextBox textBox_send;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_Recv;
        private System.Windows.Forms.Button button_Send;
    }
}