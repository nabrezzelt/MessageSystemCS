using System;
using System.Windows.Forms;

namespace MessageSystemCSDesktopApp
{
    partial class ConversationTabPage
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        //Controls
        private Button btn_send;
        private Panel panelSend;
        private RichTextBox tb_send_message;
        private WebBrowser wb_receive_message;
        private Panel panelReceive;

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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_send = new System.Windows.Forms.Button();
            this.panelSend = new System.Windows.Forms.Panel();
            this.btn_emoji = new System.Windows.Forms.Button();
            this.tb_send_message = new System.Windows.Forms.RichTextBox();
            this.wb_receive_message = new System.Windows.Forms.WebBrowser();
            this.panelReceive = new System.Windows.Forms.Panel();
            this.panelSend.SuspendLayout();
            this.panelReceive.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(281, 87);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 0;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += btn_send_Click;
            this.btn_send.GotFocus += conversationTapPage_GotFocus;
            // 
            // panelSend
            // 
            this.panelSend.Controls.Add(this.btn_emoji);
            this.panelSend.Controls.Add(this.btn_send);
            this.panelSend.Controls.Add(this.tb_send_message);
            this.panelSend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSend.Location = new System.Drawing.Point(0, 421);
            this.panelSend.Name = "panelSend";
            this.panelSend.Size = new System.Drawing.Size(397, 122);
            this.panelSend.TabIndex = 1;
            this.panelSend.GotFocus += conversationTapPage_GotFocus;
            // 
            // btn_emoji
            // 
            this.btn_emoji.BackColor = System.Drawing.Color.Transparent;
            this.btn_emoji.BackgroundImage = global::MessageSystemCSDesktopApp.Properties.Resources.emoticon;
            this.btn_emoji.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_emoji.Location = new System.Drawing.Point(357, 6);
            this.btn_emoji.Name = "btn_emoji";
            this.btn_emoji.Size = new System.Drawing.Size(32, 32);
            this.btn_emoji.TabIndex = 4;
            this.btn_emoji.UseVisualStyleBackColor = false;
            this.btn_emoji.Click += new System.EventHandler(this.btn_emoji_Click);
            this.btn_emoji.GotFocus += conversationTapPage_GotFocus;
            // 
            // tb_send_message
            // 
            this.tb_send_message.Location = new System.Drawing.Point(12, 6);
            this.tb_send_message.MaxLength = 550;
            this.tb_send_message.Name = "tb_send_message";
            this.tb_send_message.Size = new System.Drawing.Size(339, 75);
            this.tb_send_message.TabIndex = 2;
            this.tb_send_message.Text = "";
            this.tb_send_message.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tb_send_message_KeyUp);
            this.tb_send_message.GotFocus += conversationTapPage_GotFocus;
            // 
            // wb_receive_message
            // 
            this.wb_receive_message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb_receive_message.Location = new System.Drawing.Point(0, 0);
            this.wb_receive_message.Name = "wb_receive_message";
            this.wb_receive_message.Size = new System.Drawing.Size(397, 421);
            this.wb_receive_message.TabIndex = 1;
            this.wb_receive_message.GotFocus += conversationTapPage_GotFocus;
            // 
            // panelReceive
            // 
            this.panelReceive.Controls.Add(this.wb_receive_message);
            this.panelReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReceive.Location = new System.Drawing.Point(0, 0);
            this.panelReceive.Name = "panelReceive";
            this.panelReceive.Size = new System.Drawing.Size(397, 421);
            this.panelReceive.TabIndex = 3;
            this.panelReceive.GotFocus += conversationTapPage_GotFocus;
            // 
            // ConversationTabPage
            // 
            this.Controls.Add(this.panelReceive);
            this.Controls.Add(this.panelSend);
            this.Name = "ConversationTabPage";
            this.Size = new System.Drawing.Size(397, 543);
            this.panelSend.ResumeLayout(false);
            this.panelReceive.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion

        private Button btn_emoji;
    }
}
