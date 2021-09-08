using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.DAO
{
    public class VendaProdutoDAO : StandardDAO
    {
        public VendaProdutoDAO()
        {
            TableName = "venda_produto";
        }
        protected override MySqlParameter[] CreateParameters(StandardVO standardVO)
        {
            VendaProdutoVO vendaProduto = standardVO as VendaProdutoVO;
            MySqlParameter[] parameters =
            {
                new MySqlParameter("cod", vendaProduto.Cod),
                new MySqlParameter("codVenda", vendaProduto.CodVenda),
                new MySqlParameter("codProduto", vendaProduto.CodProduto),
                new MySqlParameter("precoVenda", vendaProduto.PrecoVenda),
                new MySqlParameter("quantidade", vendaProduto.Quantidade)
            };

            return parameters;
        }
        protected override string CreateInsertQuery()
        {
            return $"INSERT INTO {TableName} (codVenda, codProduto, precoVenda, quantidade) VALUES (@codVenda, @codProduto, @precoVenda, @quantidade)";
        }
        protected override string CreateUpdateQuery()
        {
            return $"UPDATE {TableName} SET codVenda = @codVenda, codProduto = @codProduto, precoVenda = @precoVenda, quantidade = @quantidade WHERE cod = @cod";
        }
        /// <summary>
        /// This Method returns only venda_produto code.
        /// </summary>
        /// <param name="standardVO"></param>
        /// <returns></returns>
        public List<StandardVO> SelectSellProductByProductCode(StandardVO standardVO)
        {
            string sqlCommand = $"SELECT {PrimaryKeyName} FROM {TableName} WHERE codProduto = @cod";
            MySqlParameter[] parameters =
            {
                 new MySqlParameter("cod", standardVO.Cod)
            };
            return ConvertDataTableToList(DataBaseUtils.ExecuteOnlySelectQuery(sqlCommand, parameters));
        }
        protected override StandardVO CreateVO(DataRow dataRow)
        {
            VendaProdutoVO vendaProduto = new VendaProdutoVO
            {
                Cod = Convert.ToInt64(dataRow["cod"]),
                CodVenda = dataRow.Table.Columns.Contains("codVenda") ? Convert.ToInt64(dataRow["codVenda"]) : 0,
                CodProduto = dataRow.Table.Columns.Contains("codProduto") ? Convert.ToInt64(dataRow["codProduto"]) : 0,
                PrecoVenda = dataRow.Table.Columns.Contains("precoVenda") ? Convert.ToDouble(dataRow["precoVenda"]) : 0,
                Quantidade = dataRow.Table.Columns.Contains("quantidade") ? Convert.ToDouble(dataRow["quantidade"]) : 0
            };

            return vendaProduto;
        }
    }
}
