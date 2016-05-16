using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Steganography
{
    public static class OutputConsole
    {
        private static ListBox list;

        public static void Bind(ListBox listbox)
        {
            list = listbox;
        }

        public static void Show()
        {
            if (list != null)
            {
                list.Visible = true;
            }
        }

        public static void Hide()
        {
            if (list != null)
            {
                list.Visible = false;
            }
        }

        public static void Write(string text)
        {
            if (list != null)
            {
                list.Items.Add(text);
                if (list.Items.Count > (list.Height / list.ItemHeight))
                {
                    list.Items.RemoveAt(0);
                }
                list.SelectedIndex = list.Items.Count - 1;
            }
        }

        public static void Clear()
        {
            if (list != null)
            {
                list.Items.Clear();
            }
        }

    }

}
