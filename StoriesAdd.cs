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
    public partial class StoriesAdd : Form
    {
        public int ID = 0;
        public StoriesAdd(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
        }

        private void StoriesAdd_FormClosed(object sender, FormClosedEventArgs e)
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

        //Добавление рассказа.
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены все поля.
            if (textBox1.Text == null || textBox1.Text == "" || textBox2.Text == null || textBox2.Text == "" || comboBox1.Text.Equals("") || comboBox2.Text.Equals(""))
            {
                MessageBox.Show("Долны быть заполнены все поля ввода данных.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Добавить произведение?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    try
                    {
                        string query = "INSERT INTO stories (story_name, story_author, story_text, story_genre) VALUES ('" + textBox1.Text + "', (select author_id from authors where author_name='" + comboBox2.Text + "'), '" + textBox2.Text + "', (select genre_id from genres where genre_name='" + comboBox1.Text + "')); ";
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
                        MessageBox.Show("Произведение добавлено!", "Операция выполнена успешно");
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Произошла ошибка!" + Environment.NewLine + ex.Message);
                    }
                }
            }

        }

        //Заполняем элементы comboBox данными из БД.
        private void StoriesAdd_Load(object sender, EventArgs e)
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

                string query1 = "SELECT author_name FROM authors WHERE author_auth=" + ID + ";";
                MySqlConnection conn1 = DBUtils.GetDBConnection();
                MySqlCommand cmDB1 = new MySqlCommand(query1, conn1);

                conn1.Open();
                MySqlCommand command1 = new MySqlCommand(query1, conn1);
                MySqlDataReader reader1 = command1.ExecuteReader();
                while (reader1.Read())
                {
                    comboBox2.DropDownHeight = 150;
                    comboBox2.Items.Add(reader1.GetString("author_name"));
                }
                conn1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
