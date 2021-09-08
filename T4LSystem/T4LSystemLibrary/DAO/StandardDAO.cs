using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.DAO
{
    public abstract class StandardDAO
    {
        protected string TableName { get; set; }
        protected string PrimaryKeyName { get; set; } = "cod";
        protected abstract string CreateInsertQuery();
        protected abstract string CreateUpdateQuery();
        protected abstract MySqlParameter[] CreateParameters(StandardVO standardVO);
        protected abstract StandardVO CreateVO(DataRow dataRow);
        protected virtual string CreateDeleteQuery()
        {
            return $"DELETE FROM {TableName} WHERE {PrimaryKeyName} = @cod";
        }
        protected virtual string CreateSelectQuery()
        {
            return $"SELECT * FROM {TableName}";
        }
        public virtual void Insert(StandardVO standardVO)
        {
            string sqlCommand = CreateInsertQuery();
            DataBaseUtils.ExecuteSQLQuery(sqlCommand, CreateParameters(standardVO));
        }
        public virtual void Update(StandardVO standardVO)
        {
            string sqlCommand = CreateUpdateQuery();
            DataBaseUtils.ExecuteSQLQuery(sqlCommand, CreateParameters(standardVO));
        }
        public virtual void Delete(StandardVO standardVO)
        {
            string sqlCommand = CreateDeleteQuery();
            DataBaseUtils.ExecuteSQLQuery(sqlCommand, CreateParameters(standardVO));
        }
        public virtual List<StandardVO> Select(StandardVO standardVO = null, string sqlCommand = null)
        {
            if (sqlCommand == null)
                sqlCommand = CreateSelectQuery();
            MySqlParameter[] parameters = standardVO == null ? null : CreateParameters(standardVO);
            DataTable returnedTable = DataBaseUtils.ExecuteOnlySelectQuery(sqlCommand, parameters);
            if (returnedTable.Rows.Count == 0)
                return null;
            return ConvertDataTableToList(returnedTable);
        }
        public virtual List<StandardVO> ConvertDataTableToList(DataTable dataTable)
        {
            List<StandardVO> standardVOs = new List<StandardVO>();
            foreach (DataRow row in dataTable.AsEnumerable())
                standardVOs.Add(CreateVO(row));
            return standardVOs;
        }
    }
}
