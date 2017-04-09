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
    public partial class ColumnCustomizer : Form
    {
        public ColumnCustomizer()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItems.Count > 0)
            {
                string item = listBox1.SelectedItem.ToString();
                listBox2.Items.Add(item);
                listBox1.Items.Remove(item);
                foreach (var column in Common.Config.Columns)
                    if (column.Name == item)
                        column.Visible = false;
            } else
            {
                MessageBox.Show("Must select a column to hide.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItems.Count > 0)
            {
                string item = listBox2.SelectedItem.ToString();
                listBox1.Items.Add(item);
                listBox2.Items.Remove(item);
                foreach (var column in Common.Config.Columns)
                    if (column.Name == item)
                        column.Visible = true;
            }
            else
            {
                MessageBox.Show("Must select a column to show.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                string item = listBox1.SelectedItem.ToString();
                int index = listBox1.Items.IndexOf(item);
                if(index != 0)
                {
                    string itemAbove = listBox1.Items[index - 1].ToString();
                    Common.Config.Columns.FirstOrDefault(x => x.Name == item).DisplayOrder -= 1;
                    Common.Config.Columns.FirstOrDefault(x => x.Name == itemAbove).DisplayOrder += 1;

                    // sort columns
                    Array.Sort(Common.Config.Columns, delegate (Models.Column x, Models.Column y) { return x.DisplayOrder.CompareTo(y.DisplayOrder); });

                    listBox1.Items.Clear();
                    foreach (var column in Common.Config.Columns)
                        if(column.Visible)
                            listBox1.Items.Add(column.Name);

                    listBox1.SelectedItem = item;
                } else
                {
                    MessageBox.Show("Can't move column beyond first place.");
                }
            }
            else
            {
                MessageBox.Show("Must select a column to move up.");
            }
        }

        private void ColumnCustomizer_Load(object sender, EventArgs e)
        {
            // sort columns
            Array.Sort(Common.Config.Columns, delegate (Models.Column x, Models.Column y) { return x.DisplayOrder.CompareTo(y.DisplayOrder); });

            foreach (var column in Common.Config.Columns)
            {
                if(column.Visible)
                {
                    listBox1.Items.Add(column.Name);
                } else
                {
                    listBox2.Items.Add(column.Name);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                string item = listBox1.SelectedItem.ToString();
                int index = listBox1.Items.IndexOf(item);
                if (index != listBox1.Items.Count - 1)
                {
                    string itemBelow = listBox1.Items[index + 1].ToString();
                    Common.Config.Columns.FirstOrDefault(x => x.Name == item).DisplayOrder += 1;
                    Common.Config.Columns.FirstOrDefault(x => x.Name == itemBelow).DisplayOrder -= 1;

                    // sort columns
                    Array.Sort(Common.Config.Columns, delegate (Models.Column x, Models.Column y) { return x.DisplayOrder.CompareTo(y.DisplayOrder); });

                    listBox1.Items.Clear();
                    foreach (var column in Common.Config.Columns)
                        if(column.Visible)
                            listBox1.Items.Add(column.Name);

                    listBox1.SelectedItem = item;
                }
                else
                {
                    MessageBox.Show("Can't move column beyond first place.");
                }
            }
            else
            {
                MessageBox.Show("Must select a column to move up.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // set hiddens
            foreach (string hiddenColumn in listBox2.Items)
                foreach (var column in Common.Config.Columns)
                    if (hiddenColumn == column.Name)
                        column.Visible = false;

            // create new order
            List<Models.Column> newOrder = new List<Models.Column>();
            foreach(string visibleColumn in listBox1.Items)
            {
                foreach(var column in Common.Config.Columns)
                {
                    if(visibleColumn == column.Name)
                    {
                        column.Visible = true;
                        newOrder.Add(column);
                    }
                }
            }

            Common.Config.Columns = newOrder.ToArray();

            UI.Instance.reconstructUI();
        }
    }
}
