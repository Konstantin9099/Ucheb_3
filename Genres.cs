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
    public partial class Genres : Form
    {
        public int ID = 0;
        public Genres(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Get_Info(id_user);
            ID = id_user;
        }

        private void Genres_FormClosed(object sender, FormClosedEventArgs e)
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
            string query = "SELECT genre_id as 'Код жанра', genre_name as 'Наименование жанра' FROM genres; ";
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

        //Переход на форму Добавление жанра (GenresAdd).
        private void button1_Click(object sender, EventArgs e)
        {
            GenresAdd genresAdd = new GenresAdd(ID); // Переход на форму GenresAdd.
            genresAdd.Owner = this;
            this.Hide();
            genresAdd.Show(); // Запуск окна GenresAdd.
        }

        //Переход на форму Изменение жанра (GenresChange).
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == null)
            {
                MessageBox.Show("Для изменения данных выберете строку в таблице жанров!", "Сообщение");
                return;
            }
            else
            {
                int id_genr = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString()); // Определяем id записи.
                Genr.ID_genr = id_genr.ToString();
                Genr.Name_genr = textBox1.Text;
                GenresChange genresChange = new GenresChange(ID); // Переход на форму GenresChange.
                genresChange.Owner = this;
                this.Hide();
                genresChange.Show(); // Запуск окна GenresChange.
            }
        }

        //Вывод наименования жанра в текстовое поле, при выборе строки в таблице dataGridView1.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
        }
    }

    static class Genr
    {
        public static string ID_genr { get; set; }
        public static string Name_genr { get; set; }
    }
}
