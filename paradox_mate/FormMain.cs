using paradox_mate.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paradox_mate
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonSelectPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxPath.Text = dialog.SelectedPath;
            }
        }

        private void buttonPath1_Click(object sender, EventArgs e)
        {
            var path = textBoxPath.Text;
            if (path.Count() > 0)
            {
                try
                {
                    var charSet = new HashSet<char>();
                    var files = PathUtil.ListFile(path).Where(x => x.EndsWith("english.yml"));
                    foreach (var f in files)
                    {
                        var unicode = Encoding.Unicode;
                        var u8Bytes = TextUtil.ReadFile(f);
                        byte[] uBytes = Encoding.Convert(Encoding.UTF8, unicode, u8Bytes);
                        string sss = unicode.GetString(uBytes);
                        char[] uChars = new char[unicode.GetCharCount(uBytes, 0, uBytes.Length)];
                        unicode.GetChars(uBytes, 0, uBytes.Length, uChars, 0);
                        foreach (var c in uChars)
                        {
                            if (c > 255)
                            {
                                charSet.Add(c);
                            }
                        }

                        byte[] isoBytes = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, u8Bytes);
                        string uf8converted = Encoding.UTF8.GetString(isoBytes);
                    }
                }
                catch { }
            }
            
                
        }
    }
}
