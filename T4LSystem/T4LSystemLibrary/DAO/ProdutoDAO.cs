using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.DAO
{
    public class ProdutoDAO : StandardDAO
    {
        public ProdutoDAO()
        {
            TableName = "produto";
        }
        protected override MySqlParameter[] CreateParameters(StandardVO standardVO)
        {
            ProdutoVO product = standardVO as ProdutoVO;
            MySqlParameter[] parameters =
            {
                new MySqlParameter("cod", product.Cod),
                new MySqlParameter("descricao", product.Descricao),
                new MySqlParameter("codGrupo", product.CodGrupo),
                new MySqlParameter("codBarra", product.CodBarra),
                new MySqlParameter("precoCusto", product.PrecoCusto),
                new MySqlParameter("precoVenda", product.PrecoVenda),
                new MySqlParameter("ativo", product.Ativo),
                new MySqlParameter("codMedida", product.CodMedida),
                new MySqlParameter("dataHoraCadastro", product.DataHoraCadastro)
            };
            return parameters;
        }
        protected override string CreateInsertQuery()
        {
            return $"INSERT INTO {TableName} (descricao, codGrupo, codBarra, precoCusto, precoVenda, ativo, codMedida, dataHoraCadastro) VALUES (@descricao, @codGrupo, @codBarra, @precoCusto, @precoVenda, @ativo, @codMedida, @dataHoraCadastro)";
        }
        protected override string CreateUpdateQuery()
        {
            return $"UPDATE {TableName} SET descricao = @descricao, codGrupo = @codGrupo, codBarra = @codBarra, precoCusto = @precoCusto, precoVenda = @precoVenda, ativo = @ativo, codMedida = @codMedida, dataHoraCadastro = @dataHoraCadastro WHERE cod = @cod";
        }
        public ProdutoVO SelectProductByCode(int code)
        {
            string sqlCommand = $"SELECT * FROM {TableName} WHERE cod = @cod";
            MySqlParameter[] parameters = { new MySqlParameter("cod", code) };
            List<StandardVO> returnedProductList = ConvertDataTableToList(DataBaseUtils.ExecuteOnlySelectQuery(sqlCommand, parameters));
            return (returnedProductList.Count > 0) ? returnedProductList[0] as ProdutoVO : null;
        }
        protected override StandardVO CreateVO(DataRow dataRow)
        {
            ProdutoVO product = new ProdutoVO
            {
                Cod = Convert.ToInt64(dataRow["cod"]),
                Descricao = dataRow["descricao"].ToString(),
                CodGrupo = Convert.ToInt32(dataRow["codGrupo"]),
                CodBarra = dataRow["codBarra"].ToString(),
                PrecoCusto = Convert.ToDouble(dataRow["precoCusto"]),
                PrecoVenda = Convert.ToDouble(dataRow["precoVenda"]),
                Ativo = Convert.ToInt16(dataRow["ativo"]),
                CodMedida = Convert.ToInt64(dataRow["codMedida"]),
                DataHoraCadastro = Convert.ToDateTime(dataRow["dataHoraCadastro"])
            };

            return product;
        }
    } 
}
