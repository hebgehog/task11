using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace task11UP
{
    class Program
    {
        static void Main(string[] args)
        {
            String ishodnic = Shifr("Учебная практика!", "ключ");
            String ishodnic2 = DeShifr(ishodnic, "ключ");
            Console.WriteLine("Шифрованный текст: "+ishodnic);
            Console.WriteLine("Текст после дешифровки: "+ishodnic2);
            Console.ReadKey();
        }
        public static string Shifr(string ishodnic, string key,//метод шифрования строки
               string prog = "programming", string cryptographic = "SHA1",
               int keyI = 2, string vec = "a6doPuTitLz1hYe#",
               int keySize = 256)
        {
            if (string.IsNullOrEmpty(ishodnic))
                return "";
            byte[] vic1 = Encoding.ASCII.GetBytes(vec);
            byte[] prog1 = Encoding.ASCII.GetBytes(prog);
            byte[] ishodnic1 = Encoding.UTF8.GetBytes(ishodnic);
            PasswordDeriveBytes keyVAR = new PasswordDeriveBytes(key, prog1, cryptographic, keyI);
            byte[] keyBytes = keyVAR.GetBytes(keySize / 8);
            RijndaelManaged summafromk = new RijndaelManaged();
            summafromk.Mode = CipherMode.CBC;
            byte[] TextBytes = null;
            using (ICryptoTransform encryptor = summafromk.CreateEncryptor(keyBytes, vic1))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(ishodnic1, 0, ishodnic1.Length);
                        cryptoStream.FlushFinalBlock();
                        TextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }
            summafromk.Clear();
            return Convert.ToBase64String(TextBytes);
        }
        public static string DeShifr(string criptText, string key,//метод дешифрования
               string prog = "programming", string cryptographic = "SHA1",
               int keyI = 2, string vec = "a6doPuTitLz1hYe#",
               int keySize = 256)
        {
            if (string.IsNullOrEmpty(criptText))
                return "";
            byte[] vec1 = Encoding.ASCII.GetBytes(vec);
            byte[] prog1 = Encoding.ASCII.GetBytes(prog);
            byte[] criptText1 = Convert.FromBase64String(criptText);
            PasswordDeriveBytes keyVAR = new PasswordDeriveBytes(key, prog1, cryptographic, keyI);
            byte[] keyBytes = keyVAR.GetBytes(keySize / 8);
            RijndaelManaged summafromk = new RijndaelManaged();
            summafromk.Mode = CipherMode.CBC;
            byte[] plainTextBytes = new byte[criptText1.Length];
            int byteCount = 0;
            using (ICryptoTransform decryptor = summafromk.CreateDecryptor(keyBytes, vec1))
            {
                using (MemoryStream mSt = new MemoryStream(criptText1))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        mSt.Close();
                        cryptoStream.Close();
                    }
                }
            }
            summafromk.Clear();
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }
    }
}