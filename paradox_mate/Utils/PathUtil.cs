using System.IO;
using System.Reflection;

namespace paradox_mate.Utils
{
    public class PathUtil
    {
        public static string GetLocation()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public static string[] ListFile(string path)
        {
            var fileList = Directory.GetFiles(path);
            return fileList;
        }
    }
}
