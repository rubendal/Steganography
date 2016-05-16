using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Steganography
{
    public static class Steganography
    {

        public static long GetImageMaxLinear(Bitmap img)
        {
            return img.Width * img.Height;
        }

        private static Point LinearIndexToPoint(int index, int width, int height)
        {
            if (index < 0)
            {
                index *= -1;
            }
            return new Point(index % width, (int)Math.Floor((double)(index / width)));
        }

        public static Bitmap InsertEncryptedTextToImage(Image image, string text)
        {
            Bitmap img = new Bitmap(image);
            EncodeMessage(ref img, text);
            return img;
        }

        public static Bitmap InsertEncryptedTextToImageLinear(Image image, string text)
        {
            Bitmap img = new Bitmap(image);
            EncodeMessageLinear(ref img, text);
            return img;
        }

        private static void EncodeMessage(ref Bitmap img, string text)
        {
            int maxLinear = img.Width * img.Height;
            SeedRNG generator = new SeedRNG((maxLinear + img.Width).GetHashCode(), maxLinear);
            OutputConsole.Write("Seed generated");
            string pass = string.Format("{0}x{1}={2}", img.Width, img.Height, maxLinear);
            AESEncrypt encrypt = new AESEncrypt();
            string encrypted = encrypt.EncryptString(text, pass);
            OutputConsole.Write(string.Format("Text encrypted \n{0}",encrypted));
            OutputConsole.Write("Processing image...");
            if (encrypted.Length < maxLinear)
            {
                for (int i = 0; i < encrypted.Length; i++)
                {
                    Point point = LinearIndexToPoint(generator.Next, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    char letter = encrypted[i];
                    int value = Convert.ToInt32(letter);
                    Color n = EncodePixel(pixel,value);
                    img.SetPixel(point.X, point.Y, n);
                }
                //Null value placed at the end to indicate end of text
                Point pointEnd = LinearIndexToPoint(generator.Next, img.Width, img.Height);
                Color pixelEnd = img.GetPixel(pointEnd.X, pointEnd.Y);
                img.SetPixel(pointEnd.X, pointEnd.Y, EncodePixel(pixelEnd, 0));
                OutputConsole.Write("Finished embedding encrypted text");
            }
            else
            {
                OutputConsole.Write("Error: Image size doesn't support encrypted text size");
                img = null;
            }
        }

        private static void EncodeMessageLinear(ref Bitmap img, string text)
        {
            int maxLinear = img.Width * img.Height;
            string pass = string.Format("{0}x{1}={2}", img.Width, img.Height, maxLinear);
            AESEncrypt encrypt = new AESEncrypt();
            int c = 0;
            string encrypted = encrypt.EncryptString(text, pass);
            OutputConsole.Write(string.Format("Text encrypted \n{0}", encrypted));
            OutputConsole.Write("Processing image...");
            if (encrypted.Length < maxLinear)
            {
                for (int i = 0; i < encrypted.Length; i++)
                {
                    Point point = LinearIndexToPoint(i, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    char letter = encrypted[i];
                    int value = Convert.ToInt32(letter);
                    Color n = EncodePixel(pixel, value);
                    img.SetPixel(point.X, point.Y, n);
                    c = i;
                }
                //Null value placed at the end to indicate end of text - or using 255 for better hiding
                Point pointEnd = LinearIndexToPoint(c, img.Width, img.Height);
                Color pixelEnd = img.GetPixel(pointEnd.X, pointEnd.Y);
                img.SetPixel(pointEnd.X, pointEnd.Y, EncodePixel(pixelEnd, 0));
                OutputConsole.Write("Finished embedding encrypted text");
            }
            else
            {
                OutputConsole.Write("Error: Image size doesn't support encrypted text size");
                img = null;
            }
        }

        public static string GetDecryptedTextFromImage(Image image)
        {
            Bitmap img = new Bitmap(image);
            int maxLinear = image.Width * image.Height;
            SeedRNG generator = new SeedRNG((maxLinear + image.Width).GetHashCode(), maxLinear);
            OutputConsole.Write("Seed generated");
            string pass = string.Format("{0}x{1}={2}", image.Width, image.Height, maxLinear);
            AESEncrypt encrypt = new AESEncrypt();
            string text = string.Empty;
            int value=0;
            OutputConsole.Write("Processing image...");
            do
            {
                Point point = LinearIndexToPoint(generator.Next, image.Width, image.Height);
                Color pixel = img.GetPixel(point.X, point.Y);
                value = DecodePixel(pixel);
                
                if (value != 0)
                    text += Convert.ToChar(value);
            } while (value != 0);
            try
            {
                OutputConsole.Write(string.Format("String found: \n{0}", text));
                OutputConsole.Write("Decrypting text...");
                return encrypt.DecryptString(text, pass);
            }
            catch (Exception e)
            {
                OutputConsole.Write("Error: Text not found");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string GetDecryptedTextFromImageLinear(Image image)
        {
            Bitmap img = new Bitmap(image);
            int maxLinear = image.Width * image.Height;
            string pass = string.Format("{0}x{1}={2}", image.Width, image.Height, maxLinear);
            AESEncrypt encrypt = new AESEncrypt();
            string text = string.Empty;
            int value = 0;
            int i = 0;
            OutputConsole.Write("Processing image...");
            do
            {
                Point point = LinearIndexToPoint(i, image.Width, image.Height);
                Color pixel = img.GetPixel(point.X, point.Y);
                value = DecodePixel(pixel);
                i++;
                if (value != 255)
                    text += Convert.ToChar(value);
            } while (value != 255);
            try
            {
                OutputConsole.Write(string.Format("String found: \n{0}", text));
                OutputConsole.Write("Decrypting text...");
                return encrypt.DecryptString(text, pass);
            }
            catch (Exception e)
            {
                OutputConsole.Write("Error: Text not found");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static Bitmap InsertFileToImage(Image image, byte[] file, string filename)
        {
            Bitmap img = new Bitmap(image);
            EncodeFileWithColor(ref img, file, filename);
            return img;
        }

        public static Bitmap InsertFileToImageLinear(Image image, byte[] file, string filename)
        {
            Bitmap img = new Bitmap(image);
            EncodeFileWithColorLinear(ref img, file, filename);
            return img;
        }

        public static Bitmap InsertFileToImage2(Image image, byte[] file, string filename)
        {
            Bitmap img = new Bitmap(image);
            EncodeFileWithColor2(ref img, file, filename);
            return img;
        }

        private static void EncodeFileWithColor(ref Bitmap img, byte[] file, string filename)
        {
            Bitmap backup = img.Clone() as Bitmap;
            int maxLinear = img.Width * img.Height;
            OutputConsole.Write(string.Format("File size: {0}", FileSizeFormatProvider.GetFileSize(file.Length)));
            SeedRNG generator = new SeedRNG((maxLinear + img.Width).GetHashCode(), maxLinear);
            OutputConsole.Write("Seed generated");
            int extraBytes = 2 + filename.Length + file.Length.ToString().Length;
            HiddenFile f = new HiddenFile(file, filename);
            f.cipherFile((maxLinear + img.Width).GetHashCode());
            OutputConsole.Write("Ciphering file...");
            if (file.Length < maxLinear - extraBytes)
            {
                string fileLength = file.Length.ToString();
                OutputConsole.Write("Processing image...");
                OutputConsole.Write("Writing metadata...");
                for (int i = 0; i < fileLength.Length; i++)
                {
                    Point point = LinearIndexToPoint(generator.Next, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    char letter = fileLength[i];
                    int value = Convert.ToInt32(letter);
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel,value));
                }
                //Insert # to separate file length from filename
                Point point1 = LinearIndexToPoint(generator.Next, img.Width, img.Height);
                Color pixel1 = img.GetPixel(point1.X, point1.Y);
                int value1 = Convert.ToInt32('#');
                img.SetPixel(point1.X, point1.Y, EncodePixel(pixel1, value1));

                //Insert filename and finish with null char
                for (int i = 0; i < filename.Length; i++)
                {
                    Point point = LinearIndexToPoint(generator.Next, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    char letter = filename[i];
                    int value = Convert.ToInt32(letter);
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel, value));
                }
                point1 = LinearIndexToPoint(generator.Next, img.Width, img.Height);
                pixel1 = img.GetPixel(point1.X, point1.Y);
                value1 = 0;
                img.SetPixel(point1.X, point1.Y, EncodePixel(pixel1,value1));
                OutputConsole.Write("Writing file data...");
                //Write file
                for (int i = 0; i < file.Length; i++)
                {
                    Point point = LinearIndexToPoint(generator.Next, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    int value = f.file[i];
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel, value));
                }
                OutputConsole.Write("Finished embedding file");
            }
            else
            {
                OutputConsole.Write("File size is greater than total pixels in image with extra data, resize or use another image to encrypt this file");
                img = null;
            }
        }

        private static void EncodeFileWithColorLinear(ref Bitmap img, byte[] file, string filename)
        {
            Bitmap backup = img.Clone() as Bitmap;
            int maxLinear = img.Width * img.Height;
            int c = 0;
            OutputConsole.Write(string.Format("File size: {0}", FileSizeFormatProvider.GetFileSize(file.Length)));
            int extraBytes = 2 + filename.Length + file.Length.ToString().Length;
            HiddenFile f = new HiddenFile(file, filename);
            f.cipherFile((maxLinear + img.Width).GetHashCode());
            OutputConsole.Write("Ciphering file...");
            if (file.Length < maxLinear - extraBytes)
            {
                string fileLength = file.Length.ToString();
                OutputConsole.Write("Processing image...");
                OutputConsole.Write("Writing metadata...");
                for (int i = 0; i < fileLength.Length; i++)
                {
                    Point point = LinearIndexToPoint(c, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    char letter = fileLength[i];
                    int value = Convert.ToInt32(letter);
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel, value));
                    c++;
                }
                //Insert # to separate file length from filename
                Point point1 = LinearIndexToPoint(c, img.Width, img.Height);
                Color pixel1 = img.GetPixel(point1.X, point1.Y);
                int value1 = Convert.ToInt32('#');
                img.SetPixel(point1.X, point1.Y, EncodePixel(pixel1, value1));
                c++;
                //Insert filename and finish with null char
                for (int i = 0; i < filename.Length; i++)
                {
                    Point point = LinearIndexToPoint(c, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    char letter = filename[i];
                    int value = Convert.ToInt32(letter);
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel, value));
                    c++;
                }
                point1 = LinearIndexToPoint(c, img.Width, img.Height);
                pixel1 = img.GetPixel(point1.X, point1.Y);
                value1 = 0;
                img.SetPixel(point1.X, point1.Y, EncodePixel(pixel1, value1));
                c++;
                //Write file
                OutputConsole.Write("Writing file data...");
                for (int i = 0; i < file.Length; i++)
                {
                    Point point = LinearIndexToPoint(c, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    int value = f.file[i];
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel, value));
                    c++;
                }
                OutputConsole.Write("Finished embedding file");
            }
            else
            {
                OutputConsole.Write("File size is greater than total pixels in image with extra data, resize or use another image to encrypt this file");
                img = null;
            }
        }

        private static void EncodeFileWithColor2(ref Bitmap img, byte[] file, string filename)
        {
            Bitmap backup = img.Clone() as Bitmap;
            int maxLinear = img.Width * img.Height;
            OutputConsole.Write(string.Format("File size: {0}", FileSizeFormatProvider.GetFileSize(file.Length)));
            SeedRNG generator = new SeedRNG((maxLinear + img.Width).GetHashCode(), maxLinear, true);
            OutputConsole.Write("Seed generated");
            int extraBytes = 2 + filename.Length + file.Length.ToString().Length;
            HiddenFile f = new HiddenFile(file, filename);
            f.cipherFile((maxLinear + img.Width).GetHashCode());
            OutputConsole.Write("Ciphering file...");
            if (file.Length < maxLinear - extraBytes)
            {
                string fileLength = file.Length.ToString();
                OutputConsole.Write("Processing image...");
                OutputConsole.Write("Writing metadata...");
                for (int i = 0; i < fileLength.Length; i++)
                {
                    Point point = LinearIndexToPoint(generator.NextN, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    char letter = fileLength[i];
                    int value = Convert.ToInt32(letter);
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel, value));
                }
                //Insert # to separate file length from filename
                Point point1 = LinearIndexToPoint(generator.NextN, img.Width, img.Height);
                Color pixel1 = img.GetPixel(point1.X, point1.Y);
                int value1 = Convert.ToInt32('#');
                img.SetPixel(point1.X, point1.Y, EncodePixel(pixel1, value1));

                //Insert filename and finish with null char
                for (int i = 0; i < filename.Length; i++)
                {
                    Point point = LinearIndexToPoint(generator.NextN, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    char letter = filename[i];
                    int value = Convert.ToInt32(letter);
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel, value));
                }
                point1 = LinearIndexToPoint(generator.NextN, img.Width, img.Height);
                pixel1 = img.GetPixel(point1.X, point1.Y);
                value1 = 0;
                img.SetPixel(point1.X, point1.Y, EncodePixel(pixel1, value1));
                OutputConsole.Write("Writing file data...");
                //Write file
                for (int i = 0; i < file.Length; i++)
                {
                    Point point = LinearIndexToPoint(generator.NextN, img.Width, img.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    int value = f.file[i];
                    img.SetPixel(point.X, point.Y, EncodePixel(pixel, value));
                }
                OutputConsole.Write("Finished embedding file");
            }
            else
            {
                OutputConsole.Write("File size is greater than total pixels in image with extra data, resize or use another image to encrypt this file");
                img = null;
            }
        }

        public static HiddenFile GetFileFromImage(Image image)
        {
            try
            {
                Bitmap img = new Bitmap(image);
                int maxLinear = image.Width * image.Height;
                SeedRNG generator = new SeedRNG((maxLinear + image.Width).GetHashCode(), maxLinear);
                OutputConsole.Write("Seed generated");
                string text = string.Empty;
                int value = 0;
                //Read file length
                OutputConsole.Write("Processing image...");
                OutputConsole.Write("Reading metadata...");
                do
                {
                    Point point = LinearIndexToPoint(generator.Next, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);

                    if (value != '#')
                        text += Convert.ToChar(value);
                } while (value != '#' && char.IsNumber((char)value));
                int filelength = int.Parse(text);
                OutputConsole.Write(string.Format("Extracted file size: {0} bytes", filelength));
                text = string.Empty;
                value = 0;
                //Read filename
                do
                {
                    Point point = LinearIndexToPoint(generator.Next, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);

                    if (value != 0)
                        text += Convert.ToChar(value);
                } while (value != 0);
                string filename = text;
                OutputConsole.Write(string.Format("Extracted file name: {0}", filename));
                byte[] file = new byte[filelength];
                for (int i = 0; i < filelength; i++)
                {
                    Point point = LinearIndexToPoint(generator.Next, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);
                    file[i] = (byte)value;
                }
                OutputConsole.Write(string.Format("Extracted file content"));
                HiddenFile f = new HiddenFile(file, filename);
                f.cipherFile((maxLinear + img.Width).GetHashCode());
                OutputConsole.Write("Ciphering file...");
                return f;
            }
            catch (Exception e)
            {
                OutputConsole.Write("Error: File not found");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static HiddenFile GetFileFromImageLinear(Image image)
        {
            try
            {
                Bitmap img = new Bitmap(image);
                int maxLinear = image.Width * image.Height;
                int c = 0;
                string text = string.Empty;
                int value = 0;
                OutputConsole.Write("Processing image...");
                OutputConsole.Write("Reading metadata...");
                //Read file length
                do
                {
                    Point point = LinearIndexToPoint(c, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);
                    c++;
                    if (value != '#')
                        text += Convert.ToChar(value);
                } while (value != '#' && char.IsNumber((char)value));
                int filelength = int.Parse(text);
                OutputConsole.Write(string.Format("Extracted file size: {0} bytes", filelength));
                text = string.Empty;
                value = 0;
                //Read filename
                do
                {
                    Point point = LinearIndexToPoint(c, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);
                    c++;
                    if (value != 0)
                        text += Convert.ToChar(value);
                } while (value != 0);
                string filename = text;
                OutputConsole.Write(string.Format("Extracted file name: {0}", filename));
                byte[] file = new byte[filelength];
                for (int i = 0; i < filelength; i++)
                {
                    Point point = LinearIndexToPoint(c, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);
                    file[i] = (byte)value;
                    c++;
                }
                OutputConsole.Write(string.Format("Extracted file content"));
                HiddenFile f = new HiddenFile(file, filename);
                f.cipherFile((maxLinear + img.Width).GetHashCode());
                OutputConsole.Write("Ciphering file...");
                return f;
            }
            catch (Exception e)
            {
                OutputConsole.Write("Error: File not found");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static HiddenFile GetFileFromImage2(Image image)
        {
            try
            {
                Bitmap img = new Bitmap(image);
                int maxLinear = image.Width * image.Height;
                SeedRNG generator = new SeedRNG((maxLinear + image.Width).GetHashCode(), maxLinear, true);
                OutputConsole.Write("Seed generated");
                string text = string.Empty;
                int value = 0;
                //Read file length
                OutputConsole.Write("Processing image...");
                OutputConsole.Write("Reading metadata...");
                do
                {
                    Point point = LinearIndexToPoint(generator.NextN, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);

                    if (value != '#')
                        text += Convert.ToChar(value);
                } while (value != '#' && char.IsNumber((char)value));
                int filelength = int.Parse(text);
                OutputConsole.Write(string.Format("Extracted file size: {0} bytes", filelength));
                text = string.Empty;
                value = 0;
                //Read filename
                do
                {
                    Point point = LinearIndexToPoint(generator.NextN, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);

                    if (value != 0)
                        text += Convert.ToChar(value);
                } while (value != 0);
                string filename = text;
                OutputConsole.Write(string.Format("Extracted file name: {0}", filename));
                byte[] file = new byte[filelength];
                for (int i = 0; i < filelength; i++)
                {
                    Point point = LinearIndexToPoint(generator.NextN, image.Width, image.Height);
                    Color pixel = img.GetPixel(point.X, point.Y);
                    value = DecodePixel(pixel);
                    file[i] = (byte)value;
                }
                OutputConsole.Write(string.Format("Extracted file content"));
                HiddenFile f = new HiddenFile(file, filename);
                f.cipherFile((maxLinear + img.Width).GetHashCode());
                OutputConsole.Write("Ciphering file...");
                return f;
            }
            catch (Exception e)
            {
                OutputConsole.Write("Error: File not found");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static Color EncodePixel(Color pixel, int value)
        {
            int blueValue = value & 7;
            int greenValue = (value >> 3) & 7;
            int redValue = (value >> 6) & 3;

            int red = (pixel.R & 0xFC) | redValue;
            int green = (pixel.G & 0xF8) | greenValue;
            int blue = (pixel.B & 0xF8) | blueValue;

            return Color.FromArgb(red, green, blue);
        }

        public static int DecodePixel(Color pixel)
        {
            int red = (pixel.R & 3);
            int green = (pixel.G & 7);
            int blue = (pixel.B & 7);
            int value = blue | (green << 3) | (red << 6);
            return value;
        }
    }
}
