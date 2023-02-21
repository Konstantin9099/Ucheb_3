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
    public partial class StoriesChange : Form
    {
        public int ID = 0;
        public StoriesChange(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
        }

        private void StoriesChange_FormClosed(object sender, FormClosedEventArgs e)
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

        //Переход на форму Стихи, Истории, Рассказы (Stories).
        private void button2_Click(object sender, EventArgs e)
        {
            Stories stories = new Stories(ID); // Переход на форму Stories.
            stories.Owner = this;
            this.Hide();
            stories.Show(); // Запуск окна Stories.
        }

        //Изменение данных произведения.
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены поля ввода/вывода данных.
            if (textBox1.Text == "" || textBox1.Text == null || textBox2.Text == "" || textBox2.Text == null || comboBox1.Text.Equals("") || comboBox2.Text.Equals(""))
            {
                MessageBox.Show("Должны быть заполнены все поля ввода данных!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Сохранить внесенные изменения?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    int id_st = int.Parse(Stor.ID_stor); // id произведения.
                    string query = "UPDATE stories SET story_name='" + textBox1.Text + "', story_author=(select author_id from authors where author_name='" + comboBox2.Text + "'), story_text='" + textBox2.Text + "', story_genre=(select genre_id from genres where genre_name='" + comboBox1.Text + "') WHERE story_id=" + Stor.ID_stor + "; ";
                    MySqlConnection conn = DBUtils.GetDBConnection();
                    MySqlCommand cmDB = new MySqlCommand(query, conn);
                    try
                    {
                        conn.Open();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла непредвиденная ошибка!" + Environment.NewLine + ex.Message);
                    }
                    Action(query);
                    MessageBox.Show("Внесенные изменения сохранены.", "Операция выполнена успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //Заполняем выпадающие списки comboBox данными из БД.
        private void StoriesChange_Load(object sender, EventArgs e)
        {
            comboBox1.Text = Stor.Genre_stor;
            comboBox2.Text = Stor.Author_stor;
            textBox1.Text = Stor.Name_stor;
            textBox2.Text = Stor.Text_stor;
        }

        //Заполняем выпадающий список comboBox1 данными из БД.
        private void comboBox1_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT genre_name FROM genres;";
                MySqlConnection conn = DBUtils.GetDBConnection();
                MySqlCommand cmDB = new MySqlCommand(query, conn);

                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.DropDownHeight = 150;
                    comboBox1.Items.Add(reader.GetString("genre_name"));
                    //comboBox1.Items.Add(reader["genre_name"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //Заполняем выпадающий список comboBox2 данными из БД.
        private void comboBox2_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT author_name FROM authors WHERE author_auth=" + ID + ";";
                MySqlConnection conn = DBUtils.GetDBConnection();
                MySqlCommand cmDB = new MySqlCommand(query, conn);

                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.DropDownHeight = 150;
                    comboBox2.Items.Add(reader.GetString("author_name"));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }
    }
}
