namespace TCPServer
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lstClientIP = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSendAll = new System.Windows.Forms.Button();
            this.btnCorrect = new System.Windows.Forms.Button();
            this.btnError1 = new System.Windows.Forms.Button();
            this.btnError2 = new System.Windows.Forms.Button();
            this.btnEmpty = new System.Windows.Forms.Button();
            this.btnNotHamming = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(262, 495);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(74, 18);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(486, 23);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "127.0.0.1:9000";
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(74, 47);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(486, 442);
            this.txtInfo.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(219, 553);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lstClientIP
            // 
            this.lstClientIP.FormattingEnabled = true;
            this.lstClientIP.ItemHeight = 15;
            this.lstClientIP.Location = new System.Drawing.Point(566, 47);
            this.lstClientIP.Name = "lstClientIP";
            this.lstClientIP.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstClientIP.Size = new System.Drawing.Size(256, 529);
            this.lstClientIP.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(566, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Client IP:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btnSendAll
            // 
            this.btnSendAll.Location = new System.Drawing.Point(300, 553);
            this.btnSendAll.Name = "btnSendAll";
            this.btnSendAll.Size = new System.Drawing.Size(75, 23);
            this.btnSendAll.TabIndex = 10;
            this.btnSendAll.Text = "SendAll";
            this.btnSendAll.UseVisualStyleBackColor = true;
            this.btnSendAll.Click += new System.EventHandler(this.btnSendAll_Click);
            // 
            // btnCorrect
            // 
            this.btnCorrect.Location = new System.Drawing.Point(100, 524);
            this.btnCorrect.Name = "btnCorrect";
            this.btnCorrect.Size = new System.Drawing.Size(75, 23);
            this.btnCorrect.TabIndex = 11;
            this.btnCorrect.Text = "Correct";
            this.btnCorrect.UseVisualStyleBackColor = true;
            this.btnCorrect.Click += new System.EventHandler(this.btnCorrect_Click);
            // 
            // btnError1
            // 
            this.btnError1.Location = new System.Drawing.Point(181, 524);
            this.btnError1.Name = "btnError1";
            this.btnError1.Size = new System.Drawing.Size(75, 23);
            this.btnError1.TabIndex = 12;
            this.btnError1.Text = "Error 1 bit";
            this.btnError1.UseVisualStyleBackColor = true;
            this.btnError1.Click += new System.EventHandler(this.btnError1_Click);
            // 
            // btnError2
            // 
            this.btnError2.Location = new System.Drawing.Point(262, 524);
            this.btnError2.Name = "btnError2";
            this.btnError2.Size = new System.Drawing.Size(75, 23);
            this.btnError2.TabIndex = 13;
            this.btnError2.Text = "Error 2 bit";
            this.btnError2.UseVisualStyleBackColor = true;
            this.btnError2.Click += new System.EventHandler(this.btnError2_Click);
            // 
            // btnEmpty
            // 
            this.btnEmpty.Location = new System.Drawing.Point(343, 523);
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.Size = new System.Drawing.Size(75, 23);
            this.btnEmpty.TabIndex = 15;
            this.btnEmpty.Text = "Empty";
            this.btnEmpty.UseVisualStyleBackColor = true;
            this.btnEmpty.Click += new System.EventHandler(this.btnEmpty_Click);
            // 
            // btnNotHamming
            // 
            this.btnNotHamming.Location = new System.Drawing.Point(424, 524);
            this.btnNotHamming.Name = "btnNotHamming";
            this.btnNotHamming.Size = new System.Drawing.Size(94, 23);
            this.btnNotHamming.TabIndex = 16;
            this.btnNotHamming.Text = "NotHamming";
            this.btnNotHamming.UseVisualStyleBackColor = true;
            this.btnNotHamming.Click += new System.EventHandler(this.btnNotHamming_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 586);
            this.Controls.Add(this.btnNotHamming);
            this.Controls.Add(this.btnEmpty);
            this.Controls.Add(this.btnError2);
            this.Controls.Add(this.btnError1);
            this.Controls.Add(this.btnCorrect);
            this.Controls.Add(this.btnSendAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstClientIP);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCP/IP Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Button btnStart;
        private TextBox txtIP;
        private TextBox txtInfo;
        private Button btnSend;
        private ListBox lstClientIP;
        private Label label3;
        private Button btnSendAll;
        private Button btnCorrect;
        private Button btnError1;
        private Button btnError2;
        private Button btnEmpty;
        private Button btnNotHamming;
    }
}