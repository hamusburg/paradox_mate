using System.IO;

namespace paradox_mate.Utils
{
    public class TextUtil
    {
        public static byte[] ReadFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] datas = new byte[fs.Length];
            fs.Read(datas, 0, datas.Length);
            fs.Close();
            return datas;
        }
    }
}
