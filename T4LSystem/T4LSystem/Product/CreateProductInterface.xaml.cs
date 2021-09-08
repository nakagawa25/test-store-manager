using System;
using System.Windows;
using T4LSystemLibrary.VO;
using T4LSystemBackEnd.BusinessLayer;
using System.Windows.Controls;

namespace T4LSystem.Product
{
    /// <summary>
    /// Lógica interna para CreateProdcutInterface.xaml
    /// </summary>
    public partial class CreateProdcutInterface : Window
    {
        private static bool IsUpdateOperation { get; set; } = false;
        private ProdutoVO ProductToUpdate { get; set; }
        public CreateProdcutInterface(ProdutoVO product = null, bool? isInsertOperation = null)
        {
            InitializeComponent();
            LoadProductGroupComboBox();
            LoadMeasuresComboBox();
            IsUpdateOperation = isInsertOperation == true ? false : true;
            LoadProductFields(product);
        }
        private int TakeComboBoxIndex(StandardVO standarVO, ComboBox comboBox)
        {

            try
            {
                int i = 0;
                foreach (object item in comboBox.Items)
                {
                    if ((item as StandardVO).Cod == standarVO.Cod)
                        return i;
                    i++;
                }
                return -1;
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro no combobox. Erro: " + error.Message);
                return -1;
            }
        }
        private void LoadProductFields(ProdutoVO product)
        {
            try
            {
                if (product == null)
                    return;
                IsUpdateOperation = true;
                ProductToUpdate = product;
                txtProductDescription.Text = ProductToUpdate.Descricao;
                txtProductBarCode.Text = ProductToUpdate.CodBarra;
                txtProductPriceValue.Text = ProductToUpdate.PrecoCusto.ToString();
                txtProductMarketPriceValue.Text = ProductToUpdate.PrecoVenda.ToString();
                chbProductActive.IsChecked = ProductToUpdate.Ativo == 1;
                cbProductGroup.SelectedIndex = TakeComboBoxIndex(ProductGroupController.Select(ProductToUpdate.CodGrupo), cbProductGroup);
                cmbProductUnity.SelectedIndex = TakeComboBoxIndex(MeasureController.Select(ProductToUpdate.CodMedida), cmbProductUnity);
                btnInsert.Content = "Atualizar Produto";
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao preencher os campos do produto. Erro: " + error.Message);
            }
        }
        private void LoadMeasuresComboBox()
        {
            try
            {
                cmbProductUnity.ItemsSource = MeasureController.SelectAllMeasures();
                cmbProductUnity.DisplayMemberPath = "UnidadeMedida";
                cmbProductUnity.SelectedValue = "Cod";
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao carregar as unidades de medida. Erro: " + error.Message);
            }
        }
        private void LoadProductGroupComboBox()
        {
            try
            {
                cbProductGroup.ItemsSource = ProductGroupController.SelectAllProductGroups();
                cbProductGroup.DisplayMemberPath = "Nome";
                cbProductGroup.SelectedValuePath = "Cod";
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao carregar os grupos dos produtos. Erro: " + error.Message);
            }
        }
        private long TakeMeasureUnityCode()
        {
            try
            {
                return Convert.ToInt64((cmbProductUnity.SelectedItem as MedidaVO).Cod);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private int TakeProductGroupCode()
        {
            try
            {
                return Convert.ToInt32((cbProductGroup.SelectedItem as ProdutoGrupoVO).Cod);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string productDescription = txtProductDescription.Text.Trim();
                string productBarCode = txtProductBarCode.Text.Trim();
                string productCostPrice = txtProductPriceValue.Text.Trim();
                string productMarketPrice = txtProductMarketPriceValue.Text.Trim();
                int productGroupCode = TakeProductGroupCode();
                long measureCode = TakeMeasureUnityCode();
                if (productGroupCode <= 0 || measureCode <= 0)
                {
                    MessageBox.Show("O Grupo de Produto e a Unidade de Medida devem ser definidos.");
                    return;
                }
                int productActive = (chbProductActive.IsChecked == true) ? 1 : 0;
                ProdutoVO product;
                if (IsUpdateOperation)
                {
                    if (ProductController.ValidateProductInputs(productDescription, productGroupCode, measureCode, productBarCode, productCostPrice, productMarketPrice, productActive, out product, ProductToUpdate.Cod))
                    {
                        string message = ProductController.UpdateProduct(product) ? "Produto Atualizado com sucesso!" : "ERRO!!! Não foi possível atualizar o produto, verifique os logs para mais informações.";
                        MessageBox.Show(message);
                    }
                    else
                        MessageBox.Show("Os campos inseridos não estão corretos.");
                    IsUpdateOperation = false;
                    this.Close();
                }
                else
                {
                    if (ProductController.ValidateProductInputs(productDescription, productGroupCode, measureCode, productBarCode, productCostPrice, productMarketPrice, productActive, out product))
                    {
                        string message = ProductController.InsertProduct(product) ? "Produto Cadastrado com sucesso!" : "ERRO!!! Não foi possível cadastrar o produto, verifique os logs para mais informações.";
                        MessageBox.Show(message);
                    }
                    else
                        MessageBox.Show("Os campos inseridos não estão corretos.");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao cadastrar novo produto. Erro: " + error.Message);
            }
        }
    }
}
