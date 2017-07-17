using System.Collections.Generic;
using System.IO;
using System.Text;

namespace paradox_mate.Yml
{
    // yml文件格式：
    // l_english:
    //  key:num "val"
    public struct YmlNode
    {
        public string Key;
        public string Val;
        public int Num;
    }
    public class YmlFile
    {
        public static string Header = "l_english:";
        public Dictionary<string, YmlNode> Ymls = new Dictionary<string, YmlNode>();

        public void ReadOrigin(string file)
        {
            StreamReader sr = File.OpenText(file);
            string str;
            str = sr.ReadLine();
            // 第一行应该是Header
            if (str.Contains(Header))
            {
                while ((str = sr.ReadLine()) != null)
                {
                    // 用一个比较傻但是有效的方法，先找:，:前面是key，后面一位是num，后移两位到结尾是val
                    YmlNode node;
                    var i = str.IndexOf(':');
                    var i2 = str.IndexOf('#');
                    if (i != -1 && (i2 == -1 || i2 >= 3))
                    {
                        node.Key = str.Substring(0, i).TrimStart();
                        node.Num = int.Parse(str.Substring(i + 1, 1));
                        node.Val = str.Substring(i + 2).Trim();
                        Ymls[node.Key] = node;
                    }
                }
            }
            sr.Close();
        }

        public void ReadNew(string file)
        {
            StreamReader sr = File.OpenText(file);
            string str;
            str = sr.ReadLine();
            // 第一行应该是Header
            if (str.Contains(Header))
            {
                while ((str = sr.ReadLine()) != null)
                {
                    // 用一个比较傻但是有效的方法，先找:，:前面是key，后面一位是num，后移两位到结尾是val
                    var i = str.IndexOf(':');
                    var i2 = str.IndexOf('#');
                    if (i != -1 && (i2 == -1 || i2 >= 3))
                    {
                        YmlNode node;
                        var Key = str.Substring(0, i).TrimStart();
                        var Val = str.Substring(i + 2).Trim();
                        if (Ymls.TryGetValue(Key, out node))
                        {
                            node.Val = Val;
                            Ymls[Key] = node;
                        }
                    }
                }
            }
            sr.Close();
        }

        // 老版本的没有num，只能用来覆盖翻译
        public void ReadOld(string file)
        {
            StreamReader sr = File.OpenText(file);
            string str;
            str = sr.ReadLine();
            // 第一行应该是Header
            if (str.Contains(Header))
            {
                while ((str = sr.ReadLine()) != null)
                {
                    // 用一个比较傻但是有效的方法，先找:，:前面是key，后移两位到结尾是val
                    var i = str.IndexOf(':');
                    var i2 = str.IndexOf('#');
                    if (i != -1 && (i2 == -1 || i2 >= 3))
                    {
                        YmlNode node;
                        var Key = str.Substring(0, i).TrimStart();
                        var Val = str.Substring(i + 2).Trim();
                        if (Ymls.TryGetValue(Key, out node))
                        {
                            node.Val = Val;
                            Ymls[Key] = node;
                        }
                        
                    }
                }
            }
            sr.Close();
        }

        public void Write(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine(Header);
            foreach (var node in Ymls)
            {
                string v = node.Value.Val
                    .Replace("\\ n", "\\n")
                    .Replace("\\\\n", "\\n")
                    .Replace("\\忠", "\\n忠")
                    .Replace("\\如", "\\n如")
                    .Replace("\\但", "\\n但");
                sw.WriteLine(" " + node.Key + ":" + node.Value.Num.ToString() + " " + v);
            }
            sw.Flush();
            sw.Close();
        }
    }
}
