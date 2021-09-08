using MySql.Data.MySqlClient;
using System.Data;

namespace T4LSystemLibrary.DAO
{
    public class DataBaseUtils
    {
        internal static void ExecuteSQLQuery(string sqlCommand, MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = DataBaseConnection.GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(sqlCommand, connection))
                {
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        internal static DataTable ExecuteOnlySelectQuery(string sqlCommand, MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = DataBaseConnection.GetConnection())
            {
                using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCommand, connection))
                {
                    if (parameters != null)
                        dataAdapter.SelectCommand.Parameters.AddRange(parameters);
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    return table;
                }
            }
        }
    }
}
