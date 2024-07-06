using System.IO;

namespace XTC.OpenEL.Archive
{
    public class FileReader : StreamReader
    {
        public void Open(string _filepath)
        {
            if(!File.Exists(_filepath))
            {
                return;
            }

            stream_ = File.OpenRead(_filepath);
            parseHeader();
        }
    }
}

