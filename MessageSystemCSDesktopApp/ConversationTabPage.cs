using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageSysDataManagementLib;

namespace MessageSystemCSDesktopApp
{
    public class ConversationTabPage : TabPage
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
        private WebBrowser wb_receive_message;
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

            setDefaultHTML();
            MessageBox.Show(wb_receive_message.DocumentText);
        }

        private void InitializeComponent()
        {
            this.btn_send = new Button();
            this.panelSend = new Panel();
            this.wb_receive_message = new WebBrowser();
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
            // wb_receive_message
            // 
            this.wb_receive_message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb_receive_message.Location = new System.Drawing.Point(0, 0);
            this.wb_receive_message.Size = new System.Drawing.Size(344, 311);
            this.wb_receive_message.TabIndex = 1;
            this.wb_receive_message.GotFocus += tb_receive_message_GotFocus;
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
            this.panelReceive.Controls.Add(this.wb_receive_message);
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
                main.Log(tb_send_message.Text);
                main.SendMessage(this.UID, MessageSysDataManagementLib.KeyManagement.Encrypt(this.PublicKey, tb_send_message.Text.TrimEnd(Environment.NewLine.ToCharArray())));
                NewMessageFromMe(DateTime.Now, tb_send_message.Text.TrimEnd(Environment.NewLine.ToCharArray()));                
            }                      
        }

        public void DisableAll(string message)
        {
            btn_send.Enabled = false;

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

            tb_send_message.Enabled = true;

            if (message != "")
            {
                ShowSystemMessage(message);
            }
        }

        private void ShowSystemMessage(string message)
        {
            //tb_receive_message.Text += message + "\n";
            wb_receive_message.Document.GetElementById("chat-history").InnerHtml += "<div class='system-message'>" + message + "</div>";
            wb_receive_message.Document.Window.ScrollTo(0, wb_receive_message.Document.Window.Size.Height);
        }

        public void NewMessageFromMe(DateTime messageTimeStamp, string message)
        {
            //tb_receive_message.Text += messageTimeStamp.ToString("HH:mm:ss") + " - Du: " + message + "\n"; 
            wb_receive_message.Document.GetElementById("chat-history").InnerHtml += "<div class='my-message'>" + Emojione.ReplaceAllShortnamesWithHTML(message) + "<div class='message-data text-right text-muted'><span class='glyphicon glyphicon-time'>" + messageTimeStamp.ToString() + "</span></div></div>";
            wb_receive_message.Document.Window.ScrollTo(0, wb_receive_message.Document.Window.Size.Height);
        }

        public void NewMessageFromOther(string otherUID, DateTime messageTimeStamp, string message)
        {
            //tb_receive_message.Text += messageTimeStamp.ToString("HH:mm:ss") + " - " + otherUID + ": " + message + "\n";
            wb_receive_message.Document.GetElementById("chat-history").InnerHtml += "<div class='other-message'>" + Emojione.ReplaceAllShortnamesWithHTML(message) + "<div class='message-data-other text-right text-muted'><span class='glyphicon glyphicon-time'>" + messageTimeStamp.ToString() + "</span></div></div>";
            wb_receive_message.Document.Window.ScrollTo(0, wb_receive_message.Document.Window.Size.Height);
        }

        private void setDefaultHTML()
        {
            string defaultHTML = "<html> " +
                                "<head>" +
                                "<meta http-equiv='X-UA-Compatible' content='IE=10' />" +
                                "    <style>" +
                                "        body {" +
                                "            font-family: 'Open Sans', sans-serif;" +
                                "            background-color: #47484b;" +
                                "        }" +
                                "        .my-message {" +
                                "            background: #a0a0a0;" +
                                "            border-color: #a0a0a0;" +
                                "            color: white; " +
                                "            border-radius: 4px; " +
                                "            width: 75%; " +
                                "            padding: 15px; " +
                                "            margin-bottom: 5px; " +
                                "            margin-top: 5px; " +
                                "            float: right;" +
                                "            font-size: 13px; " +
                                "        }" +
                                "        .chat-container {" +
                                "            overflow-y: scroll;" +
                                "            height: 600px;" +
                                "            border: 0.5px dimgrey solid;" +
                                "        }" +
                                "        .system-message { " +
                                "           width: 70%; " +
                                "           float: right; " +
                                "           background: rgba(168, 255, 0, 0.4); " +
                                "           border-color: #a0a0a0; " +
                                "           color: white; " +
                                "           border-radius: 4px; " +
                                "           padding-left: 7px; " +
                                "           margin-bottom: 5px; " +
                                "           margin-top: 5px; " +
                                "           font-size: 11px; " +
                                "           text-align: center; " +
                                "           padding-right: 7px; " +
                                "           margin-right: 15%; " +
                                "           padding-top: 5px; " +
                                "           padding-bottom: 5px; " +
                                "        }" +
                                "        .other-message {" +
                                "            background-color: #2a9fd6;" +
                                "            border-color: #2a9fd6;" +
                                "            color: white;        " +
                                "            border-radius: 4px;     " +
                                "            width: 75%;" +
                                "            padding: 15px;" +
                                "            margin-bottom: 5px;" +
                                "            margin-top: 5px;" +
                                "            float: left;       " +
                                "            font-size: 13px; " +
                                "        }" +
                                "        .message-data {" +
                                "            font-size: 11px;" +
                                "            text-align: right;" +
                                "        }" +
                                "        .message-data-other {" +
                                "            color: rgb(86, 86, 86);" +
                                "            font-size: 11px;" +
                                "            text-align: right;" +
                                "        }" +
                                "        .message-textbox {" +
                                "            border-radius: 0px;" +
                                "        }" +
                                "        .other-message > a {" +
                                "            color: #302b26;" +
                                "        }" +
                                "        .other-message > a:hover {" +
                                "            color: #090909;" +
                                "        }" +
                                "        .emojione {" +
                                "            width: 24px;" +
                                "            height: 24px;" +
                                "        }" +
                                "    </style>" +
                                "</head>" +
                                "<body>" +
                                "    <div id='chat-history'>" +
                                "    </div>    " +
                                "</body>" +
                                "</html>";

            wb_receive_message.Navigate("about:blank");
            if (wb_receive_message.Document != null)
            {
                wb_receive_message.Document.Write(string.Empty);
            }
            wb_receive_message.DocumentText = defaultHTML;
            wb_receive_message.Document.Write(defaultHTML);
        }
    }
}