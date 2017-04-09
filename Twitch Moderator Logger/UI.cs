using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Twitch_Moderator_Logger
{
    public partial class UI : Form
    {
        private static UI instance;

        public UI()
        {
            InitializeComponent();
        }

        public static UI Instance
        {
            get
            {
                return instance;
            }
        }

        private void UI_Load(object sender, EventArgs e)
        {
            // TODO: fix
            CheckForIllegalCrossThreadCalls = false;
            instance = this;
            TwitchLib.TwitchApi.SetClientId(Common.TwitchClientId);
            preparePubSub();
            bool notFirstRun = Settings.LoadSettings();
            buildColumns();
            if(!notFirstRun)
            {
                Login lg = new Login();
                lg.Show();
            } else
            {
                this.Width = Common.Config.AppWidth;
                this.Height = Common.Config.AppHeight;
                Common.PubSub.Connect();
            }
            loadColumnWidths();
        }

        private void UI_Closing(object sender, FormClosingEventArgs e)
        {
            Common.Config.AppWidth = this.Width;
            Common.Config.AppHeight = this.Height;
            syncColumnWidths();
            Settings.SaveSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColumnCustomizer cc = new ColumnCustomizer();
            cc.Show();
        }

        #region UI Resizing
        private Size priorSize;
        private bool isResizing = false;
        private void UI_ResizeBegin(object sender, EventArgs e)
        {
            isResizing = true;
            priorSize = listView1.Size;
        }

        private void UI_Resize(object sender, EventArgs e)
        {
            foreach (ColumnHeader ch in listView1.Columns)
            {
                foreach (var column in Common.Config.Columns)
                {
                    if (ch.Text == column.Name)
                    {
                        int newWidth = (int)(((double)column.Width / priorSize.Width) * listView1.Width);
                        ch.Width = newWidth;
                    }
                }
            }
        }

        private void UI_ResizeEnd(object sender, EventArgs e)
        {
            isResizing = false;
            foreach (ColumnHeader ch in listView1.Columns)
                foreach (var column in Common.Config.Columns)
                    if (ch.Text == column.Name)
                        column.Width = ch.Width;
        }

        private void listView1_ColumnClicked(object sender, ColumnClickEventArgs e)
        {
            Console.WriteLine("Clicked!");
        }

        public bool isAvailable = false;
        private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if(!isResizing && isAvailable)
            {
                var ch = listView1.Columns[e.ColumnIndex];
                foreach (var column in Common.Config.Columns)
                    if (column.Name == ch.Text)
                        column.Width = ch.Width;
            }
        }
        #endregion

        private void preparePubSub()
        {
            Common.PubSub.OnPubSubServiceConnected += Twitch_Moderator_Logger.Events.onConnect;
            Common.PubSub.OnListenResponse += Twitch_Moderator_Logger.Events.onListenResponse;
            Common.PubSub.OnBan += Twitch_Moderator_Logger.Events.onBan;
            Common.PubSub.OnTimeout += Twitch_Moderator_Logger.Events.onTimeout;
            Common.PubSub.OnUnban += Twitch_Moderator_Logger.Events.onUnban;
        }

        private void syncColumnWidths()
        {
            foreach (ColumnHeader ch in listView1.Columns)
                foreach (var column in Common.Config.Columns)
                    if (ch.Text == column.Name)
                        column.Width = ch.Width;
        }

        private void loadColumnWidths()
        {
            priorSize = listView1.Size;
            foreach (ColumnHeader ch in listView1.Columns)
            {
                foreach (var column in Common.Config.Columns)
                {
                    if (ch.Text == column.Name)
                    {
                        int newWidth = (int)(((double)column.Width / priorSize.Width) * listView1.Width);
                        ch.Width = newWidth;
                    }
                }
            }
        }

        private void buildColumns()
        {
            // sort columns
            Array.Sort(Common.Config.Columns, delegate (Models.Column x, Models.Column y) { return x.DisplayOrder.CompareTo(y.DisplayOrder); });

            // build listview
            foreach(var column in Common.Config.Columns)
            {
                if(column.Visible)
                {
                    ColumnHeader ch = new ColumnHeader();
                    ch.Text = column.Name;
                    ch.Width = column.Width;
                    listView1.Columns.Add(ch);
                }
            }
        }

        private void buildRows()
        {
            if(Common.Data.Count != 0)
                foreach (var row in Common.Data)
                    addListing(row);
        }

        public void toggleConnected(bool isConnected)
        {
            if(isConnected)
            {
                toolStripStatusLabel1.Text = "Connected";
                toolStripStatusLabel1.ForeColor = Color.Green;
            } else
            {
                toolStripStatusLabel1.Text = "Disconnected";
                toolStripStatusLabel1.ForeColor = Color.Red;
            }
        }

        public void reconstructUI()
        {
            listView1.Items.Clear();
            listView1.Columns.Clear();
            buildColumns();
            buildRows();
        }

        public int getListViewWidth()
        {
            return listView1.Width;
        }

        public void addListing(Models.Listing listing)
        {
            ListViewItem lvi = new ListViewItem();
            for(int i = 0; i < listView1.Columns.Count; i++)
            {
                switch (listView1.Columns[i].Text)
                {
                    case "Date":
                        if (i == 0)
                            lvi.Text = listing.DateTime.ToShortDateString();
                        else
                            lvi.SubItems.Add(listing.DateTime.ToShortDateString());
                        break;
                    case "Time":
                        if (i == 0)
                            lvi.Text = listing.DateTime.ToShortTimeString();
                        else
                            lvi.SubItems.Add(listing.DateTime.ToShortTimeString());
                        break;
                    case "Moderator Name":
                        if (i == 0)
                            lvi.Text = listing.ModeratorName;
                        else
                            lvi.SubItems.Add(listing.ModeratorName);
                        break;
                    case "Action Performed":
                        if (i == 0)
                            lvi.Text = listing.ActionPerformed;
                        else
                            lvi.SubItems.Add(listing.ActionPerformed);
                        break;
                    case "Viewer Name":
                        if (i == 0)
                            lvi.Text = listing.ViewerName;
                        else
                            lvi.SubItems.Add(listing.ViewerName);
                        break;
                    case "Action Message":
                        if (i == 0)
                            lvi.Text = listing.ActionMessage;
                        else
                            lvi.SubItems.Add(listing.ActionMessage);
                        break;
                }
            }

            listView1.Items.Add(lvi);
        }
    }
}
