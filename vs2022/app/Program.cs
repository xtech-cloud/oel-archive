using System.Runtime.Intrinsics.Wasm;
using System.Text;
using XTC.OpenEL.Archive;

namespace app
{
    internal class Program
    {
        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="_dir">目录</param>
        static void pack(string _dir)
        {
            if (!Directory.Exists(_dir))
            {
                Console.WriteLine("Error: dir not found");
                return;
            }
            FileWriter fWriter = new FileWriter();
            fWriter.Open(_dir + ".bin", true);
            foreach (var file in Directory.GetFiles(_dir, "*", SearchOption.AllDirectories))
            {
                var bytes = File.ReadAllBytes(file);
                var uri = Path.GetRelativePath(_dir, file).Replace("\\", "/");
                fWriter.Write(uri, bytes);
            }
            fWriter.Flush();
            fWriter.Close();
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="_file">文件</param>
        static void unpack(string _file)
        {
            if (!File.Exists(_file))
            {
                Console.WriteLine("Error: file not found");
                return;
            }

            var dir = _file.Remove(_file.Length - 4, 4);
            FileReader fReader = new FileReader();
            fReader.Open(_file);
            foreach (string entry in fReader.entries)
            {
                var file = Path.Combine(dir, entry);
                var dirEntry = Path.GetDirectoryName(file);
                Directory.CreateDirectory(dirEntry);
                var bytes = fReader.Read(entry);
                File.WriteAllBytes(file, bytes);
            }
        }

        static void printUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("Pack: oelArchive -c [dir]");
            Console.WriteLine("Unpack: oelArchive -x [file]");
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                printUsage();
                return;
            }

            if (args[0] == "-c")
                pack(args[1]);
            else if (args[0] == "-x")
                unpack(args[1]);
            else
                printUsage();
        }
    }
}
