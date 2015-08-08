using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Steganography
{
    public class DESEncrypt
    {
        static TripleDES CreateDES(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }

        public string EncryptString(string text, string password)
        {
            byte[] textBytes = Encoding.Unicode.GetBytes(text);
            MemoryStream stream = new MemoryStream();
            TripleDES des = CreateDES(password);
            CryptoStream crypt = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write);
            crypt.Write(textBytes, 0, textBytes.Length);
            crypt.FlushFinalBlock();
            return Convert.ToBase64String(stream.ToArray());
        }

        public string DecryptString(string text, string password)
        {
            byte[] textBytes = Convert.FromBase64String(text);
            MemoryStream stream = new MemoryStream();
            TripleDES des = CreateDES(password);
            CryptoStream crypt = new CryptoStream(stream, des.CreateDecryptor(), CryptoStreamMode.Write);
            crypt.Write(textBytes, 0, textBytes.Length);
            crypt.FlushFinalBlock();
            return Encoding.Unicode.GetString(stream.ToArray());
        }
    }
}
