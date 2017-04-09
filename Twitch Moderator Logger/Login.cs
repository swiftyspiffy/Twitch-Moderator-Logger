using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Twitch_Moderator_Logger
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length < 1)
            {
                MessageBox.Show("No channel supplied.");
                return;
            }
            if(textBox2.Text.Length < 1)
            {
                MessageBox.Show("No moderator username supplied.");
                return;
            }
            if(textBox3.Text.Length < 1)
            {
                MessageBox.Show("No moderator oauth supplied.");
                return;
            }

            Common.Config.Channel = textBox1.Text;
            Common.Config.ModeratorUsername = textBox2.Text;
            if (textBox3.Text.Contains(" "))
                Common.Config.ModeratorOAuth = textBox3.Text.Split(' ')[1];
            else
                Common.Config.ModeratorOAuth = textBox3.Text;

            Common.PubSub.Connect();
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://twitchtokengenerator.com");
        }
    }
}
