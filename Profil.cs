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
    public partial class Profil : Form
    {
        public int ID = 0;
        public Profil(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
        }

        private void Profil_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //Функция, позволяющая отправить команду на сервер БД для оптимизации кода.
        public void Action(string query)
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            MySqlCommand cmDB = new MySqlCommand(query, conn);
            try
            {
                conn.Open();
                cmDB.ExecuteReader();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        //Переход на форму Истории (Stories).
        private void button2_Click(object sender, EventArgs e)
        {
            Stories stories = new Stories(ID); // Переход на форму Stories.
            stories.Owner = this;
            this.Hide();
            stories.Show(); // Запуск окна Stories.
        }

        //Изменяес логин и пароль.
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Изменить")
            {
                label1.Visible = true;
                label2.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                button1.Text = "Сохранить";
            }
            else if (button1.Text == "Сохранить")
            {
                string query = "update auth set auth_log ='" + textBox1.Text + "', auth_pwd ='" + textBox2.Text + "' where auth_id = " + ID + ";";
                MySqlConnection conn = DBUtils.GetDBConnection();
                MySqlCommand cmDB = new MySqlCommand(query, conn);
                try
                {
                    conn.Open();
                    cmDB.ExecuteReader();
                    conn.Close();
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    button1.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Text = "Данные профиля изменены!";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Возникла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                }
            }

        }
    }
}
