using System;
using T4LSystemBackEnd.Utils;
using T4LSystemLibrary.CustomExceptions;
using T4LSystemLibrary.DAO;
using T4LSystemLibrary.Singleton;
using T4LSystemLibrary.VO;

namespace T4LSystemBackEnd.BusinessLayer
{
    public class LoginController
    {
        public static bool VerifyUserCredentialExistance(string userCredential)
        {
            try
            {
                UserLoginDAO userLoginDAO = new UserLoginDAO();
                UserLoginVO returnedUser = userLoginDAO.SelectUserLoginByCredential(userCredential);
                return (returnedUser != null) ? true : false;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao verificar existência de uma credencial de usuário. Erro: " + error.Message);
                return false;
            }
        }
        public static bool InsertUser(UserLoginVO userLogin)
        {
            try
            {
                if (VerifyUserCredentialExistance(userLogin.Credential))
                    return false;
                userLogin.Password = SecurityTools.GetPasswordHash(userLogin.Password);
                UserLoginDAO userLoginDAO = new UserLoginDAO();
                userLoginDAO.Insert(userLogin);
                return true;
            }
            catch (SecurityPoliciesException securityError)
            {
                // Implementação da política de segurança para uma possível feature nova.
                LogController.WriteLog("A política de segurança bloqueou o cadastro de usuário. Erro: " + securityError.Message);
                return false;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao cadastrar novo usuário. Erro: " + error.Message);
                return false;
            }
        }
        public static bool UserAuthenticate(string userCredential, string userPassword, out UserLoginVO user)
        {
            try
            {
                UserLoginDAO userLoginDAO = new UserLoginDAO();
                UserLoginVO returnedUser = userLoginDAO.SelectUserLoginByCredential(userCredential);
                if (returnedUser == null)
                {
                    user = null;
                    return false;
                }
                else if (SecurityTools.VerifyPasswordHash(userPassword, returnedUser.Password))
                {
                    user = returnedUser;
                    UserLoginSingleton.SetUser(returnedUser);
                    return true;
                }
                else
                {
                    user = null;
                    return false;
                }
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro na autenticação de usuário. Erro: " + error.Message);
                user = null;
                return false;
            }
        }
        private static UserLoginVO SelectUser(UserLoginVO userLogin)
        {
            try
            {
                UserLoginDAO userLoginDAO = new UserLoginDAO();
                return userLoginDAO.Select(userLogin)[0] as UserLoginVO;
            }
            catch (SecurityPoliciesException securityError)
            {
                // Implementação da política de segurança para uma possível feature nova.
                LogController.WriteLog("A política de segurança bloqueou a consulta de usuário. Erro: " + securityError.Message);
                return null;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao cadastrar novo usuário. Erro: " + error.Message);
                return null;
            }
        }
    }
}
