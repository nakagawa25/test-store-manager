using System;
using System.Collections.Generic;
using System.Linq;
using T4LSystemLibrary.DAO;
using T4LSystemLibrary.VO;

namespace T4LSystemBackEnd.BusinessLayer
{
    public class ProductGroupController
    {
        public static ProdutoGrupoVO Select(int code)
        {
            try
            {
                return SelectAllProductGroups().Find(pg => pg.Cod == code);
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao consultar um grupo de produto. Erro: " + error.Message);
                return null;
            }
        }
        public static List<ProdutoGrupoVO> SelectAllProductGroups()
        {
            try
            {
                ProdutoGrupoDAO productGroupDAO = new ProdutoGrupoDAO();
                return productGroupDAO.Select().Cast<ProdutoGrupoVO>().ToList();
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao consultar todos os grupos de produtos. Erro: " + error.Message);
                return null;
            }
        }
    }
}
