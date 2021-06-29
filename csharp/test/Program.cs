using System;
using System.IO;
using XTC.oelArchive;
using System.Text;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string archiveFile = Path.Combine(Path.GetTempPath(), "archive.bin");
            // Write
            FileWriter writer = new FileWriter();
            writer.SetPassword("abcd");
            writer.Open(archiveFile, true);
            writer.Write("a.text", Encoding.UTF8.GetBytes("aaaaaaaaa"));
            writer.Write("b.text", Encoding.UTF8.GetBytes("bbb"));
            writer.Write("1/c.text", Encoding.UTF8.GetBytes("ccc"));
            writer.Flush();
            writer.Close();

            // Read

            FileReader fReader = new FileReader();
            fReader.SetPassword("abcd");
            fReader.Open(archiveFile);
            foreach (string entry in fReader.entries)
            {
                Console.WriteLine(entry);
                byte[] data = fReader.Read(entry);
                Console.WriteLine(Encoding.UTF8.GetString(data));
            }

            MemoryReader mReader = new MemoryReader();
            mReader.SetPassword("abcd");
            mReader.Open(File.ReadAllBytes(archiveFile));
            foreach (string entry in mReader.entries)
            {
                Console.WriteLine(entry);
                byte[] data = fReader.Read(entry);
                Console.WriteLine(Encoding.UTF8.GetString(data));
            }



        }
    }
}
