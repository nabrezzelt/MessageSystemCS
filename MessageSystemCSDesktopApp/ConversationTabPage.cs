using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageSysDataManagementLib;
using System.Text.RegularExpressions;

namespace MessageSystemCSDesktopApp
{
    public partial class ConversationTabPage : TabPage  /* UserControl */
    {
        private frm_main main;
        private frm_emoji emoji = null;

        private string _uid;
        private string _publicKey;
        private bool _disabled = false;

        public string PublicKey { get => _publicKey; set => _publicKey = value; }
        public string UID { get => _uid; set => _uid = value; }
        public bool Disabled { get => _disabled; set => _disabled = value; }        

        public ConversationTabPage(frm_main main, string uid, string publicKey)
        {
            this.main = main;
            _publicKey = publicKey;
            _uid = uid;

            InitializeComponent();

            this.Name = uid;
            this.Text = uid;

            SetDefaultHTML();

            wb_receive_message.Navigating += wb_receive_message_Navigating;
        }

        private void conversationTapPage_GotFocus(object sender, EventArgs e)
        {
            FlashWindow.Stop(main);
        }

        private void tb_send_message_KeyUp(object sender, KeyEventArgs e)
        {
            FlashWindow.Stop(main);

            if (e.KeyCode == Keys.Enter && e.Shift)
            {
                main.Log("Shift + Enter pressed.");
            }
            else if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                main.Log("Only Enter pressed.");
                btn_send.PerformClick();                                
                main.Log("Message sent.");
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tb_send_message.Text) || !String.IsNullOrEmpty(tb_send_message.Text))
            {
                main.Log(tb_send_message.Text);
                main.SendMessage(this.UID, MessageSysDataManagementLib.KeyManagement.Encrypt(this.PublicKey, tb_send_message.Text.TrimEnd(Environment.NewLine.ToCharArray())));
                NewMessageFromMe(DateTime.Now, tb_send_message.Text.TrimEnd(Environment.NewLine.ToCharArray()));
                tb_send_message.Clear();
            }
        }

        public void DisableAll(string message)
        {
            btn_send.Enabled = false;

            tb_send_message.Enabled = false;

            if (message != "")
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
            Application.DoEvents();
            wb_receive_message.Document.Window.ScrollTo(0, wb_receive_message.Document.Body.ScrollRectangle.Height);            
        }

        public void NewMessageFromMe(DateTime messageTimeStamp, string message)
       {
            //tb_receive_message.Text += messageTimeStamp.ToString("HH:mm:ss") + " - Du: " + message + "\n"; 
            wb_receive_message.Document.GetElementById("chat-history").InnerHtml += "<div class='my-message'>" + SetFormatHTMLEmoticons(message) + "<div class='message-data text-right text-muted'><span class='glyphicon glyphicon-time'>" + messageTimeStamp.ToString("HH:mm:ss") + "</span></div></div>";
            Application.DoEvents();
            wb_receive_message.Document.Window.ScrollTo(0, wb_receive_message.Document.Body.ScrollRectangle.Height);
        }

        public void NewMessageFromOther(string otherUID, DateTime messageTimeStamp, string message)
        {
            //tb_receive_message.Text += messageTimeStamp.ToString("HH:mm:ss") + " - " + otherUID + ": " + message + "\n";
            wb_receive_message.Document.GetElementById("chat-history").InnerHtml += "<div class='other-message'>" + SetFormatHTMLEmoticons(message) + "<div class='message-data-other text-right text-muted'><span class='glyphicon glyphicon-time'>" + messageTimeStamp.ToString("HH:mm:ss") + "</span></div></div>";
            Application.DoEvents();
            wb_receive_message.Document.Window.ScrollTo(0, wb_receive_message.Document.Body.ScrollRectangle.Height);
        }

        private void SetDefaultHTML()
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

        public void AppendEmoijiShortname(string shortname)
        {
            tb_send_message.AppendText(shortname);
        }

        private void btn_emoji_Click(object sender, EventArgs e)
        {
            if(emoji != null)
            {
                emoji.Location = new Point(MousePosition.X, MousePosition.Y);
                emoji.Show();
            }
            else
            {
                emoji = new frm_emoji(this);               
                emoji.Location = new Point(MousePosition.X, MousePosition.Y);
                emoji.Show();
            }
        }

        private void wb_receive_message_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.ToString() != "about:blank")
            {
                System.Diagnostics.Process.Start(e.Url.ToString());
                e.Cancel = true;
            }
        }

        private string SetFormatHTMLEmoticons(string message)
        {
            //message = "http://www.iconsdb.com/icons/preview/white/message-xxl.png";            
            
            //Images
            //message = Regex.Replace(message, @"((?:(?:https?:\/\/))[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b(?:[-a-zA-Z0-9@:%_\+.~#?&\/=]*(\.jpg|\.png|\.jpeg|\.gif)))$", "<img class='img-link' src='$1'/>");

            //Format everything to Link:
            message = Regex.Replace(message, @"(['])?([(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b[-a-zA-Z0-9@:%_\+.~#?&//=]*)(['])?$", "<a href='$2'>$2<a/>");

            //Format Emoticons
            message = Emojione.ReplaceAllShortnamesWithHTML(message);

            return message;
        }
    }
}
