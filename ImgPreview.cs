using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Steganography
{
    public partial class ImgPreview : Form
    {
        public ImgPreview()
        {
            InitializeComponent();
        }

        public ImgPreview(Image image)
        {
            InitializeComponent();
            imageBox.Image = image;
            this.Size = image.Size;
        }

        private void ImgPreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            imageBox.Image.Dispose();
        }
    }
}
