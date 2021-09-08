using System;
using System.Data;
using MySql.Data.MySqlClient;
using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.DAO
{
    public class VendaDAO : StandardDAO
    {
        public VendaDAO()
        {
            TableName = "venda";
        }
        protected override MySqlParameter[] CreateParameters(StandardVO standardVO)
        {
            VendaVO venda = standardVO as VendaVO;
            MySqlParameter[] parameters =
            {
                new MySqlParameter("cod", venda.Cod),
                new MySqlParameter("clienteDocumento", venda.ClienteDocumento),
                new MySqlParameter("clienteNome", venda.ClienteNome),
                new MySqlParameter("obs", venda.Obs),
                new MySqlParameter("total", venda.Total),
                new MySqlParameter("dataHora", venda.DataHora)
            };

            return parameters;
        }
        protected override string CreateInsertQuery()
        {
            return $"INSERT INTO {TableName} (clienteDocumento, clienteNome, obs, total, dataHora) VALUES (@clienteDocumento, @clienteNome, @obs, @total, @dataHora)";
        }
        protected override string CreateUpdateQuery()
        {
            return $"UPDATE {TableName} SET clienteDocumento = @clienteDocumento. clienteNome = @clienteNome, obs = @obs, total = @total, dataHora = @dataHora WHERE cod = @cod";
        }
        public int SelectLastSaleCode()
        {
            string sqlCommand = $"SELECT max(cod) AS 'cod' FROM {TableName}";
            DataTable returnedTable = DataBaseUtils.ExecuteOnlySelectQuery(sqlCommand, null);
            return Convert.ToInt32(returnedTable.Rows[0]["cod"]);
        }
        protected override StandardVO CreateVO(DataRow dataRow)
        {
            VendaVO venda = new VendaVO
            {
                Cod = Convert.ToInt64(dataRow["cod"]),
                ClienteDocumento = dataRow["clienteDocumento"].ToString(),
                ClienteNome = dataRow["clienteNome"].ToString(),
                Obs = dataRow["obs"].ToString(),
                Total = Convert.ToDouble(dataRow["total"]),
                DataHora = Convert.ToDateTime(dataRow["dataHora"])
            };

            return venda;
        }
    }
}
