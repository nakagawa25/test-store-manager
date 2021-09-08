using System;
using System.Collections.Generic;
using System.Linq;
using T4LSystemBackEnd.Utils;
using T4LSystemLibrary.DAO;
using T4LSystemLibrary.VO;

namespace T4LSystemBackEnd.BusinessLayer
{
    public class ProductController
    {
        public static bool DeleteProducts(List<ProdutoVO> productsList, out bool? deleteBlockedByProductSell)
        {
            try
            {
                deleteBlockedByProductSell = false;
                ProdutoDAO productDAO = new ProdutoDAO();
                foreach (ProdutoVO product in productsList)
                {
                    if (CheckProductInProductSell(product))
                        deleteBlockedByProductSell = true;
                    else
                        productDAO.Delete(product);
                }
                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao excluir um produto. Erro: " + error.Message);
                deleteBlockedByProductSell = null;
                return false;
            }
        }
        public static bool UpdateProduct(ProdutoVO product)
        {
            try
            {
                ProdutoDAO produtoDAO = new ProdutoDAO();
                produtoDAO.Update(product);
                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao atualizar um produto. Erro: " + error.Message);
                return false;
            }
        }
        public static bool InsertProduct(ProdutoVO product)
        {
            try
            {
                ProdutoDAO produtoDAO = new ProdutoDAO();
                produtoDAO.Insert(product);
                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao inserir um produto. Erro: " + error.Message);
                return false;
            }
        }
        private static bool CheckProductInProductSell(ProdutoVO product)
        {
            VendaProdutoDAO productSellDAO = new VendaProdutoDAO();
            List<StandardVO> productsCode = productSellDAO.SelectSellProductByProductCode(product);
            return (productsCode.Count > 0) ? true : false;
        }
        public static bool ValidateProductInputs(string productDescription, int productGroupCode, long measureCode, string productBarCode, string productCostPrice, string productMarketPrice, int productActive, out ProdutoVO produto, long? productCode = null)
        {
            try
            {
                double productCostPriceValue, productMarketPriceValue;
                if (!FieldsValidator.FieldValidate(productDescription) || !FieldsValidator.FieldValidate(productCostPrice, out productCostPriceValue) ||
                    !FieldsValidator.FieldValidate(productMarketPrice, out productMarketPriceValue) || productGroupCode <= 0 || measureCode <= 0)
                {
                    produto = null;
                    return false;
                }
                ProdutoVO productAuxiliary = new ProdutoVO
                {
                    Descricao = productDescription,
                    CodGrupo = productGroupCode,
                    CodMedida = measureCode,
                    CodBarra = productBarCode,
                    PrecoCusto = productCostPriceValue,
                    PrecoVenda = productMarketPriceValue,
                    Ativo = productActive,
                    DataHoraCadastro = DateTime.Now
                };
                if (productCode != null && productCode > 0)
                    productAuxiliary.Cod = Convert.ToInt64(productCode);
                produto = productAuxiliary;
                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao validar os campos do front-end para inserir produtos. Erro: " + error.Message);
                produto = null;
                return false;
            }
        }
        public static List<ProdutoVO> SelectAllProducts()
        {
            try
            {
                ProdutoDAO productDAO = new ProdutoDAO();
                return productDAO.Select().Cast<ProdutoVO>().ToList();
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao consultar todos os produtos. Erro: " + error.Message);
                return null;
            }
        }
        public static ProdutoVO SelectProductByCode(int code)
        {
            try
            {
                ProdutoDAO productDAO = new ProdutoDAO();
                return productDAO.SelectProductByCode(code);
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao consultar um produto pelo código. Erro: " + error.Message);
                return null;
            }
        }
        public static List<ProdutoXProdutoGrupo> SelectAllProductsWithGroupName()
        {
            try
            {
                return ConvertProductToProductXGroup(SelectAllProducts());
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao retornar lista ProdutoXProdutoGrupo. Erro: " + error.Message);
                return null;
            }
        }
        public static List<ProdutoVO> ConvertProductXGroupToProduct(List<ProdutoXProdutoGrupo> inputList)
        {
            try
            {
                List<ProdutoVO> outputList = new List<ProdutoVO>();
                List<ProdutoGrupoVO> productsGroupList = ProductGroupController.SelectAllProductGroups();
                List<MedidaVO> measuresList = MeasureController.SelectAllMeasures();
                foreach (ProdutoXProdutoGrupo productXproductGroup in inputList)
                {
                    ProdutoVO product = new ProdutoVO
                    {
                        Cod = productXproductGroup.Cod,
                        Descricao = productXproductGroup.Descricao,
                        CodGrupo = Convert.ToInt32(productsGroupList.Find(pg => pg.Nome == productXproductGroup.NomeGrupo).Cod),
                        CodBarra = productXproductGroup.CodBarra,
                        PrecoCusto = productXproductGroup.PrecoCusto,
                        PrecoVenda = productXproductGroup.PrecoVenda,
                        Ativo = productXproductGroup.Ativo,
                        CodMedida = Convert.ToInt64(measuresList.Find(mu => mu.UnidadeMedida == productXproductGroup.UnidadeMedida).Cod)
                    };
                    outputList.Add(product);
                }
                return outputList;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao converter um objeto produtoXgrupo em um objeto produto. Erro: " + error.Message);
                return null;
            }
        }
        private static List<ProdutoXProdutoGrupo> ConvertProductToProductXGroup(List<ProdutoVO> inputList)
        {
            try
            {
                List<ProdutoXProdutoGrupo> outputList = new List<ProdutoXProdutoGrupo>();
                List<ProdutoGrupoVO> productsGroupList = ProductGroupController.SelectAllProductGroups();
                List<MedidaVO> measuresList = MeasureController.SelectAllMeasures();
                foreach (ProdutoVO product in inputList)
                {
                    ProdutoXProdutoGrupo produtoXProdutoGrupo = new ProdutoXProdutoGrupo
                    {
                        Cod = product.Cod,
                        Descricao = product.Descricao,
                        CodBarra = product.CodBarra,
                        NomeGrupo = productsGroupList.Find(pg => pg.Cod == product.CodGrupo).Nome,
                        PrecoCusto = product.PrecoCusto,
                        PrecoVenda = product.PrecoVenda,
                        Ativo = product.Ativo,
                        UnidadeMedida = measuresList.Find(mu => mu.Cod == product.CodMedida).UnidadeMedida
                    };
                    outputList.Add(produtoXProdutoGrupo);
                }
                return outputList;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao converter um objeto produto em um objeto produtoXgrupo. Erro: " + error.Message);
                return null;
            }
        }
    }
}
