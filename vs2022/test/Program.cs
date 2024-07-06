using System;
using System.IO;
using XTC.OpenEL.Archive;
using System.Text;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string archiveFile = Path.Combine(Path.GetTempPath(), "archive.bin");
            // Write
            FileWriter fWriter = new FileWriter();
            fWriter.SetPassword("abcd");
            fWriter.Open(archiveFile, true);
            fWriter.Write("a.text", Encoding.UTF8.GetBytes("aaaaaaaaa"));
            fWriter.Write("b.text", Encoding.UTF8.GetBytes("bbb"));
            fWriter.Write("1/c.text", Encoding.UTF8.GetBytes("ccc"));
            fWriter.Flush();
            fWriter.Close();

            MemoryWriter mWriter = new MemoryWriter();
            mWriter.Open();
            mWriter.Write("c.text", Encoding.UTF8.GetBytes("ccccc"));
            mWriter.Write("d.text", Encoding.UTF8.GetBytes("ddddd"));
            mWriter.Write("2/f.text", Encoding.UTF8.GetBytes("fffff"));
            mWriter.Flush();
            byte[] data = mWriter.GetBytes();
            mWriter.Close();

            // Read

            FileReader fReader = new FileReader();
            fReader.SetPassword("abcd");
            fReader.Open(archiveFile);
            foreach (string entry in fReader.entries)
            {
                Console.WriteLine(entry);
                Console.WriteLine(Encoding.UTF8.GetString(fReader.Read(entry)));
            }

            MemoryReader mReader = new MemoryReader();
            mReader.Open(data);
            foreach (string entry in mReader.entries)
            {
                Console.WriteLine(entry);
                Console.WriteLine(Encoding.UTF8.GetString(mReader.Read(entry)));
            }



        }
    }
}
