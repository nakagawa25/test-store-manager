using System;
using T4LSystemLibrary.DAO;
using T4LSystemLibrary.Singleton;
using T4LSystemLibrary.VO;

namespace T4LSystemBackEnd.BusinessLayer
{
    public class LogController
    {
        public static bool WriteLog(string message)
        {
            try
            {
                if (string.IsNullOrEmpty(message.Trim()))
                    return false;
                LogDAO logDAO = new LogDAO();
                LogVO log = new LogVO
                {
                    Message = message.Trim(),
                    UserName = UserLoginSingleton.GetUser().Credential,
                    DateTime = DateTime.Now
                };
                logDAO.Insert(log);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
