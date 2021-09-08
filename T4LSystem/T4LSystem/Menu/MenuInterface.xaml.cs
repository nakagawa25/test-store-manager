using System;
using System.Windows;

namespace T4LSystem.Menu
{
    /// <summary>
    /// Interaction logic for MenuInterface.xaml
    /// </summary>
    public partial class MenuInterface : Window
    {
        public MenuInterface()
        {
            InitializeComponent();
        }
        private void BtnShowProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product.ViewProductInterface productInterface = new Product.ViewProductInterface();
                productInterface.ShowDialog();
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao exibir tela de produto. Erro: " + error.Message);
            }
        }
        private void BtnShowSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sale.SaleOperationInterface saleInterface = new Sale.SaleOperationInterface();
                saleInterface.ShowDialog();
                saleInterface = null;
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao exibir tela de venda. Erro: " + error.Message);
            }
        }
    }
}
