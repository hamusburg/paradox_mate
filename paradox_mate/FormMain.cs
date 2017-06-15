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
                var unicode = Encoding.Unicode;
                var iso = Encoding.GetEncoding("iso-8859-1");
                var utf8 = Encoding.UTF8;
                var gbk = Encoding.GetEncoding("GBK");
                try
                {
                    var charSet = new HashSet<char>();
                    var files = PathUtil.ListFile(path).Where(x => x.EndsWith("english.yml"));
                    foreach (var f in files)
                    {

                        var u8Bytes = TextUtil.ReadFile(f);
                        var u8Str = Encoding.UTF8.GetString(u8Bytes);
                        // 原文本是utf8
                        var i = u8Str.IndexOf('\"');
                        while (i != -1)
                        {
                            var j = u8Str.IndexOf('\"', i + 1);
                            if (j == -1) break;
                            var subStr = u8Str.Substring(i + 1, j - i -1);
                            if (subStr.Count() > 0)
                            {
                                var bs = utf8.GetBytes(subStr);
                                // 原版exe处理是先utf8转unicode，再转latin1
                                byte[] uBytes = Encoding.Convert(utf8, unicode, bs);
                                byte[] latin1Bytes = Encoding.Convert(unicode, iso, uBytes);
                                var str1 = iso.GetString(latin1Bytes);

                                // 汉化版exe是gbk伪装成latin1
                                byte[] uBytes2 = Encoding.Convert(gbk, unicode, latin1Bytes);
                                var str2 = unicode.GetString(uBytes2);

                                char[] uChars = new char[unicode.GetCharCount(uBytes2, 0, uBytes2.Length)];
                                unicode.GetChars(uBytes2, 0, uBytes2.Length, uChars, 0);
                                foreach (var c in uChars)
                                {
                                    if (c > 255)
                                    {
                                        charSet.Add(c);
                                    }
                                }
                            }
                            i = u8Str.IndexOf('\"', j + 1);
                        }
                    }
                    string outStr = "";
                    var output = Path.Combine(path, "hehe.txt");
                    FileStream fs = new FileStream(output, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    int pos = 0;
                    foreach (var c in charSet)
                    {
                        outStr += c;
                        pos++;
                        if (pos == 64)
                        {
                            outStr += "\n";
                            pos = 0;
                        }
                    }
                    byte[] unicodeBytes = unicode.GetBytes(outStr);
                    sw.Write(outStr);
                    fs.Close();
                }
                catch { }
            }
            
                
        }
    }
}
