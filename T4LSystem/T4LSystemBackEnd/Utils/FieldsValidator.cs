using System;
using T4LSystemBackEnd.BusinessLayer;

namespace T4LSystemBackEnd.Utils
{
    public class FieldsValidator
    {
        public static bool FieldValidate(string inputString, out double outputValue)
        {
            try
            {
                outputValue = Convert.ToDouble(inputString);
                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao validar campo double. Erro: " + error.Message);
                outputValue = 0;
                return false;
            }
        }
        public static bool FieldValidate(string inputString, out long outputValue)
        {
            try
            {
                outputValue = Convert.ToInt64(inputString);
                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao validar campo long. Erro: " + error.Message);
                outputValue = 0;
                return false;
            }
        }
        public static bool FieldValidate(string inputString, out int outputValue)
        {
            try
            {
                outputValue = Convert.ToInt32(inputString);
                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao validar campo int. Erro: " + error.Message);
                outputValue = 0;
                return false;
            }
        }
        public static bool FieldValidate(string inputString, out DateTime? outputValue)
        {
            try
            {
                outputValue = Convert.ToDateTime(inputString);
                return true;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao validar campo dateTime. Erro: " + error.Message);
                outputValue = null;
                return false;
            }
        }
        /// <summary>
        /// Validation for string fields only, this method returns true if the field is not null or empty.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static bool FieldValidate(string inputString)
        {
            return (inputString.Trim() == null || inputString.Trim().Length == 0) ? false : true;
        }
    }
}
