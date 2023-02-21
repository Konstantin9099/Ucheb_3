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
    public partial class AuthorsChange : Form
    {
        public int ID = 0;
        public AuthorsChange(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
        }

        private void AuthorsChange_FormClosed(object sender, FormClosedEventArgs e)
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

        //Переход на форму Авторы (Authors).
        private void button2_Click(object sender, EventArgs e)
        {
            Authors authors = new Authors(ID); // Переход на форму Authors.
            authors.Owner = this;
            this.Hide();
            authors.Show(); // Запуск окна Authors.
        }

        //Изменение данных автора.
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, чтобы были заполнены поля ввода/вывода данных.
            if (textBox1.Text == null || textBox1.Text == "" )
            {
                MessageBox.Show("Введите ФИО автора!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Изменить данные автора?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    int id_author = int.Parse(Author.ID_author); // id автора.
                    string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string query = "UPDATE authors SET author_name='" + textBox1.Text + "', author_birth='" + date + "' WHERE author_id='" + id_author + "'; ";
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
                    MessageBox.Show("Данные автора изменены.", "Операция выполнена успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //Заполняем поля формы данными, полученными с формы "Авторы".
        private void AuthorsChange_Load(object sender, EventArgs e)
        {
            textBox1.Text = Author.Fio_author;
            dateTimePicker1.Text =  Author.Birth_author;
        }
    }
}
