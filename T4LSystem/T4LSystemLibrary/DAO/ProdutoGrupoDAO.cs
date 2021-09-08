using System;
using System.Data;
using MySql.Data.MySqlClient;
using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.DAO
{
    public class ProdutoGrupoDAO : StandardDAO
    {
        public ProdutoGrupoDAO()
        {
            TableName = "produto_grupo";
        }
        protected override MySqlParameter[] CreateParameters(StandardVO standardVO)
        {
            ProdutoGrupoVO produtoGrupo = standardVO as ProdutoGrupoVO;
            MySqlParameter[] parameters =
            {
                new MySqlParameter("cod", produtoGrupo.Cod),
                new MySqlParameter("nome", produtoGrupo.Nome)
            };

            return parameters;
        }
        protected override string CreateInsertQuery()
        {
            return $"INSERT INTO {TableName} (nome) VALUES (@nome)";
        }
        protected override string CreateUpdateQuery()
        {
            return $"UPDATE {TableName} SET nome = @nome WHERE cod = @cod";
        }
        protected override StandardVO CreateVO(DataRow dataRow)
        {
            ProdutoGrupoVO produtoGrupo = new ProdutoGrupoVO();
            produtoGrupo.Cod = Convert.ToInt64(dataRow["cod"]);
            produtoGrupo.Nome = dataRow["nome"].ToString();

            return produtoGrupo;
        }
    }
}
