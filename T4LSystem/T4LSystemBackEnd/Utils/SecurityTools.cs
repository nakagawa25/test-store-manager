using System;
using System.Security.Cryptography;
using System.Text;

namespace T4LSystemBackEnd.Utils
{
    internal class SecurityTools
    {
        internal static string GetPasswordHash(string clearPassword)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] passwordSecurityOperation = md5Hasher.ComputeHash(Encoding.Default.GetBytes(clearPassword));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < passwordSecurityOperation.Length; i++)
                stringBuilder.Append(passwordSecurityOperation[i].ToString("x2"));
            return stringBuilder.ToString();
        }
        internal static bool VerifyPasswordHash(string clearPassword, string hashedPassword)
        {
            string hashOfInput = GetPasswordHash(clearPassword);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hashedPassword)) ? true : false;
        }
    }
}
