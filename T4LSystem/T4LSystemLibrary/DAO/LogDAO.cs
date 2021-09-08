using System;
using System.Data;
using MySql.Data.MySqlClient;
using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.DAO
{
    public class LogDAO : StandardDAO
    {
        public LogDAO()
        {
            TableName = "log";
        }
        protected override string CreateInsertQuery()
        {
            return $"INSERT INTO {TableName} (message, userName, dateTime) VALUES (@message, @userName, @dateTime)";
        }
        protected override string CreateUpdateQuery()
        {
            // Caso seja necessário algum dia, o UPDATE já está pronto.
            return $"UPDATE {TableName} SET message = @message, userName = @userName, dateTime = @dateTime WHERE cod = @cod";
        }
        protected override MySqlParameter[] CreateParameters(StandardVO standardVO)
        {
            LogVO log = standardVO as LogVO;
            MySqlParameter[] parameters =
            {
                new MySqlParameter("cod", log.Cod),
                new MySqlParameter("message", log.Message),
                new MySqlParameter("userName", log.UserName),
                new MySqlParameter("dateTime", log.DateTime)
            };
            return parameters;
        }
        protected override StandardVO CreateVO(DataRow dataRow)
        {
            LogVO log = new LogVO
            {
                Cod = Convert.ToInt64(dataRow["cod"]),
                Message = dataRow["message"].ToString(),
                UserName = dataRow["userName"].ToString(),
                DateTime = Convert.ToDateTime(dataRow["dateTime"])
            };
            return log;
        }
    }
}
