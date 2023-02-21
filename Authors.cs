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
    public partial class Authors : Form
    {
        public int ID = 0;
        public Authors(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
            Get_Info(id_user);
        }

        private void Authors_FormClosed(object sender, FormClosedEventArgs e)
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

        //Выводим данные их БД в таблицу dataGridView1.
        public void Get_Info(int ID)
        {
            string query = "SELECT author_id as 'Код автора', author_name as 'ФИО автора', author_birth as 'День рождения' FROM authors WHERE author_auth=" + ID + "; ";
            MySqlConnection conn = DBUtils.GetDBConnection();
            MySqlDataAdapter sda = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                this.dataGridView1.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[0].Width = 70;
                this.dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[1].Width = 280;
                this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[2].Width = 120;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        //Переход на форму Истории (Stories).
        private void button3_Click(object sender, EventArgs e)
        {
            Stories stories = new Stories(ID); // Переход на форму Stories.
            stories.Owner = this;
            this.Hide();
            stories.Show(); // Запуск окна Stories.
        }

        //Переход на форму Добавить автора (AuthorsAdd).
        private void button1_Click(object sender, EventArgs e)
        {
            AuthorsAdd authorsAdd = new AuthorsAdd(ID); // Переход на форму AuthorsAdd.
            authorsAdd.Owner = this;
            this.Hide();
            authorsAdd.Show(); // Запуск окна AuthorsAdd.
        }

        //Переход на форму Изменить автора (AuthorsChange).
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == null || textBox2.Text == "" || textBox2.Text == null)
            {
                MessageBox.Show("Для изменения данных выберете строку в таблице авторов!", "Сообщение");
                return;
            }
            else
            {
                int id_author = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString()); // Определяем id записи.
                Author.ID_author = id_author.ToString();
                Author.Fio_author = textBox1.Text;
                Author.Birth_author = textBox2.Text;
                AuthorsChange authorsChange = new AuthorsChange(ID); // Переход на форму AuthorsChange.
                authorsChange.Owner = this;
                this.Hide();
                authorsChange.Show(); // Запуск окна AuthorsChange.
            }
        }

        //Вывод в поля формы данных из строки , выбранной в таблице.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = ((DateTime)dataGridView1.CurrentRow.Cells[2].Value).ToString("dd-MM-yyyy");
        }
    }

    static class Author
    {
        public static string ID_author { get; set; }
        public static string Fio_author { get; set; }
        public static string Birth_author { get; set; }
    }
}
