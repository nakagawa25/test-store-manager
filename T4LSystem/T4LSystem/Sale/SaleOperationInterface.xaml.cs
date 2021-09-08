using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using T4LSystemBackEnd.BusinessLayer;
using T4LSystemBackEnd.Utils;
using T4LSystemLibrary.VO;

namespace T4LSystem.Sale
{
    /// <summary>
    /// Interaction logic for SaleOperationInterface.xaml
    /// </summary>
    public partial class SaleOperationInterface : Window
    {
        private static bool InsertClientData { get; set; } = false;
        private SaleController SaleControllerObject { get; set; }
        public SaleOperationInterface()
        {
            InitializeComponent();
            StartSale();
        }
        private void StartSale()
        {
            ClearAllFields();
            btnInsertSaleProduct.IsEnabled = false;
            InsertClientData = VerifyInsertClientData();
            AllowClientNameAndDocumentInsert();
            SaleControllerObject = new SaleController();
            RefreshList();
        }
        private void ClearAllFields()
        {
            SaleControllerObject = null;
            dgdSaleList.ItemsSource = null;
            txtClientName.Clear();
            txtClientDocument.Clear();
            txtProductAmount.Clear();
            txtProductCode.Clear();
            txtObservation.Clear();
            lblTotalValue.Content = 0;
        }
        private bool VerifyInsertClientData()
        {
            return (MessageBox.Show("Deseja inserir documento e/ou nome do cliente?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes) ? true : false;
        }
        private void AllowClientNameAndDocumentInsert()
        {
            txtClientName.IsEnabled = InsertClientData;
            txtClientDocument.IsEnabled = InsertClientData;
            lblClientName.IsEnabled = InsertClientData;
            lblClientDocument.IsEnabled = InsertClientData;
            if (InsertClientData)
                txtClientName.Focus();
            else
                txtProductCode.Focus();
        }
        private void OnKeyDownHandlerProduct(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (AddProduct())
                    txtProductAmount.Focus();
            }
        }
        private void OnKeyDownHandlerAmount(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (AddAmount())
                {
                    txtProductAmount.Clear();
                    txtProductCode.Clear();
                    txtProductCode.Focus();
                }
            }
        }
        private List<int> GetListIndex(List<ProductWithAmount> productWithAmountList)
        {
            if (productWithAmountList == null)
                return null;
            List<int> returnedList = new List<int>();
            for (int i = 0; i < productWithAmountList.Count; i++)
                returnedList.Add(i + 1);
            return returnedList;
        }
        private void RefreshList()
        {
            try
            {
                dgdSaleList.ItemsSource = null;
                List<ProductWithAmount> productWithAmountList = SaleControllerObject.SelectProductWithAmountList();
                dgdSaleList.ItemsSource = productWithAmountList;
                lblTotalValue.Content = SaleControllerObject.TakeTotalSaleValue().ToString("N");
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao recarregar a lista de produtos da venda. Erro: " + error.Message);
            }
        }
        private bool VerifyTxtProductAmountDecimalNumber()
        {
            return txtProductAmount.Text.Contains(".") || txtProductAmount.Text.Contains(",") ? true : false;
        }
        private bool CheckProductUnityMeasureInt()
        {
            ProductWithAmount productWithAmount = SaleControllerObject.SelectLastProductWithAmount();
            return productWithAmount.Product.CodMedida == 1 ? true : false;
        }
        private bool AddAmount()
        {
            try
            {
                if (CheckProductUnityMeasureInt() && VerifyTxtProductAmountDecimalNumber())
                {
                    MessageBox.Show("O produto selecionado é vendido por unidade, favor inserir um número inteiro no campo quantidade.");
                    return false;
                }
                double amount = Convert.ToDouble(txtProductAmount.Text.Replace('.', ','));
                if (amount <= 0)
                {
                    MessageBox.Show("A quantidade deve ser maior que 0.");
                    return false;
                }
                else
                {
                    SaleControllerObject.UpdateProductAmountInTheList(amount);
                    RefreshList();
                    btnInsertSaleProduct.IsEnabled = true;
                    return true;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao inserir a quantidade. Erro: " + error.Message);
                return false;
            }
        }
        private bool AddProduct()
        {
            try
            {
                int code = Convert.ToInt32(txtProductCode.Text);
                SelectProductByCodeStatus operationStatus = SaleControllerObject.InsertProductOnSaleList(code);
                switch (operationStatus)
                {
                    case SelectProductByCodeStatus.Success:
                        MessageBox.Show("Produto adicionado na lista da venda, digite a quantidade desse produto na venda.");
                        return true;
                    case SelectProductByCodeStatus.NotFound:
                        MessageBox.Show("O produto não foi encontrado, digite outro código.");
                        return false;
                    case SelectProductByCodeStatus.ProductDeactivated:
                        MessageBox.Show("Esse produto está desativado");
                        return false;
                    case SelectProductByCodeStatus.OperationError:
                        MessageBox.Show("Erro na operação. Consulte os Logs");
                        return false;
                    case SelectProductByCodeStatus.NeedInsertAmount:
                        MessageBox.Show("Digite a quantidade do produto antes de inserir um novo produto na venda.");
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Erro ao adicionar produto na venda. Erro: " + error.Message);
                return false;
            }
        }
        private void BtnInserSaleProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string clientDocument = InsertClientData ? txtClientDocument.Text.Trim() : string.Empty;
                string message = string.Empty;
                if (InsertClientData && CPFValidator.VerifyCPF(clientDocument) != true)
                    message = "O CPF digitado está incorreto, favor verificar e tentar novamente.";
                else
                    message = SaleControllerObject.InsertSale(txtClientName.Text.Trim(), clientDocument, txtObservation.Text.Trim()) ? "Sucesso!" : "Não foi possível inserir a venda. confira os logs";     
                MessageBox.Show(message);
                StartSale();
            }
            catch (Exception error)
            {
                MessageBox.Show("Houve um erro ao inserir a venda. Erro: " + error.Message);
            }
        }
        private void DgdSaleList_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
