using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Steganography
{
    public class AESEncrypt
    {

        static AesCryptoServiceProvider CreateAES(string key)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(key);
            AesCryptoServiceProvider a = new AesCryptoServiceProvider();
            Rfc2898DeriveBytes r = new Rfc2898DeriveBytes(key, new byte[] { 1,2,3,4,5,6,7,8});
            a.Key = r.GetBytes(a.BlockSize / 8);
            a.IV = new byte[a.BlockSize / 8];
            a.Padding = PaddingMode.PKCS7;
            a.Mode = CipherMode.CBC;
            return a;
        }

        public string EncryptString(string text, string password)
        {
            try {
                byte[] textBytes = Encoding.Unicode.GetBytes(text);
                MemoryStream stream = new MemoryStream();
                AesCryptoServiceProvider aes = CreateAES(password);
                CryptoStream crypt = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                crypt.Write(textBytes, 0, textBytes.Length);
                crypt.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch
            {

            }
            return null;
        }

        public string DecryptString(string text, string password)
        {
            try {
                byte[] textBytes = Convert.FromBase64String(text);
                MemoryStream stream = new MemoryStream();
                AesCryptoServiceProvider aes = CreateAES(password);
                CryptoStream crypt = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                crypt.Write(textBytes, 0, textBytes.Length);
                crypt.FlushFinalBlock();
                return Encoding.Unicode.GetString(stream.ToArray());
            }
            catch
            {

            }
            return null;
        }
    }
}
