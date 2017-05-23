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
        private string _uid;
        private string _publicKey;
        private frm_main main;
        private bool _disabled = false;
        private int _newMessages;

        //Controls
        private Button btn_send;
        private Panel panelSend;
        private RichTextBox tb_send_message;
        private RichTextBox tb_receive_message;
        private Panel panelReceive;

        public string PublicKey { get => _publicKey; set => _publicKey = value; }
        public string UID { get => _uid; set => _uid = value; }
        public bool Disabled { get => _disabled; set => _disabled = value; }
        public int NewMessages {
            get
            {
                return _newMessages;
            }
            set {
                _newMessages = value;
                UpdateText();
            }
        }

        private void UpdateText()
        {
            if(NewMessages > 0)
            {
                this.Text = UID + " (" + NewMessages + ")";
            }
            else
            {
                this.Text = UID;
            }            
        }

        public ConversationTabPage(frm_main main, string uid, string publicKey)
        {
            this.main = main;
            _publicKey = publicKey;
            _uid = uid;
            _newMessages = 0;
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
            this.btn_send.GotFocus += btn_send_GotFocus;
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
            this.panelSend.GotFocus += panelSend_GotFocus;
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
            this.tb_receive_message.GotFocus += tb_receive_message_GotFocus;
            // 
            // tb_send_message
            // 
            this.tb_send_message.Location = new System.Drawing.Point(12, 6);
            this.tb_send_message.Name = "tb_send_message";
            this.tb_send_message.Size = new System.Drawing.Size(320, 75);
            this.tb_send_message.TabIndex = 2;
            this.tb_send_message.Text = "";
            this.tb_send_message.MaxLength = 550;
            this.tb_send_message.KeyUp += tb_send_message_KeyUp;
            this.tb_send_message.GotFocus += tb_send_message_GotFocus;
            // 
            // panelReceive
            // 
            this.panelReceive.Controls.Add(this.tb_receive_message);
            this.panelReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReceive.Location = new System.Drawing.Point(0, 0);
            this.panelReceive.Name = "panelReceive";
            this.panelReceive.Size = new System.Drawing.Size(344, 311);
            this.panelReceive.TabIndex = 3;
            this.panelReceive.GotFocus += panelReceive_GotFocus;
            // 
            // ConversationTabPage
            // 
            this.ClientSize = new System.Drawing.Size(344, 433);
            this.Controls.Add(this.panelReceive);
            this.Controls.Add(this.panelSend);
            this.Name = _uid;
            this.Text = _uid;
            this.panelSend.ResumeLayout(false);
            this.panelReceive.ResumeLayout(false);
            this.ResumeLayout(false);
            this.GotFocus += ConversationTabPage_GotFocus;

        }

        private void btn_send_GotFocus(object sender, EventArgs e)
        {
            FlashWindow.Stop(main);
        }

        private void panelSend_GotFocus(object sender, EventArgs e)
        {
            FlashWindow.Stop(main);
        }

        private void tb_receive_message_GotFocus(object sender, EventArgs e)
        {
            FlashWindow.Stop(main);
        }

        private void tb_send_message_GotFocus(object sender, EventArgs e)
        {
            FlashWindow.Stop(main);
        }

        private void panelReceive_GotFocus(object sender, EventArgs e)
        {
            FlashWindow.Stop(main);
        }

        private void ConversationTabPage_GotFocus(object sender, EventArgs e)
        {
            FlashWindow.Stop(main);
        }

        private void tb_send_message_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Shift)
            {
                main.Log("Shift + Enter pressed.");
            }
            else if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                main.Log("Only Enter pressed.");
                btn_send.PerformClick();
                tb_send_message.Clear();
                tb_send_message.Text = String.Empty;
                main.Log("Message sent.");
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(tb_send_message.Text) || !String.IsNullOrEmpty(tb_send_message.Text))
            {
                main.SendMessage(this.UID, MessageSysDataManagementLib.KeyManagement.Encrypt(this.PublicKey, tb_send_message.Text.TrimEnd(Environment.NewLine.ToCharArray())));
                NewMessageFromMe(DateTime.Now, tb_send_message.Text.TrimEnd(Environment.NewLine.ToCharArray()));
            }                      
        }

        public void DisableAll(string message)
        {
            btn_send.Enabled = false;
            tb_receive_message.Enabled = false;
            tb_send_message.Enabled = false;

            if(message != "")
            {
                ShowSystemMessage(message);
            }

            _disabled = true;
        }

        public void EnableAll(string message = "")
        {
            btn_send.Enabled = true;
            tb_receive_message.Enabled = true;
            tb_send_message.Enabled = true;

            if (message != "")
            {
                ShowSystemMessage(message);
            }
        }

        private void ShowSystemMessage(string message)
        {
            tb_receive_message.Text += message + "\n";
        }

        public void NewMessageFromMe(DateTime messageTimeStamp, string message)
        {
            tb_receive_message.Text += messageTimeStamp.ToString("HH:mm:ss") + " - Du: " + message + "\n";           
        }

        public void NewMessageFromOther(string otherUID, DateTime messageTimeStamp, string message)
        {
            tb_receive_message.Text += messageTimeStamp.ToString("HH:mm:ss") + " - " + otherUID + ": " + message + "\n";
        }
    }
}