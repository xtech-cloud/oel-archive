using System.IO;

namespace XTC.oelArchive
{
    public class MemoryReader : StreamReader
    {
        public void Open(byte[] _data)
        {
            stream_ = new MemoryStream(_data);
            parseHeader();
        }
    }
}

