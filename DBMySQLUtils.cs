using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Ucheb_3
{
    class DBMySQLUtils
    {
        public static MySqlConnection
      GetDBConnection(string host, int port, string database, string user, string password)
        {
            String connString = "Server=" + host + ";database=" + database + ";port=" + port.ToString() + ";user=" + user + ";password=" + password + ";";
            // Создаем объект MySqlConnection с именем conn и передаем ему строку для подключения connString:
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }
    }
}