using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Ucheb_3
{
    public partial class StoriesShow : Form
    {
        public int ID = 0;
        public StoriesShow(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
        }

        // Закрытие формы.
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void StoriesShow_Load(object sender, EventArgs e)
        {
            textBox1.Text = $"{Stor.Author_stor}" + $"\r\n\r\n{Stor.Name_stor}\r\n\r\n" + Stor.Text_stor;
        }
    }
}
