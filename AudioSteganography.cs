using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Steganography
{
    public static class AudioSteganography
    {
        public static byte[] EncryptText(byte[] wav, string text)
        {
            WavAudio audio = new WavAudio(wav);
            uint value = 0;
            string pass = string.Format(audio.bitsPerSample.ToString());
            AESEncrypt encrypt = new AESEncrypt();
            string encrypted = encrypt.EncryptString(text, pass);
            OutputConsole.Write(string.Format("Text encrypted \n{0}", encrypted));
            if (encrypted.Length <= Math.Floor((double)(audio.totalSamples / 8)))
            {
                SeedURNG generator = new SeedURNG(audio.totalSamples, audio.totalSamples);
                OutputConsole.Write("Seed generated");
                OutputConsole.Write("Processing wav file...");
                for (int i = 0; i < encrypted.Length; i++)
                {
                    value = encrypted[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.Next;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                    }

                }
                value = 0;
                for (int x = 0; x < 8; x++)
                {
                    uint sample = generator.Next;
                    uint sampleValue = audio.samples[sample];
                    sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                    audio.samples[sample] = sampleValue;
                }
                audio.Save();
                OutputConsole.Write(string.Format("Text encrypted... used {0} samples", encrypted.Length * 8));
                OutputConsole.Write("Saving wav file");
                return audio.data;
            }
            else
            {
                return null;
            }

        }
        public static byte[] EncryptTextLinear(byte[] wav, string text)
        {
            WavAudio audio = new WavAudio(wav);
            uint value = 0;
            string pass = string.Format(audio.bitsPerSample.ToString());
            AESEncrypt encrypt = new AESEncrypt();
            string encrypted = encrypt.EncryptString(text, pass);
            OutputConsole.Write(string.Format("Text encrypted \n{0}", encrypted));
            if (encrypted.Length <= Math.Floor((double)(audio.totalSamples / 8)))
            {
                uint n = 0;
                OutputConsole.Write("Seed generated");
                OutputConsole.Write("Processing wav file...");
                for (int i = 0; i < encrypted.Length; i++)
                {
                    value = encrypted[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = n;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                        n++;
                    }

                }
                value = 0;
                for (int x = 0; x < 8; x++)
                {
                    uint sample = n;
                    uint sampleValue = audio.samples[sample];
                    sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                    audio.samples[sample] = sampleValue;
                    n++;
                }
                audio.Save();
                OutputConsole.Write(string.Format("Text encrypted... used {0} samples", encrypted.Length * 8));
                OutputConsole.Write("Saving wav file");
                return audio.data;
            }
            else
            {
                return null;
            }

        }

        public static string DecryptText(byte[] wav)
        {
            WavAudio audio = new WavAudio(wav);
            string text = string.Empty;
            SeedURNG generator = new SeedURNG(audio.totalSamples, audio.totalSamples);
            uint value = 0;
            string pass = string.Format(audio.bitsPerSample.ToString());
            AESEncrypt encrypt = new AESEncrypt();
            OutputConsole.Write("Processing wav file...");
            do
            {
                value = 0;
                for (int x = 0; x < 8; x++)
                {
                    uint sample = generator.Next;
                    uint sampleValue = audio.samples[sample];
                    value = value | ((sampleValue & 1) << x);
                }
                if (value != 0)
                    text += Convert.ToChar(value);
            } while (value != 0);
            OutputConsole.Write("Decrypting text...");
            try
            {
                return encrypt.DecryptString(text, pass);
            }
            catch (Exception e)
            {
                OutputConsole.Write("Error: Text not found");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string DecryptTextLinear(byte[] wav)
        {
            WavAudio audio = new WavAudio(wav);
            string text = string.Empty;
            uint n = 0;
            uint value = 0;
            string pass = string.Format(audio.bitsPerSample.ToString());
            AESEncrypt encrypt = new AESEncrypt();
            OutputConsole.Write("Processing wav file...");
            do
            {
                value = 0;
                for (int x = 0; x < 8; x++)
                {
                    uint sample = n;
                    uint sampleValue = audio.samples[sample];
                    value = value | ((sampleValue & 1) << x);
                    n++;
                }
                if (value != 0)
                    text += Convert.ToChar(value);
            } while (value != 0);
            OutputConsole.Write("Decrypting text...");
            try
            {
                return encrypt.DecryptString(text, pass);
            }
            catch (Exception e)
            {
                OutputConsole.Write("Error: Text not found");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static byte[] EncryptFile(byte[] wav, byte[] file, string filename)
        {
            WavAudio audio = new WavAudio(wav);
            uint value = 0;
            int extraBytes = 2 + filename.Length + file.Length.ToString().Length;
            HiddenFile f = new HiddenFile(file, filename);
            OutputConsole.Write(string.Format("File size: {0} bytes", file.Length));
            f.cipherFile((int)audio.totalSamples);
            if (file.Length <= Math.Floor((double)(audio.totalSamples / 8)) - extraBytes)
            {
                SeedURNG generator = new SeedURNG(audio.totalSamples, audio.totalSamples);
                OutputConsole.Write("Seed generated");
                OutputConsole.Write("Ciphering file");
                OutputConsole.Write("Processing wav file...");
                OutputConsole.Write("Writing metadata...");
                //Write file size
                for (int i = 0; i < file.Length.ToString().Length; i++)
                {
                    value = file.Length.ToString()[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.Next;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                    }

                }
                value = '#';
                for (int x = 0; x < 8; x++)
                {
                    uint sample = generator.Next;
                    uint sampleValue = audio.samples[sample];
                    sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                    audio.samples[sample] = sampleValue;
                }
                //Write file name
                for (int i = 0; i < filename.Length; i++)
                {
                    value = filename[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.Next;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                    }

                }
                value = 0;
                for (int x = 0; x < 8; x++)
                {
                    uint sample = generator.Next;
                    uint sampleValue = audio.samples[sample];
                    sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                    audio.samples[sample] = sampleValue;
                }
                //Write file content
                OutputConsole.Write("Writing file data...");
                for (int i = 0; i < file.Length; i++)
                {
                    value = f.file[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.Next;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                    }

                }

            }
            else
            {
                OutputConsole.Write("Error");
                return null;
            }
            OutputConsole.Write("Finished embedding file");
            OutputConsole.Write(string.Format("Used {0} samples", (file.Length + extraBytes) * 8));
            audio.Save();
            return audio.data;
        }

        public static byte[] EncryptFileLinear(byte[] wav, byte[] file, string filename)
        {
            WavAudio audio = new WavAudio(wav);
            uint value = 0;
            int extraBytes = 2 + filename.Length + file.Length.ToString().Length;
            HiddenFile f = new HiddenFile(file, filename);
            OutputConsole.Write(string.Format("File size: {0} bytes", file.Length));
            f.cipherFile((int)audio.totalSamples);
            if (file.Length <= Math.Floor((double)(audio.totalSamples / 8)) - extraBytes)
            {
                uint n = 0;
                OutputConsole.Write("Ciphering file");
                OutputConsole.Write("Processing wav file...");
                OutputConsole.Write("Writing metadata...");
                //Write file size
                for (int i = 0; i < file.Length.ToString().Length; i++)
                {
                    value = file.Length.ToString()[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = n;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                        n++;
                    }

                }
                value = '#';
                for (int x = 0; x < 8; x++)
                {
                    uint sample = n;
                    uint sampleValue = audio.samples[sample];
                    sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                    audio.samples[sample] = sampleValue;
                    n++;
                }
                //Write file name
                for (int i = 0; i < filename.Length; i++)
                {
                    value = filename[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = n;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                        n++;
                    }

                }
                value = 0;
                for (int x = 0; x < 8; x++)
                {
                    uint sample = n;
                    uint sampleValue = audio.samples[sample];
                    sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                    audio.samples[sample] = sampleValue;
                    n++;
                }
                //Write file content
                OutputConsole.Write("Writing file data...");
                for (int i = 0; i < file.Length; i++)
                {
                    value = f.file[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = n;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                        n++;
                    }

                }
            }
            else
            {
                OutputConsole.Write("Error");
                return null;
            }
            OutputConsole.Write("Finished embedding file");
            OutputConsole.Write(string.Format("Used {0} samples", (file.Length + extraBytes) * 8));
            audio.Save();
            return audio.data;
        }

        public static byte[] EncryptFile2(byte[] wav, byte[] file, string filename)
        {
            WavAudio audio = new WavAudio(wav);
            uint value = 0;
            int extraBytes = 2 + filename.Length + file.Length.ToString().Length;
            HiddenFile f = new HiddenFile(file, filename);
            OutputConsole.Write(string.Format("File size: {0} bytes", file.Length));
            f.cipherFile((int)audio.totalSamples);
            if (file.Length <= Math.Floor((double)(audio.totalSamples / 8)) - extraBytes)
            {
                SeedURNG generator = new SeedURNG(audio.totalSamples, audio.totalSamples, true);
                OutputConsole.Write("Seed generated");
                OutputConsole.Write("Ciphering file");
                OutputConsole.Write("Processing wav file...");
                OutputConsole.Write("Writing metadata...");
                //Write file size
                for (int i = 0; i < file.Length.ToString().Length; i++)
                {
                    value = file.Length.ToString()[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.NextN;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                    }

                }
                value = '#';
                for (int x = 0; x < 8; x++)
                {
                    uint sample = generator.NextN;
                    uint sampleValue = audio.samples[sample];
                    sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                    audio.samples[sample] = sampleValue;
                }
                //Write file name
                for (int i = 0; i < filename.Length; i++)
                {
                    value = filename[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.NextN;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                    }

                }
                value = 0;
                for (int x = 0; x < 8; x++)
                {
                    uint sample = generator.NextN;
                    uint sampleValue = audio.samples[sample];
                    sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                    audio.samples[sample] = sampleValue;
                }
                //Write file content
                OutputConsole.Write("Writing file data...");
                for (int i = 0; i < file.Length; i++)
                {
                    value = f.file[i];
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.NextN;
                        uint sampleValue = audio.samples[sample];
                        sampleValue = (sampleValue & 0xFFFFFFFE) | ((value >> x) & 1);
                        audio.samples[sample] = sampleValue;
                    }

                }

            }
            else
            {
                OutputConsole.Write("Error");
                return null;
            }
            OutputConsole.Write("Finished embedding file");
            OutputConsole.Write(string.Format("Used {0} samples", (file.Length + extraBytes) * 8));
            audio.Save();
            return audio.data;
        }

        public static HiddenFile DecryptFile(byte[] wav)
        {
            try
            {
                WavAudio audio = new WavAudio(wav);
                string text = string.Empty;
                SeedURNG generator = new SeedURNG(audio.totalSamples, audio.totalSamples);
                OutputConsole.Write("Seed generated");
                OutputConsole.Write("Processing wav file...");
                OutputConsole.Write("Reading metadata...");
                uint value = 0;
                do
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.Next;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                    }
                    if (value != '#')
                        text += Convert.ToChar(value);
                } while (value != '#' && char.IsNumber((char)value));
                int filesize = int.Parse(text);
                OutputConsole.Write(string.Format("Extracted file size: {0} bytes", filesize));
                text = string.Empty;
                do
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.Next;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                    }
                    if (value != 0)
                        text += Convert.ToChar(value);
                } while (value != 0);
                string filename = text;
                OutputConsole.Write(string.Format("Extracted file name: {0}", filename));
                byte[] file = new byte[filesize];
                for (int i = 0; i < filesize; i++)
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.Next;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                    }
                    file[i] = (byte)value;
                }
                OutputConsole.Write(string.Format("Extracted file content"));
                HiddenFile f = new HiddenFile(file, filename);
                OutputConsole.Write("Ciphering file...");
                f.cipherFile((int)audio.totalSamples);
                return f;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static HiddenFile DecryptFileLinear(byte[] wav)
        {
            try
            {
                WavAudio audio = new WavAudio(wav);
                string text = string.Empty;
                uint n = 0;
                OutputConsole.Write("Processing wav file...");
                OutputConsole.Write("Reading metadata...");
                uint value = 0;
                do
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = n;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                        n++;
                    }
                    if (value != '#')
                        text += Convert.ToChar(value);
                } while (value != '#' && char.IsNumber((char)value));
                int filesize = int.Parse(text);
                OutputConsole.Write(string.Format("Extracted file size: {0} bytes", filesize));
                text = string.Empty;
                do
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = n;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                        n++;
                    }
                    if (value != 0)
                        text += Convert.ToChar(value);
                } while (value != 0);
                string filename = text;
                OutputConsole.Write(string.Format("Extracted file name: {0}", filename));
                byte[] file = new byte[filesize];
                for (int i = 0; i < filesize; i++)
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = n;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                        n++;
                    }
                    file[i] = (byte)value;
                }
                OutputConsole.Write(string.Format("Extracted file content"));
                HiddenFile f = new HiddenFile(file, filename);
                OutputConsole.Write("Ciphering file...");
                f.cipherFile((int)audio.totalSamples);
                return f;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static HiddenFile DecryptFile2(byte[] wav)
        {
            try
            {
                WavAudio audio = new WavAudio(wav);
                string text = string.Empty;
                SeedURNG generator = new SeedURNG(audio.totalSamples, audio.totalSamples, true);
                OutputConsole.Write("Seed generated");
                OutputConsole.Write("Processing wav file...");
                OutputConsole.Write("Reading metadata...");
                uint value = 0;
                do
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.NextN;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                    }
                    if (value != '#')
                        text += Convert.ToChar(value);
                } while (value != '#' && char.IsNumber((char)value));
                int filesize = int.Parse(text);
                OutputConsole.Write(string.Format("Extracted file size: {0} bytes", filesize));
                text = string.Empty;
                do
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.NextN;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                    }
                    if (value != 0)
                        text += Convert.ToChar(value);
                } while (value != 0);
                string filename = text;
                OutputConsole.Write(string.Format("Extracted file name: {0}", filename));
                byte[] file = new byte[filesize];
                for (int i = 0; i < filesize; i++)
                {
                    value = 0;
                    for (int x = 0; x < 8; x++)
                    {
                        uint sample = generator.NextN;
                        uint sampleValue = audio.samples[sample];
                        value = value | ((sampleValue & 1) << x);
                    }
                    file[i] = (byte)value;
                }
                OutputConsole.Write(string.Format("Extracted file content"));
                HiddenFile f = new HiddenFile(file, filename);
                OutputConsole.Write("Ciphering file...");
                f.cipherFile((int)audio.totalSamples);
                return f;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}