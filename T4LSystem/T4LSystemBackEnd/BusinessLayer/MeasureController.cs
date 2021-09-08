using System;
using System.Collections.Generic;
using System.Linq;
using T4LSystemLibrary.DAO;
using T4LSystemLibrary.VO;

namespace T4LSystemBackEnd.BusinessLayer
{
    public class MeasureController
    {
        public static MedidaVO Select(long code)
        {
            try
            {
                return SelectAllMeasures().Find(um => um.Cod == code);
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao consultar uma unidade de medida. Erro: " + error.Message);
                return null;
            }
        }

        public static List<MedidaVO> SelectAllMeasures()
        {
            try
            {
                MedidaDAO measureDAO = new MedidaDAO();
                return measureDAO.Select().Cast<MedidaVO>().ToList();
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao consultar todas as unidades de medida dos produtos. Erro: " + error.Message);
                return null;
            }
        }
    }
}
