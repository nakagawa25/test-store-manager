using System;
using T4LSystemBackEnd.BusinessLayer;

namespace T4LSystemBackEnd.Utils
{
    public static class CPFValidator
    {
        private static readonly int[] ArrayCalculator = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        public static bool? VerifyCPF(string cpf)
        {
            try
            {
                if (cpf.Trim().Length < 11 || string.IsNullOrEmpty(cpf))
                    return null;
                int[] cpfArray = ConvertCPFToIntArray(cpf);
                int[] cpfChecker = BuildCPFWithDigits(cpfArray);
                return (cpfArray[9] == cpfChecker[9] && cpfArray[10] == cpfChecker[10]) ? true : false;
            }
            catch (Exception error)
            {
                LogController.WriteLog("Erro ao Verificar o CPF. Erro: " + error.Message);
                return null;
            }
        }
        private static int[] BuildCPFWithDigits(int[] cpf)
        {
            int[] auxiliaryArray = new int[11];
            for (int i = 0; i < cpf.Length; i++)
                auxiliaryArray[i] = cpf[i];
            int? firstDigit = CalculateDigit(auxiliaryArray, true);
            if (firstDigit == null)
                return null;
            else
                auxiliaryArray[9] = Convert.ToInt32(firstDigit);
            int? secondDigit = CalculateDigit(auxiliaryArray, false);
            if (secondDigit == null)
                return null;
            else
                auxiliaryArray[10] = Convert.ToInt32(secondDigit);
            return auxiliaryArray;
        }
        private static int? CalculateDigit(int[] cpf, bool isFirstDigit)
        {
            if (cpf == null)
                return null;
            int summation = 0;
            int arrayCalculatorCounter = isFirstDigit ? 1 : 0;
            int finalIndex = isFirstDigit ? 9 : 10;

            for (int i = 0; i < finalIndex; i++)
            {
                summation += ArrayCalculator[arrayCalculatorCounter] * cpf[i];
                arrayCalculatorCounter++;
            }
            int module = summation % 11;
            if (module < 2)
                return 0;
            else
                return 11 - module;
        }
        private static int[] ConvertCPFToIntArray(string cpf)
        {
            int[] cpfIntArray = new int[11];
            int i = 0;
            foreach (char number in cpf)
            {
                if (number != '.' && number != '-' && number != ',' && number != ' ')
                {
                    cpfIntArray[i] = Convert.ToInt32(number.ToString());
                    i++;
                }
            }
            return cpfIntArray;
        }
    }
}
