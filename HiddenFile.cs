using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steganography
{
    public class HiddenFile
    {
        public string filename { get; set; }
        public byte[] file { get; set; }
        public int size { get; set; }

        public HiddenFile(byte[] file, string filename)
        {
            this.file = file;
            this.filename = filename;
            this.size = file.Length;
        }

        public void cipherFile(int seed)
        {
            byte[] newFile = CipherFile.cipherFile(file, seed);
            file = newFile;
        }
    }
}
