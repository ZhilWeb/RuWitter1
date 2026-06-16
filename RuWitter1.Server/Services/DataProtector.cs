using System;
using System.Text;
using System.Security.Cryptography;


namespace RuWitter1.Server.Services
{
    public static class DataProtector
    {
        private static readonly byte[] OptionalEntropy = {  };

        // Зашифровать строку
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            // переводим строку в байты
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            // шифруем
            byte[] encryptedBytes = ProtectedData.Protect(plainBytes, OptionalEntropy, DataProtectionScope.CurrentUser);

            // конвертируем в строку и возвращаем ее
            return Convert.ToBase64String(encryptedBytes);

        }

        public static string Decrypt(string encryptedText) 
        {
            if (string.IsNullOrEmpty(encryptedText)) return encryptedText;

            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                byte[] plainBytes = ProtectedData.Unprotect(encryptedBytes, OptionalEntropy, DataProtectionScope.CurrentUser);

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (CryptographicException) 
            {
                throw new Exception("Не удалось расшифровать данные. Возможно, у вас нет прав или изменены ключи.");
            }
        }

    }
}
