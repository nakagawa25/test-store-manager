using System;
using System.Collections.Generic;
using System.Linq;
using T4LSystemBackEnd.Utils;
using T4LSystemLibrary.DAO;
using T4LSystemLibrary.VO;

namespace T4LSystemBackEnd.BusinessLayer
{
    public class SaleController
    {
        public SaleController()
        {
            ProductWithAmountList = new List<ProductWithAmount>();
        }
        #region Lista do Produto Segura para Threads
        private List<ProductWithAmount> ProductWithAmountList { get; set; }
        private static object syncObject = new object();
        private bool NeedInsertAmountFlag { get; set; } = false;

        public void AddProductWithAmountList(ProductWithAmount productAndAmount)
        {
            lock (syncObject)
            {
                ProductWithAmountList.Add(productAndAmount);
            }
        }
        public void RemoveProductWithAmountList(ProductWithAmount productAndAmount)
        {
            lock (syncObject)
            {
                ProductWithAmountList.Remove(productAndAmount);
            }
        }
        public ProductWithAmount SelectLastProductWithAmount()
        {
            return ProductWithAmountList.Last();
        }
        public List<ProductWithAmount> SelectProductWithAmountList()
        {
            return ProductWithAmountList;
        }
        #endregion

        public double TakeTotalSaleValue()
        {
            double value = 0;
            foreach (ProductWithAmount productWithAmount in ProductWithAmountList)
                value += productWithAmount.Total;
            return value;
        }
        private VendaVO TakeSaleObject(string clientName, string clientDocument, string observation = null)
        {
            VendaVO sale = new VendaVO
            {
                ClienteDocumento = clientDocument,
                ClienteNome = clientName,
                DataHora = DateTime.Now,
                Total = TakeTotalSaleValue(),
                Obs = observation
            };
            return sale;
        }
        private VendaProdutoVO TakeSaleProductObject(ProdutoVO product, double amount, int saleCode)
        {
            VendaProdutoVO saleProduct = new VendaProdutoVO
            {
                CodVenda = saleCode,
                CodProduto = product.Cod,
                PrecoVenda = product.PrecoVenda,
                Quantidade = amount
            };
            return saleProduct;
        }
        public void UpdateProductAmountInTheList(double amount)
        {
            try
            {
                ProductWithAmountList.Last().Amount = amount;
                ProductWithAmountList.Last().Total = ProductWithAmountList.Last().Amount * ProductWithAmountList.Last().Product.PrecoVenda;
                NeedInsertAmountFlag = false;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao atualizar quantidade e/ou valor total na lista de produtos da venda. Erro: " + error.Message);
            }
        }
        public bool InsertSale(string clientName, string clientDocument, string observation = null)
        {
            try
            {
                if (ProductWithAmountList.Count <= 0)
                    return false;
                ProdutoDAO productDAO = new ProdutoDAO();
                VendaDAO saleDAO = new VendaDAO();
                VendaProdutoDAO saleProductDAO = new VendaProdutoDAO();
                saleDAO.Insert(TakeSaleObject(clientName, clientDocument, observation));
                int saleCode = saleDAO.SelectLastSaleCode();
                foreach (ProductWithAmount productAmount in ProductWithAmountList)
                    saleProductDAO.Insert(TakeSaleProductObject(productAmount.Product, productAmount.Amount, saleCode));

                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao inserir uma venda. Erro: " + error.Message);
                return false;
            }
        }
        public SelectProductByCodeStatus InsertProductOnSaleList(int code)
        {
            try
            {
                if (NeedInsertAmountFlag)
                    return SelectProductByCodeStatus.NeedInsertAmount;
                ProdutoDAO productDAO = new ProdutoDAO();
                ProdutoVO product = productDAO.SelectProductByCode(code);
                if (product == null)
                    return SelectProductByCodeStatus.NotFound;
                else if (product.Ativo == 0)
                    return SelectProductByCodeStatus.ProductDeactivated;
                else
                {
                    ProductWithAmount productAmount = new ProductWithAmount();
                    productAmount.Product = product;
                    AddProductWithAmountList(productAmount);
                    NeedInsertAmountFlag = true;
                    return SelectProductByCodeStatus.Success;
                }
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao inserir um produto na lista de venda. Erro: " + error.Message);
                return SelectProductByCodeStatus.OperationError;
            }
        }
    }
}
