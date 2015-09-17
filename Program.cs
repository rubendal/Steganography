using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Steganography
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //test
            string t = "test";
            AESEncrypt encrypt = new AESEncrypt();
            string encrypted = encrypt.EncryptString(t, "a password");
            Console.WriteLine(encrypted);
            Console.WriteLine(encrypt.DecryptString(encrypted,"a password"));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
