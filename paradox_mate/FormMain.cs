using paradox_mate.Utils;
using paradox_mate.Yml;
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

        private void button1_Click(object sender, EventArgs e)
        {
            var path0 = @"C:\QQDownload\origin\";
            var path1 = @"C:\QQDownload\z_1_22_1_0_stage1.yml\";
            var path2 = @"C:\QQDownload\z_1_22_1_0_stage2.yml\";
            var path3 = @"C:\QQDownload\test000\";
            var files = PathUtil.ListFile(path0).Where(x => x.EndsWith("english.yml"));
            foreach (var f in files)
            {
                YmlFile yml = new YmlFile();
                yml.ReadOrigin(f);
                var fn = Path.GetFileName(f);

                var f3 = path3 + fn;
                if (File.Exists(f3))
                {
                    yml.ReadOld(f3);
                }

                var f1 = path1 + fn;
                if (File.Exists(f1))
                {
                    yml.ReadNew(f1);
                }
                var f2 = path2 + fn;
                if (File.Exists(f2))
                {
                    yml.ReadNew(f2);
                }
                yml.Write(f);
            }
        }
    }
}
