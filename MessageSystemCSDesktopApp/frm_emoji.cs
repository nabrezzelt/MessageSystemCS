using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MessageSysDataManagementLib;

namespace MessageSystemCSDesktopApp
{
    public partial class frm_emoji : Form
    {
        private ConversationTabPage tabpage;

        private Panel pnl_main;

        public frm_emoji(ConversationTabPage tabpage)
        {
            this.tabpage = tabpage;

            InitializeComponent();

            LoadEmojis();
        }

        private void InitializeComponent()
        {
            this.pnl_main = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnl_main
            // 
            this.pnl_main.AutoScroll = true;
            this.pnl_main.AutoScrollMinSize = new System.Drawing.Size(0, 262);
            this.pnl_main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(41)))), ((int)(((byte)(46)))));
            this.pnl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_main.Location = new System.Drawing.Point(0, 0);
            this.pnl_main.Name = "pnl_main";
            this.pnl_main.Size = new System.Drawing.Size(187, 190);
            this.pnl_main.TabIndex = 0;
            this.pnl_main.MouseEnter += new System.EventHandler(this.pnl_main_MouseEnter);
            // 
            // frm_emoji
            // 
            this.ClientSize = new System.Drawing.Size(187, 190);
            this.Controls.Add(this.pnl_main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_emoji";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frm_emoji";
            this.Deactivate += new System.EventHandler(this.frm_emoji_Deactivate);
            this.ResumeLayout(false);

        }

        private void LoadEmojis()
        {
            int counter = 0;
            int currentHeight = 12;
            int currentWidth = 12;

            foreach (KeyValuePair<string, string> entry in Emojione.map)
            {                
                PictureBox pb = new PictureBox();
                pb.Location = new Point(currentWidth, currentHeight);
                pb.Name = entry.Key;
                pb.Size = new Size(24, 24);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.LoadAsync(Emojione.UnicodeToUrl(entry.Value));
                pb.Click += Pb_Click;

                pnl_main.Controls.Add(pb);

                currentWidth += 30;
                counter++;

                if (counter == 5)
                {
                    counter = 0;
                    currentWidth = 12;
                    currentHeight += 30;
                }
                
            }
        }

        private void Pb_Click(object sender, System.EventArgs e)
        {
           tabpage.AppendEmoijiShortname(":" + ((PictureBox) sender).Name + ":");
        }

        private void pnl_main_MouseEnter(object sender, System.EventArgs e)
        {
            pnl_main.Focus();
        }

        private void frm_emoji_Deactivate(object sender, System.EventArgs e)
        {
            this.Hide();
        }
    }
}
