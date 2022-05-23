using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FileDateChange
{
    public partial class FormMain : Form
    {
        string lastFolder = null;

        public FormMain()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now;
            toolTip1.SetToolTip(checkBox1, "Включая подпапки.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lastFolder != null)
            {
                if (Directory.Exists(lastFolder))
                {
                    openFileDialog1.InitialDirectory = lastFolder;
                }
            }
            else
            {
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK && openFileDialog1.FileNames.Length > 0)
            {
                listBox1.Items.AddRange(openFileDialog1.FileNames);
                button2.Enabled = true;
                lastFolder = Path.GetDirectoryName(openFileDialog1.FileName);
                Text = "File Date Change " + "(" + listBox1.Items.Count.ToString() + ")";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                dateTimePicker1.Value = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value));
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    touchFile(listBox1.Items[i].ToString());
                }
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            button2.Enabled = false;
            Text = "File Date Change";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                if (File.Exists(listBox1.Items[0].ToString()))
                {
                    MessageBox.Show("Создан:		" + File.GetCreationTime(listBox1.Items[0].ToString()) + Environment.NewLine + "Изменен:	" + File.GetLastWriteTime(listBox1.Items[0].ToString()) + Environment.NewLine + "Открыт:		" + File.GetLastAccessTime(listBox1.Items[0].ToString()) + Environment.NewLine);
                }
                else if (Directory.Exists(listBox1.Items[0].ToString()))
                {
                    MessageBox.Show("Создан:		" + Directory.GetCreationTime(listBox1.Items[0].ToString()) + Environment.NewLine + "Изменен:	" + Directory.GetLastWriteTime(listBox1.Items[0].ToString()) + Environment.NewLine + "Открыт:		" + Directory.GetLastAccessTime(listBox1.Items[0].ToString()) + Environment.NewLine);
                }
            }
        }

        private void touchFile(string file)
        {
            try
            {
                if (Directory.Exists(file))
                {
                    if (checkBox2.Checked)
                    {
                        Directory.SetLastWriteTime(file, dateTimePicker1.Value);
                    }
                    if (checkBox3.Checked)
                    {
                        if (checkBox2.Checked)
                        {
                            Thread.Sleep(50);
                        }
                        Directory.SetCreationTime(file, dateTimePicker1.Value);
                    }
                    if (checkBox4.Checked)
                    {
                        if (checkBox2.Checked || checkBox3.Checked)
                        {
                            Thread.Sleep(50);
                        }
                        Directory.SetLastAccessTime(file, dateTimePicker1.Value);
                    }
                }
                else if (File.Exists(file))
                {
                    if (checkBox2.Checked)
                    {
                        File.SetLastWriteTime(file, dateTimePicker1.Value);
                    }
                    if (checkBox3.Checked)
                    {
                        if (checkBox2.Checked)
                        {
                            Thread.Sleep(50);
                        }
                        File.SetCreationTime(file, dateTimePicker1.Value);
                    }
                    if (checkBox4.Checked)
                    {
                        if (checkBox2.Checked || checkBox3.Checked)
                        {
                            Thread.Sleep(50);
                        }
                        File.SetLastAccessTime(file, dateTimePicker1.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось изменить в: " + file + Environment.NewLine + ex.Message);
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            listBox1.Items.Clear();
            foreach (string line1 in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                listBox1.Items.Add(line1);
                if (checkBox1.Checked && Directory.Exists(line1))
                {
                    foreach (string line2 in Directory.GetFileSystemEntries(line1, "*", SearchOption.AllDirectories))
                    {
                        listBox1.Items.Add(line2);
                    }
                }
            }
            button2.Enabled = true;
            Text = "File Date Change " + "(" + listBox1.Items.Count.ToString() + ")";
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
        }
    }
}
