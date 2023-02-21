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
    public partial class Stories : Form
    {
        public int ID = 0;
        public Stories(int id_user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ID = id_user;
            Get_Info(id_user);
            this.checkBox1.Checked = false;
        }

        private void Stories_FormClosed(object sender, FormClosedEventArgs e)
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
            string query = "SELECT story_id as 'Код произведения', story_name as 'Наименование текста', author_name as 'ФИО автора', story_text as 'Текст', genre_name as 'Вид жанра' FROM stories, authors, genres WHERE author_auth=" + ID + " AND stories.story_genre=genres.genre_id and stories.story_author=authors.author_id ORDER BY story_id; ";
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
                this.dataGridView1.Columns[0].Width = 100;
                this.dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[1].Width = 320;
                this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[2].Width = 280;
                this.dataGridView1.Columns[3].Visible = false;
                this.dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[4].Width = 220;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
            }
        }

        //Переход на форму Добавление истории (StoriesAdd).
        private void button1_Click(object sender, EventArgs e)
        {
            StoriesAdd storiesAdd = new StoriesAdd(ID); // Переход на форму StoriesAdd.
            storiesAdd.Owner = this;
            this.Hide();
            storiesAdd.Show(); // Запуск окна StoriesAdd.
        }

        //Переход на форму Изменение истории (StoriesChange).
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("") || textBox2.Text == "" || textBox2.Text == null || textBox3.Text == "" || textBox3.Text == null)
            {
                MessageBox.Show("Для изменения данных выберете строку в таблице Рассказы, Истории, Стихотворения!", "Сообщение");
                return;
            }
            else
            {
                int id_stor = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString()); // Определяем id записи.
                Stor.ID_stor = id_stor.ToString();
                Stor.Author_stor = textBox2.Text;
                Stor.Genre_stor = comboBox1.Text; ;
                Stor.Name_stor = textBox3.Text;
                Stor.Text_stor = text;
                StoriesChange storiesChange = new StoriesChange(ID); // Переход на форму StoriesChange.
                storiesChange.Owner = this;
                this.Hide();
                storiesChange.Show(); // Запуск окна StoriesChange.
            }
        }

        //Переход на форму Авторы (Authors).
        private void button7_Click(object sender, EventArgs e)
        {
            Authors authors = new Authors(ID); // Переход на форму Authors.
            authors.Owner = this;
            this.Hide();
            authors.Show(); // Запуск окна Authors.
        }

        //Переход на форму Жанры (Genres).
        private void button8_Click(object sender, EventArgs e)
        {
            Genres genres = new Genres(ID); // Переход на форму Genres.
            genres.Owner = this;
            this.Hide();
            genres.Show(); // Запуск окна Genres.
        }

        //Переход на форму Профиль (Profil).
        private void button9_Click(object sender, EventArgs e)
        {
            Profil profil = new Profil(ID); // Переход на форму Profil.
            profil.Owner = this;
            this.Hide();
            profil.Show(); // Запуск окна Profil.
        }

        //Выход и закрыте программы.
        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        string text;
        //Заполнение полей формы данными из строки, выбранной в таблице dataGridView1.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            Stor.Text_stor = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            Stor.Author_stor = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            Stor.Name_stor = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            StoriesShow storiesShow = new StoriesShow(ID); // Запуск формы StoriesShow.
            storiesShow.Owner = this;
            storiesShow.Show();
        }

        //Заполнение выпадающего списка жанров данными из БД.
        private void Stories_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT genre_name FROM genres; ";
                MySqlConnection conn = DBUtils.GetDBConnection();
                MySqlCommand cmDB = new MySqlCommand(query, conn);

                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.DropDownHeight = 150;
                    comboBox1.Items.Add(reader.GetString(0));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Удаление записи
        private void button3_Click(object sender, EventArgs e)
        {
            // Проверка, что выбрана строка в таблице арендных платежей.
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("Не выбрана строка в таблице Стихи, Истории, Рассказы!");
                return;
            }
            else
            {
                DialogResult res = MessageBox.Show("Удалить произведение?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    string valueCell = dataGridView1.CurrentCell.Value != null ? dataGridView1.CurrentCell.Value.ToString() : "";
                    string del = "DELETE FROM stories WHERE story_id = " + valueCell + ";";
                    Action(del);
                    Get_Info(ID);
                }
                else
                {
                    MessageBox.Show("Удаление записи отменено!");
                }
            }
        }

        //Строка поиска.
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBox1.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
            }
        }

        //Сортировка по названию
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
        }

        //Сортировка по автору
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
        }

        //Фильтрация по жанрам
        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || checkBox1.Checked == false)
            {
                MessageBox.Show("Для выполнения фильтации по жанрам - \nвыберете жанр и поставьте флажок!");
                return;           
            }
            else
            {
                string query = "SELECT story_id as 'Код произведения', story_name as 'Наименование текста', author_name as 'ФИО автора', story_text as 'Текст', genre_name as 'Вид жанра' FROM stories, authors, genres WHERE stories.story_genre=genres.genre_id and stories.story_author=authors.author_id and genre_name='" + comboBox1.Text + "' ORDER BY story_id; ";
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
                    this.dataGridView1.Columns[0].Width = 100;
                    this.dataGridView1.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridView1.Columns[1].Width = 320;
                    this.dataGridView1.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridView1.Columns[2].Width = 280;
                    this.dataGridView1.Columns[3].Visible = false;
                    this.dataGridView1.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this.dataGridView1.Columns[4].Width = 220;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла непредвиденая ошибка!" + Environment.NewLine + ex.Message);
                }
            }
        }

        //Сброс фильтрации.
        private void button6_Click(object sender, EventArgs e)
        {
            this.checkBox1.Checked = false;
            Get_Info(ID);
        }
    }

    static class Stor
    {
        public static string ID_stor { get; set; }
        public static string Name_stor { get; set; }
        public static string Author_stor { get; set; }
        public static string Text_stor { get; set; }
        public static string Genre_stor { get; set; }
    }
}
