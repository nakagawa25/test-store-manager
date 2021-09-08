using MySql.Data.MySqlClient;

namespace T4LSystemLibrary.DAO
{
    internal static class DataBaseConnection
    {
        internal static MySqlConnection GetConnection()
        {
            // TODO: 
            // Armazenar Hash da senha em uma variável de ambiente ou afins...

            string stringConnection = "Persist Security Info = False;server = localhost;database = testdev;uid = COLOQUE_O_USER_DO_BD; pwd = COLOQUE_A_SENHA_DO_BD";
            MySqlConnection connection = new MySqlConnection(stringConnection);
            connection.Open();
            return connection;
        }
    }
}
