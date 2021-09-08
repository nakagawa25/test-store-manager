using T4LSystemLibrary.VO;

namespace T4LSystemLibrary.Singleton
{
    /// <summary>
    /// This is a Singleton Class Like, the difference is: This class only receives some data once, it´s like a instance.
    /// </summary>
    public sealed class UserLoginSingleton
    {
        public UserLoginSingleton()
        {

        }
        private static UserLoginVO userLoginInstance = null;
        public static UserLoginVO GetUser()
        {
            if (userLoginInstance == null)
            {
                UserLoginVO auxiliaryUser = new UserLoginVO
                {
                    Credential = "Usuário Auxiliar",
                    Password = "Sem Senha"
                };
                userLoginInstance = auxiliaryUser;
            }
            return userLoginInstance;
        }
        public static void SetUser(UserLoginVO userLogin)
        {
            if (userLoginInstance == null)
                userLoginInstance = userLogin;
        }
    }
}
