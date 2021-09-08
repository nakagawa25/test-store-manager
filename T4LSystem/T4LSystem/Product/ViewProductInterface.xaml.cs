using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using T4LSystemBackEnd.BusinessLayer;
using T4LSystemLibrary.VO;

namespace T4LSystem.Product
{
    /// <summary>
    /// Interaction logic for ViewProductInterface.xaml
    /// </summary>
    public partial class ViewProductInterface : Window
    {
        public ViewProductInterface()
        {
            InitializeComponent();
            FillDataGrid();
        }
        private void RefreshScreen()
        {
            FillDataGrid();
        }
        private void FillDataGrid()
        {
            try
            {
                dgdProductsList.ItemsSource = null;
                List<ProdutoXProdutoGrupo> productsList = ProductController.SelectAllProductsWithGroupName();
                dgdProductsList.ItemsSource = productsList;
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao preencher tabela com os produtos. \nErro: " + error.Message);
                return;
            }
        }
        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgdProductsList.SelectedItems.Count <= 0)
                {
                    MessageBox.Show("Selecione ao menos 1 item para exclusão.");
                    return;
                }
                if (VerifyDeleteAnswer())
                {
                    List<ProdutoXProdutoGrupo> selectedProductsList = dgdProductsList.SelectedItems.Cast<ProdutoXProdutoGrupo>().ToList();
                    string message = ProductController.DeleteProducts(ProductController.ConvertProductXGroupToProduct(selectedProductsList), out bool? blockedBySellProduct) ? "Sucesso!" : "Erro ao Excluir! Confira os logs.";
                    if (blockedBySellProduct == true)
                        message += "\nNão há problemas no sistema de exclusão, mas um produto que está cadastrado em uma venda não pode ser excluído!";
                    MessageBox.Show(message);
                    RefreshScreen();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao Excluir o Produto. \nErro: " + error.Message);
                return;
            }
        }
        private bool VerifyDeleteAnswer()
        {
            return (MessageBox.Show("Tem certeza que deseja excluir o(s) produto(s)?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes) ? true : false;
        }
        private void BtnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgdProductsList.SelectedItems.Count <= 0 || dgdProductsList.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Selecione somente 1 item para atualização do produto.");
                    return;
                }
                List<ProdutoXProdutoGrupo> selectedProductsList = dgdProductsList.SelectedItems.Cast<ProdutoXProdutoGrupo>().ToList();
                ProdutoVO productToUpdate = ProductController.ConvertProductXGroupToProduct(selectedProductsList).FirstOrDefault();
                CreateProdcutInterface updateInterface = new CreateProdcutInterface(productToUpdate);
                updateInterface.ShowDialog();
                RefreshScreen();
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao abrir tela de atualização. Erro: " + error.Message);
                return;
            }
        }
        private void BtnInsertProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateProdcutInterface insertInterface = new CreateProdcutInterface(null, true);
                insertInterface.ShowDialog();
                RefreshScreen();
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao exibir a tela de cadastro. Erro: " + error.Message);
                return;
            }
        }
    }
}
