using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Steganography
{
    public partial class Form1 : Form
    {
        public enum Mode
        {
            Image,
            Audio
        }

        Image image;
        byte[] audio;
        byte[] file;
        string filename;
        Mode currentMode;
        Stopwatch stopwatch;

        public Form1()
        {
            InitializeComponent();
            ToolConsole.Bind(console);
            random.Checked = true;
            currentMode = Mode.Image;
            stopwatch = new Stopwatch();
        }

        private void loadImage_Click(object sender, EventArgs e)
        {
            loadDialog.FileName = "*.*";
            DialogResult res = loadDialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                if (image != null)
                {
                    image.Dispose();
                }
                string ext = Path.GetExtension(loadDialog.FileName);
                if (ext == ".png" || ext == ".bmp" || ext == "jpg")
                {
                    try
                    {
                        image = Image.FromFile(loadDialog.FileName);
                        imageBox.Image = image;
                        ToolConsole.Write(string.Format("Image loaded \nTotal pixels = {0}", image.Width * image.Height));
                        ToolConsole.Write(string.Format("Maximum file size for this image = {0} - (file size digits + file name character count) bytes", FileSizeFormatProvider.GetFileSize((image.Width * image.Height) - 2)));
                        currentMode = Mode.Image;
                        audio = null;
                        audioLabel.Visible = false;
                    }
                    catch
                    {
                        image = null;
                        imageBox.Image = null;
                    }
                }
            }
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Image && image != null)
            {
                Bitmap encrypted;
                if (random.Checked || randomM2.Checked)
                {
                    encrypted = Steganography.InsertEncryptedTextToImage(image, textBox.Text);
                }
                else
                {
                    encrypted = Steganography.InsertEncryptedTextToImageLinear(image, textBox.Text);
                }
                if (encrypted != null)
                {
                    saveDialog.FileName = "*.*";
                    DialogResult res = saveDialog.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        encrypted.Save(saveDialog.FileName);
                        ToolConsole.Write("Image saved");
                    }
                }
            }
            if (currentMode == Mode.Audio && audio != null)
            {
                byte[] file;
                if (random.Checked)
                {
                    file = AudioSteganography.EncryptText(audio, textBox.Text);
                }
                else
                {
                    file = AudioSteganography.EncryptTextLinear(audio, textBox.Text);
                }
                if (file != null)
                {
                    DialogResult res = saveWav.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        File.WriteAllBytes(saveWav.FileName, file);
                        ToolConsole.Write("Wav file saved");
                    }
                }
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Image && image != null)
            {
                string text;
                if (random.Checked || randomM2.Checked)
                {
                     text = Steganography.GetDecryptedTextFromImage(image);
                }
                else
                {
                    text = Steganography.GetDecryptedTextFromImageLinear(image);
                }
                if (text != null)
                {
                    textBox.Text = text;
                    ToolConsole.Write("Text decrypted");
                }
                else
                {
                    //MessageBox.Show("This image doesn't have an encrypted text or an error occurred");
                }
            }
            if (currentMode == Mode.Audio && audio != null)
            {
                string text;
                if (random.Checked)
                {
                    text = AudioSteganography.DecryptText(audio);
                }
                else
                {
                    text = AudioSteganography.DecryptTextLinear(audio);
                }
                if (text != null)
                {
                    textBox.Text = text;
                    ToolConsole.Write("Text decrypted");
                }
            }
        }

        private void encryptFile_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Image && image != null)
            {
                loadFileDialog.FileName = "*.*";
                DialogResult res = loadFileDialog.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    file = File.ReadAllBytes(loadFileDialog.FileName);
                    filename = loadFileDialog.SafeFileName;
                    ToolConsole.Write("Added File to buffer");
                    Bitmap encrypted;
                    stopwatch.Restart();
                    if (random.Checked)
                    {
                        encrypted = Steganography.InsertFileToImage(image, file, filename);
                    }
                    else
                    {
                        if (linear.Checked)
                        {
                            encrypted = Steganography.InsertFileToImageLinear(image, file, filename);
                        }
                        else
                        {
                            encrypted = Steganography.InsertFileToImage2(image, file, filename);
                        }
                    }
                    if (encrypted != null)
                    {
                        stopwatch.Stop();
                        ToolConsole.Write(string.Format("Process completed in {0} ms", stopwatch.ElapsedMilliseconds));
                        saveDialog.FileName = "*.*";
                        DialogResult res2 = saveDialog.ShowDialog();
                        if (res2 == System.Windows.Forms.DialogResult.OK)
                        {
                            encrypted.Save(saveDialog.FileName);
                            ToolConsole.Write("Image saved");
                        }
                    }
                    stopwatch.Reset();
                }
            }
            if (currentMode == Mode.Audio && audio != null)
            {
                loadFileDialog.FileName = "*.*";
                DialogResult res = loadFileDialog.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    byte[] file;
                    stopwatch.Restart();
                    if (random.Checked)
                    {
                        file = AudioSteganography.EncryptFile(audio, File.ReadAllBytes(loadFileDialog.FileName), loadFileDialog.SafeFileName);
                    }
                    else
                    {
                        if (linear.Checked)
                        {
                            file = AudioSteganography.EncryptFileLinear(audio, File.ReadAllBytes(loadFileDialog.FileName), loadFileDialog.SafeFileName);
                        }
                        else
                        {
                            file = AudioSteganography.EncryptFile2(audio, File.ReadAllBytes(loadFileDialog.FileName), loadFileDialog.SafeFileName);
                        }
                    }
                    if (file != null)
                    {
                        stopwatch.Stop();
                        ToolConsole.Write(string.Format("Process completed in {0} ms", stopwatch.ElapsedMilliseconds));
                        DialogResult res2 = saveWav.ShowDialog();
                        if (res2 == System.Windows.Forms.DialogResult.OK)
                        {
                            File.WriteAllBytes(saveWav.FileName, file);
                            ToolConsole.Write("Wav file saved");
                        }
                    }
                    stopwatch.Reset();
                }
            }
        }

        private void decryptFile_Click(object sender, EventArgs e)
        {
            if (currentMode == Mode.Image && image != null)
            {
                HiddenFile f;
                stopwatch.Restart();
                if (random.Checked)
                {
                    f = Steganography.GetFileFromImage(image);
                }
                else
                {
                    if (linear.Checked)
                    {
                        f = Steganography.GetFileFromImageLinear(image);
                    }
                    else
                    {
                        f = Steganography.GetFileFromImage2(image);
                    }
                }
                if (f != null)
                {
                    stopwatch.Stop();
                    ToolConsole.Write(string.Format("Process completed in {0} ms", stopwatch.ElapsedMilliseconds));
                    saveFileDialog.FileName = f.filename;
                    DialogResult res = saveFileDialog.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, f.file);
                        ToolConsole.Write("File saved");
                        if (Path.GetExtension(saveFileDialog.FileName) == ".bmp" || Path.GetExtension(saveFileDialog.FileName) == ".png" || Path.GetExtension(saveFileDialog.FileName) == ".jpg")
                        {
                            ImgPreview p = new ImgPreview(Image.FromFile(saveFileDialog.FileName));
                            p.ShowDialog();
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("This image doesn't have an encrypted text or an error occurred");
                }
                stopwatch.Reset();
            }
            if (currentMode == Mode.Audio && audio != null)
            {
                HiddenFile file;
                stopwatch.Restart();
                if (random.Checked)
                {
                     file = AudioSteganography.DecryptFile(audio);
                }
                else
                {
                    if (linear.Checked)
                    {
                        file = AudioSteganography.DecryptFileLinear(audio);
                    }
                    else
                    {
                        file = AudioSteganography.DecryptFile2(audio);
                    }
                }
                if (file != null)
                {
                    stopwatch.Stop();
                    ToolConsole.Write(string.Format("Process completed in {0} ms", stopwatch.ElapsedMilliseconds));
                    saveFileDialog.FileName = file.filename;
                    DialogResult res = saveFileDialog.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, file.file);
                        ToolConsole.Write("File saved");
                        if (Path.GetExtension(saveFileDialog.FileName) == ".bmp" || Path.GetExtension(saveFileDialog.FileName) == ".png" || Path.GetExtension(saveFileDialog.FileName) == ".jpg")
                        {
                            ImgPreview p = new ImgPreview(Image.FromFile(saveFileDialog.FileName));
                            p.ShowDialog();
                        }
                    }
                }
                stopwatch.Reset();
            }
        }

        private void console_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = console.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                MessageBox.Show(console.Items[index].ToString());
            }
        }

        private void random_CheckedChanged(object sender, EventArgs e)
        {
            if (random.Checked)
                ToolConsole.Write("Using random steganography algorithm (Not recommended for huge files or text near 60% image pixels or more than 10% of available bytes in wav files)");
        }

        private void linear_CheckedChanged(object sender, EventArgs e)
        {
            if (linear.Checked)
                ToolConsole.Write("Using linear steganography algorithm (Fastest)");
        }

        private void console_KeyDown(object sender, KeyEventArgs e)
        {
            if (console.SelectedIndex >= 0 && e.KeyCode == Keys.Delete)
            {
                console.Items.RemoveAt(console.SelectedIndex);
            }
        }

        private void loadWavFile_Click(object sender, EventArgs e)
        {
            DialogResult res = loadWav.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                audio = File.ReadAllBytes(loadWav.FileName);
                WavAudio wav = new WavAudio(audio);
                if (wav.data != null)
                {
                    ToolConsole.Write(string.Format("Audio loaded \nSamples found: {0}", wav.totalSamples));
                    ToolConsole.Write(string.Format("Maximum file size for this file = {0} - (file size digits + file name character count) bytes", FileSizeFormatProvider.GetFileSize(wav.bytesAvailable)));
                    currentMode = Mode.Audio;
                    if (image != null)
                    {
                        image.Dispose();
                    }
                    image = null;
                    imageBox.Image = null;
                    audioLabel.Text = string.Format("Using Wav File: {0}", loadWav.SafeFileName);
                    audioLabel.Visible = true;
                }
                else
                {
                    audio = null;
                }
            }
        }

        private void randomM2_CheckedChanged(object sender, EventArgs e)
        {
            if (randomM2.Checked)
                ToolConsole.Write("Using random method 2 steganography algorithm (Poor performance on small files, but better performance than random using bigger files) \nWorks only with files...");
        }

    }
}
