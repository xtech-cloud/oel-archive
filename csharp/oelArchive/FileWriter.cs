using System.IO;

namespace XTC.oelArchive
{
    public class FileWriter : StreamWriter
    {
        public void Open(string _filepath, bool _overwrite)
        {
            if(File.Exists(_filepath))
            {
                if(_overwrite)
                    File.Delete(_filepath);
                else
                    return;

            }
            stream_ = File.OpenWrite(_filepath);
            prepare();
        }
    }
}

