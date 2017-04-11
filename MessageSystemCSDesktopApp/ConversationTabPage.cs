using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessageSystemCSDesktopApp
{
    class ConversationTabPage : TabPage
    {
        private string uid;
        private string publicKey;

        //Controls
        private Button btn_send;
        private Panel panelSend;
        private RichTextBox tb_send_message;
        private RichTextBox tb_receive_message;
        private Panel panelReceive;

        public ConversationTabPage(string uid, string publicKey)
        {
            this.publicKey = publicKey;
            this.uid = uid;

            InitializeComponent();            
        }

        private void InitializeComponent()
        {
            this.btn_send = new Button();
            this.panelSend = new Panel();
            this.tb_receive_message = new RichTextBox();
            this.tb_send_message = new RichTextBox();
            this.panelReceive = new Panel();
            this.panelSend.SuspendLayout();
            this.panelReceive.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(257, 87);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 0;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // panelSend
            // 
            this.panelSend.Controls.Add(this.btn_send);
            this.panelSend.Controls.Add(this.tb_send_message);
            this.panelSend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSend.Location = new System.Drawing.Point(0, 311);
            this.panelSend.Name = "panelSend";
            this.panelSend.Size = new System.Drawing.Size(344, 122);
            this.panelSend.TabIndex = 1;
            // 
            // tb_receive_message
            // 
            this.tb_receive_message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_receive_message.Location = new System.Drawing.Point(0, 0);
            this.tb_receive_message.Name = "tb_receive_message";
            this.tb_receive_message.ReadOnly = true;
            this.tb_receive_message.Size = new System.Drawing.Size(344, 311);
            this.tb_receive_message.TabIndex = 1;
            this.tb_receive_message.Text = "";
            // 
            // tb_send_message
            // 
            this.tb_send_message.Location = new System.Drawing.Point(12, 6);
            this.tb_send_message.Name = "tb_send_message";
            this.tb_send_message.Size = new System.Drawing.Size(320, 75);
            this.tb_send_message.TabIndex = 2;
            this.tb_send_message.Text = "";
            this.tb_send_message.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_send_message_KeyDown);
            // 
            // panelReceive
            // 
            this.panelReceive.Controls.Add(this.tb_receive_message);
            this.panelReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReceive.Location = new System.Drawing.Point(0, 0);
            this.panelReceive.Name = "panelReceive";
            this.panelReceive.Size = new System.Drawing.Size(344, 311);
            this.panelReceive.TabIndex = 3;
            // 
            // test
            // 
            this.ClientSize = new System.Drawing.Size(344, 433);
            this.Controls.Add(this.panelReceive);
            this.Controls.Add(this.panelSend);
            this.Name = uid;
            this.Text = uid;
            this.panelSend.ResumeLayout(false);
            this.panelReceive.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void tb_send_message_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
