using System;
using System.Data;
using MySql.Data.MySqlClient;
using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.DAO
{
    public class MedidaDAO : StandardDAO
    {
        public MedidaDAO()
        {
            TableName = "medida";
        }
        protected override string CreateInsertQuery()
        {
            // Implementado para um futuro uso.
            return $"INSERT INTO {TableName} (unidadeMedida) VALUES (@unidadeMedida)";
        }
        protected override string CreateUpdateQuery()
        {
            // Implementado para um futuro uso.
            return $"UPDATE {TableName} SET unidadeMedida = @unidadeMedida WHERE cod = @cod";
        }
        protected override MySqlParameter[] CreateParameters(StandardVO standardVO)
        {
            MedidaVO medida = standardVO as MedidaVO;
            MySqlParameter[] parameters =
            {
                new MySqlParameter("cod", medida.Cod),
                new MySqlParameter("unidadeMedida", medida.UnidadeMedida)
            };
            return parameters;
        }
        protected override StandardVO CreateVO(DataRow dataRow)
        {
            MedidaVO medida = new MedidaVO
            {
                Cod = Convert.ToInt64(dataRow["cod"]),
                UnidadeMedida = dataRow["unidadeMedida"].ToString()
            };
            return medida;
        }
    }
}
