﻿namespace MessageSystemCSDesktopApp
{
    partial class frm_main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_connect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_uid = new System.Windows.Forms.TextBox();
            this.lb_clients = new System.Windows.Forms.ListBox();
            this.tb_send_messages = new System.Windows.Forms.RichTextBox();
            this.tb_received_messages = new System.Windows.Forms.RichTextBox();
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_send = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(156, 17);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(386, 354);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Online Clients:";
            // 
            // tb_uid
            // 
            this.tb_uid.Location = new System.Drawing.Point(50, 6);
            this.tb_uid.Name = "tb_uid";
            this.tb_uid.Size = new System.Drawing.Size(100, 20);
            this.tb_uid.TabIndex = 4;
            this.tb_uid.Text = "Nabrezzelt";
            // 
            // lb_clients
            // 
            this.lb_clients.FormattingEnabled = true;
            this.lb_clients.Location = new System.Drawing.Point(386, 370);
            this.lb_clients.Name = "lb_clients";
            this.lb_clients.Size = new System.Drawing.Size(145, 186);
            this.lb_clients.TabIndex = 5;
            // 
            // tb_send_messages
            // 
            this.tb_send_messages.Location = new System.Drawing.Point(12, 460);
            this.tb_send_messages.Name = "tb_send_messages";
            this.tb_send_messages.Size = new System.Drawing.Size(368, 67);
            this.tb_send_messages.TabIndex = 6;
            this.tb_send_messages.Text = "";
            // 
            // tb_received_messages
            // 
            this.tb_received_messages.Location = new System.Drawing.Point(12, 58);
            this.tb_received_messages.Name = "tb_received_messages";
            this.tb_received_messages.ReadOnly = true;
            this.tb_received_messages.Size = new System.Drawing.Size(368, 396);
            this.tb_received_messages.TabIndex = 7;
            this.tb_received_messages.Text = "";
            // 
            // tb_ip
            // 
            this.tb_ip.Location = new System.Drawing.Point(50, 32);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(100, 20);
            this.tb_ip.TabIndex = 9;
            this.tb_ip.Text = "192.168.164.129";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "IP:";
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(305, 533);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 10;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            // 
            // frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 568);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.tb_ip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_received_messages);
            this.Controls.Add(this.tb_send_messages);
            this.Controls.Add(this.lb_clients);
            this.Controls.Add(this.tb_uid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_connect);
            this.Name = "frm_main";
            this.Text = "MessageSystemCS | DesktopApp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_main_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_uid;
        private System.Windows.Forms.ListBox lb_clients;
        private System.Windows.Forms.RichTextBox tb_send_messages;
        private System.Windows.Forms.RichTextBox tb_received_messages;
        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_send;
    }
}
