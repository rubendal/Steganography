namespace Steganography
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.loadImage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.encryptButton = new System.Windows.Forms.Button();
            this.decryptButton = new System.Windows.Forms.Button();
            this.loadDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.console = new System.Windows.Forms.ListBox();
            this.loadFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.encryptFile = new System.Windows.Forms.Button();
            this.decryptFile = new System.Windows.Forms.Button();
            this.random = new System.Windows.Forms.RadioButton();
            this.linear = new System.Windows.Forms.RadioButton();
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.loadWavFile = new System.Windows.Forms.Button();
            this.loadWav = new System.Windows.Forms.OpenFileDialog();
            this.saveWav = new System.Windows.Forms.SaveFileDialog();
            this.audioLabel = new System.Windows.Forms.Label();
            this.randomM2 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // imageBox
            // 
            this.imageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox.Location = new System.Drawing.Point(15, 12);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(239, 239);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox.TabIndex = 0;
            this.imageBox.TabStop = false;
            // 
            // loadImage
            // 
            this.loadImage.Location = new System.Drawing.Point(15, 257);
            this.loadImage.Name = "loadImage";
            this.loadImage.Size = new System.Drawing.Size(75, 23);
            this.loadImage.TabIndex = 1;
            this.loadImage.Text = "Load Image";
            this.loadImage.UseVisualStyleBackColor = true;
            this.loadImage.Click += new System.EventHandler(this.loadImage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Text";
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(383, 133);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(87, 23);
            this.encryptButton.TabIndex = 4;
            this.encryptButton.Text = "Encrypt String";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // decryptButton
            // 
            this.decryptButton.Location = new System.Drawing.Point(476, 133);
            this.decryptButton.Name = "decryptButton";
            this.decryptButton.Size = new System.Drawing.Size(89, 23);
            this.decryptButton.TabIndex = 5;
            this.decryptButton.Text = "Decrypt String";
            this.decryptButton.UseVisualStyleBackColor = true;
            this.decryptButton.Click += new System.EventHandler(this.decryptButton_Click);
            // 
            // loadDialog
            // 
            this.loadDialog.FileName = ".*";
            this.loadDialog.Filter = "Image Files (*.bmp,*.jpg,*.png)|*.bmp,*.jpg,*.png";
            // 
            // saveDialog
            // 
            this.saveDialog.Filter = "Image Files (*.bmp,*.png)|*.bmp,*.png";
            // 
            // console
            // 
            this.console.FormattingEnabled = true;
            this.console.Location = new System.Drawing.Point(282, 162);
            this.console.Name = "console";
            this.console.Size = new System.Drawing.Size(286, 173);
            this.console.TabIndex = 6;
            this.console.KeyDown += new System.Windows.Forms.KeyEventHandler(this.console_KeyDown);
            this.console.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.console_MouseDoubleClick);
            // 
            // loadFileDialog
            // 
            this.loadFileDialog.FileName = ".*";
            this.loadFileDialog.Filter = "Any file (*.*) | *.*";
            // 
            // encryptFile
            // 
            this.encryptFile.Location = new System.Drawing.Point(15, 286);
            this.encryptFile.Name = "encryptFile";
            this.encryptFile.Size = new System.Drawing.Size(75, 23);
            this.encryptFile.TabIndex = 8;
            this.encryptFile.Text = "Encrypt File";
            this.encryptFile.UseVisualStyleBackColor = true;
            this.encryptFile.Click += new System.EventHandler(this.encryptFile_Click);
            // 
            // decryptFile
            // 
            this.decryptFile.Location = new System.Drawing.Point(15, 315);
            this.decryptFile.Name = "decryptFile";
            this.decryptFile.Size = new System.Drawing.Size(75, 23);
            this.decryptFile.TabIndex = 9;
            this.decryptFile.Text = "Decrypt File";
            this.decryptFile.UseVisualStyleBackColor = true;
            this.decryptFile.Click += new System.EventHandler(this.decryptFile_Click);
            // 
            // random
            // 
            this.random.AutoSize = true;
            this.random.Location = new System.Drawing.Point(189, 257);
            this.random.Name = "random";
            this.random.Size = new System.Drawing.Size(65, 17);
            this.random.TabIndex = 10;
            this.random.Text = "Random";
            this.random.UseVisualStyleBackColor = true;
            this.random.CheckedChanged += new System.EventHandler(this.random_CheckedChanged);
            // 
            // linear
            // 
            this.linear.AutoSize = true;
            this.linear.Location = new System.Drawing.Point(189, 280);
            this.linear.Name = "linear";
            this.linear.Size = new System.Drawing.Size(54, 17);
            this.linear.TabIndex = 11;
            this.linear.Text = "Linear";
            this.linear.UseVisualStyleBackColor = true;
            this.linear.CheckedChanged += new System.EventHandler(this.linear_CheckedChanged);
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(282, 28);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(286, 99);
            this.textBox.TabIndex = 12;
            this.textBox.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(279, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Log";
            // 
            // loadWavFile
            // 
            this.loadWavFile.Location = new System.Drawing.Point(96, 257);
            this.loadWavFile.Name = "loadWavFile";
            this.loadWavFile.Size = new System.Drawing.Size(87, 23);
            this.loadWavFile.TabIndex = 14;
            this.loadWavFile.Text = "Load WAV File";
            this.loadWavFile.UseVisualStyleBackColor = true;
            this.loadWavFile.Click += new System.EventHandler(this.loadWavFile_Click);
            // 
            // loadWav
            // 
            this.loadWav.FileName = "*.wav";
            this.loadWav.Filter = "Wav File (*.wav) | *.wav";
            // 
            // saveWav
            // 
            this.saveWav.FileName = "*.wav";
            this.saveWav.Filter = "Wav File (*.wav) | *.wav";
            // 
            // audioLabel
            // 
            this.audioLabel.Location = new System.Drawing.Point(20, 19);
            this.audioLabel.Name = "audioLabel";
            this.audioLabel.Size = new System.Drawing.Size(225, 219);
            this.audioLabel.TabIndex = 15;
            this.audioLabel.Text = "Using Wav File";
            this.audioLabel.Visible = false;
            // 
            // randomM2
            // 
            this.randomM2.AutoSize = true;
            this.randomM2.Location = new System.Drawing.Point(189, 303);
            this.randomM2.Name = "randomM2";
            this.randomM2.Size = new System.Drawing.Size(83, 17);
            this.randomM2.TabIndex = 16;
            this.randomM2.Text = "Random M2";
            this.randomM2.UseVisualStyleBackColor = true;
            this.randomM2.CheckedChanged += new System.EventHandler(this.randomM2_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 349);
            this.Controls.Add(this.randomM2);
            this.Controls.Add(this.audioLabel);
            this.Controls.Add(this.loadWavFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.linear);
            this.Controls.Add(this.random);
            this.Controls.Add(this.decryptFile);
            this.Controls.Add(this.encryptFile);
            this.Controls.Add(this.console);
            this.Controls.Add(this.decryptButton);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loadImage);
            this.Controls.Add(this.imageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Steganography";
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.Button loadImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Button decryptButton;
        private System.Windows.Forms.OpenFileDialog loadDialog;
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.ListBox console;
        private System.Windows.Forms.OpenFileDialog loadFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button encryptFile;
        private System.Windows.Forms.Button decryptFile;
        private System.Windows.Forms.RadioButton random;
        private System.Windows.Forms.RadioButton linear;
        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button loadWavFile;
        private System.Windows.Forms.OpenFileDialog loadWav;
        private System.Windows.Forms.SaveFileDialog saveWav;
        private System.Windows.Forms.Label audioLabel;
        private System.Windows.Forms.RadioButton randomM2;
    }
}

