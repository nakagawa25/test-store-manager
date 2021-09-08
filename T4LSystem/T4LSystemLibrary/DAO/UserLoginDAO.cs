using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using T4LSystemLibrary.CustomExceptions;
using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.DAO
{
    public class UserLoginDAO : StandardDAO
    {
        public UserLoginDAO()
        {
            TableName = "userLogin";
        }
        protected override MySqlParameter[] CreateParameters(StandardVO standardVO)
        {
            UserLoginVO userLogin = standardVO as UserLoginVO;
            MySqlParameter[] parameters =
            {
                new MySqlParameter("cod", userLogin.Cod),
                new MySqlParameter("credential", userLogin.Credential),
                new MySqlParameter("password", userLogin.Password)
            };
            return parameters;
        }
        protected override StandardVO CreateVO(DataRow dataRow)
        {
            UserLoginVO userLogin = new UserLoginVO
            {
                Cod = Convert.ToInt64(dataRow["cod"]),
                Credential = dataRow["credential"].ToString(),
                Password = dataRow["password"].ToString()
            };
            return userLogin;
        }
        public UserLoginVO SelectUserLoginByCredential(string userCredential)
        {
            string sqlCommand = $"SELECT * FROM {TableName} WHERE credential = @credential";
            MySqlParameter[] parameter = { new MySqlParameter("credential", userCredential) };
            List<StandardVO> returnedUserList = ConvertDataTableToList(DataBaseUtils.ExecuteOnlySelectQuery(sqlCommand, parameter));
            return (returnedUserList.Count > 0) ? returnedUserList[0] as UserLoginVO : null;
        }
        protected override string CreateInsertQuery()
        {
            return $"INSERT INTO {TableName} (credential, password) VALUES (@credential, @password)";
        }
        protected override string CreateUpdateQuery()
        {
            throw new SecurityPoliciesException("Apenas o Administrador de Banco de Dados pode atualizar usuários");
        }
        protected override string CreateDeleteQuery()
        {
            throw new SecurityPoliciesException("Apenas o Administrador de Banco de Dados pode excluir usuários");
        }
    }
}
