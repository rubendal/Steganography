using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Steganography
{
    public static class ToolConsole
    {
        private static ListBox list;

        public static void Bind(ListBox listbox)
        {
            list = listbox;
        }

        public static void Show()
        {
            list.Visible = true;
        }

        public static void Hide()
        {
            list.Visible = false;
        }

        public static void Write(string text)
        {
            list.Items.Add(text);
            if (list.Items.Count > (list.Height / list.ItemHeight))
            {
                list.Items.RemoveAt(0);
            }
            list.SelectedIndex = list.Items.Count - 1;
        }

        public static void Clear()
        {
            list.Items.Clear();
        }

    }

}
