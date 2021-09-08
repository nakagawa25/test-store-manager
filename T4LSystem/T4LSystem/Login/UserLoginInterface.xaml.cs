using System;
using System.Threading.Tasks;
using System.Windows;
using T4LSystemBackEnd.BusinessLayer;
using T4LSystemBackEnd.Utils;
using T4LSystemLibrary.VO;

namespace T4LSystem.Login
{
    /// <summary>
    /// Interaction logic for UserLoginInterface.xaml
    /// </summary>
    public partial class UserLoginInterface : Window
    {
        public UserLoginInterface()
        {
            InitializeComponent();
        }
        private void SystemInitializate(UserLoginVO user)
        {
            Menu.MenuInterface menuInterface = new Menu.MenuInterface();
            menuInterface.ShowDialog();
        }
        private void Login()
        {
            try
            {
                string userCredential = txtUserCredential.Text.Trim();
                string userPassword = txtUserPassword.Password;
                if (FieldsValidator.FieldValidate(userCredential) && userPassword.Length > 0)
                {
                    if (LoginController.VerifyUserCredentialExistance(userCredential))
                    {
                        if (LoginController.UserAuthenticate(userCredential, userPassword, out UserLoginVO user))
                            SystemInitializate(user);
                        else
                            MessageBox.Show("Senha Incorreta.");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Esse usuário não existe, clique no botão Criar Usuário e crie um novo usuário para utilizar o Sistema.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Digite os campos de usuário e senha corretamente.");
                    return;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro no sistema de login. Erro: " + error.Message);
                return;
            }
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        private void BtnCreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userCredential = txtUserCredential.Text.Trim();
                string userPassword = txtUserPassword.Password;
                if (FieldsValidator.FieldValidate(userCredential) && userPassword.Length > 0)
                {
                    if (LoginController.VerifyUserCredentialExistance(userCredential))
                    {
                        MessageBox.Show("Esse usuário já está em uso.");
                        return;
                    }
                    else
                    {
                        UserLoginVO user = new UserLoginVO
                        {
                            Credential = userCredential,
                            Password = userPassword
                        };
                        string message = LoginController.InsertUser(user) ? "Sucesso!" : "Não foi possível cadastrar o novo usuário, tente novamente mais tarde.";
                        MessageBox.Show(message);
                        Login();
                    }
                }
                else
                {
                    MessageBox.Show("Digite os campos de usuário e senha corretamente.");
                    return;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro no sistema de login. Erro: " + error.Message);
                return;
            }
        }

        private void TxbClose_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
